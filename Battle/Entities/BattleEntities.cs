using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static DesignEnums;

[System.Serializable]
public class BattleEntities
{
    public BattleAction BattleAction;

    public Transform Transform { get; protected set; }
    public string Name { get; protected set; }
    public int CurrHP {  get; protected set; }
    public int MaxHP {  get; protected set; }

    // 현재 적용된 스탯 (버프/디버프 포함)
    public int ATK { get; protected set; } // 공격력
    public int SATK { get; protected set; } // 특수 공격력
    public int DEF { get; protected set; } // 방어력
    public int SDEF { get; protected set; } // 특수 방어력
    public int SPD { get; protected set; } // 속도
    public int Critical { get; protected set; } // 크리티컬 확률
    public int EVA { get; protected set; } // 회피율
    public int Level { get; protected set; } // 레벨
    public bool IsPlayer { get; protected set; } // 플레이어 여부
    public bool IsFlare { get; protected set; } // 플레어본 여부
    public int FS { get; protected set; } // 플레어 스탯
    public int SP { get; protected set; } // 무력화 수치

    // 버프가 적용되지 않은 기본 스탯
    public int BaseATK { get; protected set; }
    public int BaseSATK { get; protected set; }
    public int BaseDEF { get; protected set; }
    public int BaseSDEF { get; protected set; }
    public int BaseSPD { get; protected set; }
    public int BaseCritical { get; protected set; }
    public int BaseEVA { get; protected set; }

    //턴 카운트
    public int TurnCount { get; private set; }

    public BattleVisuals BattleVisuals { get; protected set; } 
    public EffectManager EffectManager { get; protected set; }
    public SkillInstance BasicAttack { get; protected set; } // 기본 스킬
    public SkillInstance SelectedSkill { get; set; } // 선택 스킬
    public SkillInstance FlareSkill { get; protected set; } // 플레어 스킬
    public List<SkillInstance> Skills { get; protected set; } = new List<SkillInstance>(); // 스킬 리스트

    public int Target { get; protected set; }

    public int IsStun { get; private set; } // 스턴 확인
    public bool IsDead { get; private set; } // 죽음 확인
    
    protected static System.Random rng = new System.Random(); // 더 정확한 확률 계산을 위해 Random 클래스 사용
    private float criticalDamage = 1.5f; // 치명타 피해 배율
    private const float DEFENSE_EFFICIENCY_CONSTANT = 300f; // 방어 효율 계수

    public List<Buff> ActiveBuffs { get; protected set; } // 활성화 버프 / 디버프 리스트
    public TraitController Trait { get; protected set; } // 특성
    public EmotionController Emotion { get; protected set; } // 감정
    public StressController Stress { get; protected set; } // 스트레스
    public HumanityController Humanity { get; protected set; } // 인간성
    public StaggerController Stagger { get; protected set; } // 무력화

    public void SetEntityValues(string name, int currHP, int maxHP, int atk, int sAtk, int def, int sDef, int spd, int critical, int eva, int level, bool isPlayer, bool isFlare = false) // 기본 스탯 값 설정
    {
        Name = name;
        CurrHP = currHP;
        MaxHP = maxHP;

        ATK = BaseATK = atk;
        SATK = BaseSATK = sAtk;
        DEF = BaseDEF = def;
        SDEF = BaseSDEF = sDef;
        SPD = BaseSPD = spd;
        Critical = BaseCritical = critical;
        EVA = BaseEVA = eva;

        Level = level;
        IsFlare = isFlare;
        IsPlayer = isPlayer;
        IsStun = 0;
        ActiveBuffs = new List<Buff>();

        TurnCount = 0;
    }

    public void SetBattleVisuals(BattleVisuals battleVisuals, EffectManager effectManager)
    {
        BattleVisuals = battleVisuals;
        EffectManager = effectManager;
    }

    public void SetTarget(int target) // 현재 타겟
    {
        Target = target;
    }

    public bool IsEvaded() // 회피 판정 (명중자가 사용)
    {
        int roll = rng.Next(0, 100);
        return roll < EVA;
    }

    public bool IsCriticalHit() // 크리티컬 판정 (공격자가 사용)
    {
        int roll = rng.Next(0, 100);
        return roll < Critical;
    }

    public async Task ApplyAttack(BattleEntities attacker, float skillPower , int staggerPower) // 물리 대미지 적용 (회피 및 크리티컬 판정 포함)
    {
        // 회피 판정
        if (IsEvaded())
        {
            if (EffectManager != null)
            {
                Vector3 damagePoint = BattleVisuals.GetPoint(PointType.Damage);
                await EffectManager.PlayDamageText("DamageText", damagePoint, TextType.Eva);
            }

            return;
        }

        // 데미지 계산
        float baseAttack = attacker.ATK * skillPower;

        // 방어력(DEF)에 따른 피해 감소율 계산
        float damageReduction = DEF / (DEF + DEFENSE_EFFICIENCY_CONSTANT);
        float damageAfterDefense = baseAttack * (1 - damageReduction);

        // 크리티컬 적용
        bool isCritical = attacker.IsCriticalHit();
        float finalDamage = isCritical ? damageAfterDefense * criticalDamage : damageAfterDefense;

        // 무력화 계산
        int finalStagger = staggerPower + attacker.SP;

        // 분노 감정 보정
        if (attacker.IsPlayer && attacker.Emotion.IsSelectEmotion(EmotionType.Anger))
        {
            finalDamage *= 1.5f;
            finalStagger = (int)(finalStagger * 1.5f);
        }

        // 최종 피해 및 무력화 적용
        TakeStagger(finalStagger);
        await TakeDamage(Mathf.Max(0, (int)finalDamage), isCritical);

        // 크리티컬 발생 시 추가 처리
        if (isCritical)
        {
            OnCriticalHitTaken();
        }

    }

    public async Task ApplySpecialAttack(BattleEntities attacker, float skillPower, int staggerPower) // 특수 대미지 적용
    {
        if (IsEvaded())
        {
            if (EffectManager != null)
            {
                Vector3 damagePoint = BattleVisuals.GetPoint(PointType.Damage);
                await EffectManager.PlayDamageText("DamageText", damagePoint, TextType.Eva);
            }

            return;
        }

        // 기본 공격력 설정
        float baseAttack = attacker.SATK * skillPower;

        // 특수 방어력에 따른 피해 감소율
        float damageReduction = SDEF / (SDEF + DEFENSE_EFFICIENCY_CONSTANT);
        float damageAfterDefense = baseAttack * (1 - damageReduction);

        // 크리티컬 및 최종 데미지 계산
        bool isCritical = attacker.IsCriticalHit();
        float finalDamage = isCritical ? damageAfterDefense * criticalDamage : damageAfterDefense;
        
        // 무력화 계산
        int finalStagger = staggerPower + attacker.SP;

        // 분노 보정
        if (attacker.IsPlayer && attacker.Emotion.IsSelectEmotion(EmotionType.Anger))
        {
            finalDamage *= 1.5f;
            finalStagger = (int)(finalStagger * 1.5f);
        }

        TakeStagger(finalStagger);
        await TakeDamage(Mathf.Max(0, (int)finalDamage), isCritical);

        // 크리티컬 발생 시 추가 처리
        if (isCritical)
        {
            OnCriticalHitTaken();
        }
    }

    public async Task ApplyFlareAttack(BattleEntities attacker, float skillPower, int staggerPower) // 약점 공격 적용
    {
        // 기본 공격력 설정
        float baseAttack = attacker.FS * skillPower;

        // 물리 방어력과 특수 방어력 중 더 낮은 값을 선택
        float lowerDefense = Mathf.Min(DEF, SDEF);

        // 선택된 낮은 방어력을 기준으로 피해 감소율 계산
        float damageReduction = lowerDefense / (lowerDefense + DEFENSE_EFFICIENCY_CONSTANT);

        float damageAfterDefense = baseAttack * (1 - damageReduction);

        // 크리티컬 및 최종 데미지 계산
        bool isCritical = attacker.IsCriticalHit();
        float finalDamage = isCritical ? damageAfterDefense * criticalDamage : damageAfterDefense;

        // 무력화 계산
        int finalStagger = staggerPower + attacker.SP;

        // 분노 보정
        if (attacker.IsPlayer && attacker.Emotion.IsSelectEmotion(EmotionType.Anger))
        {
            finalDamage *= 1.5f;
            finalStagger = (int)(finalStagger * 1.5f);
        }

        TakeStagger(finalStagger);
        await TakeDamage(Mathf.Max(0, (int)finalDamage), isCritical);

        // 크리티컬 발생 시 추가 처리
        if (isCritical)
        {
            OnCriticalHitTaken();
        }
    }

    protected void RecalculateStats() // 새로운 스탯 재계산 메서드
    {
        // 모든 스탯을 기본 값으로 초기화
        ATK = BaseATK;
        SATK = BaseSATK;
        DEF = BaseDEF;
        SDEF = BaseSDEF;
        SPD = BaseSPD;
        Critical = BaseCritical;
        EVA = BaseEVA;

        // 각 스탯에 대한 퍼센트 합계를 저장할 임시 변수
        float atkModifier = 0f;
        float defModifier = 0f;
        float satkModifier = 0f;
        float sdefModifier = 0f;
        float spdModifier = 0f;
        float criticalModifier = 0f;
        float evaModifier = 0f;

        // 활성화된 모든 버프 효과를 순회하며 다시 적용
        foreach (Buff buff in ActiveBuffs)
        {
            switch (buff.BuffType)
            {
                case BuffType.IncreaseATK:
                    atkModifier += buff.Amount;
                    break;
                case BuffType.DecreaseATK:
                    atkModifier -= buff.Amount;
                    break;
                case BuffType.IncreaseDEF:
                    defModifier += buff.Amount;
                    break;
                case BuffType.DecreaseDEF:
                    defModifier -= buff.Amount;
                    break;
                case BuffType.IncreaseSATK:
                    satkModifier += buff.Amount;
                    break;
                case BuffType.DecreaseSATK:
                    satkModifier -= buff.Amount;
                    break;
                case BuffType.IncreaseSDEF:
                    sdefModifier += buff.Amount;
                    break;
                case BuffType.DecreaseSDEF:
                    sdefModifier -= buff.Amount;
                    break;
                case BuffType.IncreaseSPD:
                    spdModifier += buff.Amount;
                    break;
                case BuffType.DecreaseSPD:
                    spdModifier -= buff.Amount;
                    break;
                case BuffType.IncreaseCritical:
                    criticalModifier += buff.Amount;
                    break;
                case BuffType.DecreaseCritical:
                    criticalModifier -= buff.Amount;
                    break;
                case BuffType.IncreaseEVA:
                    evaModifier += buff.Amount;
                    break;
                case BuffType.DecreaseEVA:
                    evaModifier -= buff.Amount;
                    break;
            }
        }

        ATK = (int)(BaseATK * (1f + atkModifier));
        DEF = (int)(BaseDEF * (1f + defModifier));
        SATK = (int)(BaseSATK * (1f + satkModifier));
        SDEF = (int)(BaseSDEF * (1f + sdefModifier));
        SPD = (int)(BaseSPD * (1f + spdModifier));
        Critical = (int)(BaseCritical * (1f + criticalModifier));
        EVA = (int)(BaseEVA * (1f + evaModifier));


        ATK = Mathf.Max(0, ATK);
        DEF = Mathf.Max(0, DEF);
        SATK = Mathf.Max(0, SATK);
        SDEF = Mathf.Max(0, SDEF);
        SPD = Mathf.Max(0, SPD);
        Critical = Mathf.Max(0, Critical);
        EVA = Mathf.Max(0, EVA);
    }

    public virtual void ApplyBuff(Buff buff) // 버프/디버프 적용
    {
        ActiveBuffs.Add(buff);
        RecalculateStats(); // 스탯 재계산 호출
        Logger.Log(buff.BuffType.ToString());
    }

    public void OnBuffRemoved() // 버프 해제 스탯 재계산
    {
        RecalculateStats();
    }

    public virtual void OnTurnStart() // 턴 시작
    {
        TurnCount++;

        int buffsBefore = ActiveBuffs.Count;

        for (int i = ActiveBuffs.Count - 1; i >= 0; i--)  // 버프&디버프 지속시간 감소
        {
            ActiveBuffs[i].OnTurnStart();
        }

        // 비활성화된 버프를 리스트에서 제거
        ActiveBuffs.RemoveAll(buff => !buff.IsActive);

        // 버프가 제거되었다면 스탯을 다시 계산
        if (ActiveBuffs.Count < buffsBefore)
        {
            OnBuffRemoved();
        }

        ReduceSkillCooldowns(); // 스킬 쿨타임 감소
    }

    public virtual void OnTurnEnd() // 턴 끝
    {
        for (int i = ActiveBuffs.Count - 1; i >= 0; i--) // 버프&디버프 효과 발동
        {
            ActiveBuffs[i].OnTurnEnd();
        }
    }

    public async Task DealDamageByyPercent(float aomunt) // 최대 체력 비례 대미지
    {
        float damage = MaxHP * aomunt;

        int bleedDamage = Mathf.Max(1, (int)damage);

        await TakeDamage(bleedDamage, false);
    }

    public async Task HealByPercent(float aomunt) // 최대 체력 비례 회복
    {
        float healAmount = MaxHP * aomunt;

        int finalHealAmount = Mathf.Max(1, Mathf.RoundToInt(healAmount));

        await Heal(finalHealAmount);
    }

    public async Task TakeDamage(int amount, bool isCritical) // 받은 데미지
    {
        CurrHP = Mathf.Max(0, CurrHP - amount);
        BattleVisuals.animationHandler.PlayHitAnimation();

        if (EffectManager != null)
        {
            // 실행할 비동기 작업들을 담을 리스트 생성
            List<Task> effectTasks = new List<Task>();

            // 파티클 이펙트 작업을 리스트에 추가
            effectTasks.Add(EffectManager.PlayEffectAsync("HitEffect", this, EffectSpawn.Hit));

            // 데미지 텍스트 작업을 리스트에 추가
            Vector3 damagePoint = BattleVisuals.GetPoint(PointType.Damage);
            effectTasks.Add(EffectManager.PlayDamageText("DamageText", damagePoint, TextType.Damage, amount, isCritical));

            // 크리티컬일 경우, 크리티컬 텍스트 작업을 리스트에 추가
            if (isCritical)
            {
                Vector3 textPoint = BattleVisuals.GetPoint(PointType.Damage) + Vector3.up * 0.5f;
                effectTasks.Add(EffectManager.PlayDamageText("DamageText", textPoint, TextType.Critical));
            }

            // 리스트에 담긴 모든 작업을 동시에 실행하고 완료될 때까지 기다림
            await Task.WhenAll(effectTasks);
        }


        if (CurrHP <= 0)
        {
            IsDead = true;
            BattleVisuals.animationHandler.PlayDeathAnimation();

            if(IsPlayer)
            {
                BattleVisuals.AwakenEffect(false);
                BattleVisuals.CollapseEffect(false);
                BattleVisuals.TranscendentEffect(false);
            }
            else
            {
                BattleVisuals.EnrageEffect(false);
            }
        }

        UpdateUI();
    }

    public async Task Heal(int amount) // 회복
    {
        if (IsDead) return;

        CurrHP = Mathf.Min(MaxHP, CurrHP + amount);

        if (EffectManager != null)
        {
            Vector3 damagePoint = BattleVisuals.GetPoint(PointType.Damage);
            await EffectManager.PlayDamageText("DamageText", damagePoint, TextType.Heal, amount);
        }

        UpdateUI();
    }

    public void ApplyStun(int num) // 기절 상태
    {
        IsStun = num;
        Logger.Log("기절");
        // 기절 애니메이션
    }

    public void ClearStun() // 스턴 회복
    {
        if(IsStun > 0)
            IsStun--;

        if(IsStun == 0)
        {
            if (Stagger != null)
            {
                Stagger.StaggerReset();
                BattleVisuals.ChangeStat(CurrHP, Stagger.CurrentSG);
            }
            UpdateUI();
            // 기절 상태 해제 애니메이션
        }
    }

    protected virtual void ReduceSkillCooldowns() // 쿨타임 감소
    {
        foreach (SkillInstance skill in Skills)
        {
            if (skill.CurrentCooldown > 0)
            {
                skill.CurrentCooldown--;
            }
        }
    }

    public virtual void TakeStagger(int amount) { } // 무력화 게이지 변화
    public virtual void OnCriticalHitTaken() { } // 크리티컬 공격을 당했을 때
    public virtual void UpdateUI() { } // UI 업데이트
    public virtual void EmotionAdditionalEffect() { } // 감정 활성화 부과 효과
}

[System.Serializable]
public class EnemyEntities : BattleEntities
{
    public IEnemyAI EnemyAI { get; private set; }
    public EnemyType EnemyType { get; private set; }

    public void SetEnemy(int maxSg, EnemyType enemyType, List<SkillInstance> skills)
    {
        Skills = skills;
        Stagger = new StaggerController(maxSg, this);
        EnemyType = enemyType;
        EnemyAI = CreateAIForType(enemyType); // 타입에 맞는 AI를 생성하고 할당합니다.
    }

    // 적 타입에 맞는 AI를 생성하는 새로운 메서드
    private IEnemyAI CreateAIForType(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Normal:
                return new NormalEnemyAI();
            case EnemyType.Elite:
                return new EliteEnemyAI();
            case EnemyType.Boss:
                return new BossEnemyAI(0.5f); // 50% 체력에서 격노
            default:
                return new NormalEnemyAI();
        }
    }

    public override void TakeStagger(int amount)
    {
        Stagger.AddStagger(amount);
    }

    public override void UpdateUI()
    {
        BattleVisuals.ChangeStat(CurrHP, Stagger.CurrentSG);
    }

    public override void OnCriticalHitTaken()
    {
        
    }
}

[System.Serializable]
public class PartyEntities : BattleEntities
{
    public CharacterInstance SourceInstance { get; private set; }

    private Effect currentEmotionEffect; // 현제 작동 중인 감정 이펙트
    public int TranscendenceTurns { get; private set; } // 초월 상태 남은 턴

    public void SetFlareborn(CharacterInstance sourceInstance, int ps, int currST, int currHG, int sp, SkillInstance basicAttack, List<SkillInstance> skills, Transform transform, SkillInstance flare, List<Trait> traits = null)
    {
        SourceInstance = sourceInstance;

        FS = ps;
        BasicAttack = basicAttack;
        Skills = skills;
        Transform = transform;
        FlareSkill = flare;
        SP = sp;

        Trait = new TraitController(this, traits);
        Humanity = new HumanityController(currHG);
        Stress = new StressController(currST, Humanity);
        Emotion = new EmotionController(Trait, Stress);

        Stress.OnStressMaxed += HandleStressMaxed;
        Humanity.OnHumanityDepleted += BecomeTranscendent;
    }

    public void SetAshborn(CharacterInstance sourceInstance, int currST, int currHG, int sp, SkillInstance basicAttack, List<SkillInstance> skills, Transform transform, List<Trait> traits = null)
    {
        SourceInstance = sourceInstance;

        FS = 0;
        BasicAttack = basicAttack;
        Skills = skills;
        Transform = transform;
        FlareSkill = null;
        SP = sp;

        Trait = new TraitController(this, traits);
        Humanity = null;
        Stress = new StressController(currST, Humanity);
        Emotion = new EmotionController(Trait, Stress);

        Stress.OnStressMaxed += HandleStressMaxed;
    }

    protected override void ReduceSkillCooldowns() // 쿨타임 감소
    {
        base.ReduceSkillCooldowns();

        if(IsFlare)
        {
            if(FlareSkill.CurrentCooldown > 0)
            {
                FlareSkill.CurrentCooldown--;
            }
        }
    }

    public override void OnCriticalHitTaken()
    {
        int stress = rng.Next(8, 12);
        Stress.AddStress(stress);
    }

    public void ActiveDesire() // 욕망 효과
    {
        if (Emotion.IsSelectEmotion(EmotionType.Desire))
        {
            Logger.Log("욕망");
            foreach (var skill in Skills)
            {
                if (skill.CurrentCooldown > 0)
                {
                    skill.CurrentCooldown = 0;
                }
            }
        }
    }

    public void ActivePleasure() // 즐거움 효과
    {
        if (Emotion.IsSelectEmotion(EmotionType.Pleasure))
        {
            List<Buff> buffs = BuffFactory.CreateBuffs(BuffName.Pleasure, this);

            ActiveBuffs.AddRange(buffs);
            RecalculateStats();
        }
    }

    public override void EmotionAdditionalEffect() // 감정 활성화 부가효과
    {
        if(!Emotion.SelectedEmotionForTurn.HasValue) return;

        switch(Emotion.SelectedEmotionForTurn.Value)
        {
            case EmotionType.Joy:
                Stress.ReduceStress(20);
                break;
            case EmotionType.Pleasure:
                ActivePleasure();
                Stress.ReduceStress(20);
                break;
            case EmotionType.Anger:
                Stress.AddStress(10);
                break;
            case EmotionType.Sorrow:
            case EmotionType.Affection:
            case EmotionType.Disgust:
                break;
            case EmotionType.Desire:
                ActiveDesire();
                break;
        }

        // 인간성 회복
        Humanity?.RecoverHumanity(5);
    }

    public async void SelectEmotion(EmotionType type)
    {
        if (Emotion.SelectedEmotionForTurn == type) // 감정 OFF
        {
            // 이펙트 반환
            EffectManager.ReturnContinuousEffect(type.ToString(), currentEmotionEffect);
            currentEmotionEffect = null;

            Emotion.UnSelectEmotion();
            return;
        }

        if (Emotion.IsEmotionSelectable(type)) // 감정 ON
        {
            if (Emotion.SelectedEmotionForTurn != null)
            {
                // 기존 이펙트 반환
                EffectManager.ReturnContinuousEffect(Emotion.SelectedEmotionForTurn.ToString(), currentEmotionEffect);
                currentEmotionEffect = null;
            }

            Emotion.SelectEmotionForTurn(type);

            // 이펙트 생성
            currentEmotionEffect = await EffectManager.GetContinuousEffect(type.ToString(), BattleVisuals.GetPoint(PointType.Ground));

            if (currentEmotionEffect != null)
            {
                var particle = currentEmotionEffect.GetComponentInChildren<ParticleSystem>();
                if (particle != null)
                {
                    particle.Play();
                }
            }

        }
    }

    public async Task OnTurnEndAsync()
    {
        base.OnTurnEnd();

        if (Emotion.SelectedEmotionForTurn != null)
        {
            // 이펙트 반환
            EffectManager.ReturnContinuousEffect(Emotion.SelectedEmotionForTurn.ToString(), currentEmotionEffect);
            currentEmotionEffect = null;

            Emotion.OnTurnEnd();
        }

        if(IsFlare)
        {
            if (Humanity.IsTranscendent)
            {
                TranscendenceTurns--; // 초월 턴 감소
                if (TranscendenceTurns <= 0)
                {
                    BattleVisuals.TranscendentEffect(false);
                    Humanity.SetTranscendent(false); // 초월 상태 해제
                    Humanity.RecoverHumanity(70); // 인간성 70으로 회복
                    ApplyStun(5); // 기절

                    if (Stress.MaxStress <= Stress.CurrentStress && !Stress.IsCollapse)
                    {
                        HandleStressMaxed();
                    }
                    else if(Stress.IsCollapse)
                    {
                        Collapse();
                    }
                }
            }
        }

        await Task.CompletedTask;
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();

        Trait?.OnTurnStart();
    }

    private void HandleStressMaxed() // 스트레스가 최대치에 도달했을 때 호출될 메서드
    {
        int roll = rng.Next(0, 2);

        if (roll == 0) // 각성
        {
            Awaken();
        }
        else // 붕괴
        {
            if(Stress.IsAwaken) ClearAwaken();

            Collapse();
        }
    }

    private void Awaken() // 각성 상태 처리
    {
        Logger.Log("각성");

        BattleVisuals.AwakenEffect(true);

        Stress.SetStress(60);
        Stress.SetAwaken(true);

        ActiveBuffs.Clear(); // 모든 버프 초기화

        List<Buff> buffs = BuffFactory.CreateBuffs(BuffName.Awaken, this);
        ActiveBuffs.AddRange(buffs);
        RecalculateStats();
    }

    public void ClearAwaken()
    {
        BattleVisuals.AwakenEffect(false);
        Stress.SetAwaken(false);
    }

    private void Collapse() // 붕괴 상태 처리
    {
        Logger.Log("붕괴");

        BattleVisuals.CollapseEffect(true);

        Stress.SetCollapsed(true);

        ActiveBuffs.Clear(); // 모든 버프 초기화

        List<Buff> buffs = BuffFactory.CreateBuffs(BuffName.Collapse, this);
        ActiveBuffs.AddRange(buffs);
        RecalculateStats();
    }

    private void BecomeTranscendent() // 초월 진입 처리
    {
        if (Humanity.IsTranscendent) return;

        if(Stress.IsCollapse)
        {
            BattleVisuals.CollapseEffect(false);
        }

        if (Stress.IsAwaken)
        {
            BattleVisuals.AwakenEffect(false);
        }

        Logger.Log("초월");

        BattleVisuals.TranscendentEffect(true);

        Humanity.SetTranscendent(true);

        ActiveBuffs.Clear(); // 모든 버프 초기화

        List<Buff> buffs = BuffFactory.CreateBuffs(BuffName.Transcendent, this);
        ActiveBuffs.AddRange(buffs);
        RecalculateStats();

        TranscendenceTurns = 3; // 3턴 뒤 기절
    }
}