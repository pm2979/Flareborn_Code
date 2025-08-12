using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ink.Runtime;
using UnityEngine;
using static DesignEnums;

[Serializable]
public abstract class SkillInstance
{
    public SkillData SkillData { get; private set; }

    [SerializeField] private string skillName; // 인스펙터에 표시될 필드
    public string Name => SkillData.Name; // 스킬 이름
    public float Power => SkillData.Power; // 스킬 파워
    public int SP => SkillData.SP;
    public List<float> BuffPowers => SkillData.BuffPower; // 버프 파워
    public SkillType SkillType => SkillData.SkillType;
    public AttackType AttackType => SkillData.AttackType;

    public TargetType TargetType => SkillData.TargetType;

    public CostType CostType => SkillData.CostType;
    public int CostValue => SkillData.CostValue;

    public string Description => SkillData.Description;

    public int HitCount => SkillData.HitCount;
    public int Cooldown => SkillData.Cooldown;
    public List<BuffType> BuffTypes => SkillData.BuffType;

    public int Duration => SkillData.Duration;
    public List<JobType> JobType => SkillData.JobType;
    public bool IsUltimate => SkillData.IsUltimate;
    public string AnimationKey => SkillData.AnimationKey;
    public string EffectPrefab => SkillData.EffectPrefab;

    public EffectSpawn EffectSpawn => SkillData.EffectSpawn;

    public int MaxCooldown => SkillData.Cooldown; // 스킬의 최대 쿨타임
    public int CurrentCooldown { get; set; } // 현재 남은 쿨타임


    public SkillInstance(SkillData skillData)
    {
        SkillData = skillData;
        CurrentCooldown = 0;

        skillName = skillData.Name;
    }

    public void ResetCooldown() // 쿨타임 초기화
    {
        CurrentCooldown = 0;
    }

    public virtual async Task Use(BattleEntities attacker, IEnumerable<BattleEntities> targets)
    {
        CoolTime();
        await ApplySkillCost(attacker);
    }

    private void CoolTime()
    {
        if (MaxCooldown > 0)
        {
            CurrentCooldown = MaxCooldown + 1;
        }
    }

    protected float ActiveJoy(EmotionController emotionController) // 기쁨 효과
    {
        if (emotionController.IsSelectEmotion(EmotionType.Joy))
        {
            Logger.Log("기쁨");
            return 1.5f;
        }

        return 1f;
    }

    protected float ActiveAffection(EmotionController emotionController) // 애정 효과
    {
        if (emotionController.IsSelectEmotion(EmotionType.Affection))
        {
            Logger.Log("애정");
            return 2f;
        }

        return 1f;
    }

    protected float ActiveDisgust(EmotionController emotionController) // 미음 효과
    {
        if (emotionController.IsSelectEmotion(EmotionType.Disgust))
        {
            Logger.Log("슬픔");
            return 2f;
        }

        return 1f;
    }

    protected async Task ApplySkillCost(BattleEntities attacker)
    {
        if (!attacker.IsPlayer) return;
        if (attacker.Emotion.IsSelectEmotion(EmotionType.Sorrow)) return;

        switch (CostType)
        {
            case CostType.HP:
                await attacker.TakeDamage(CostValue, false);
                break;
            case CostType.ST:
                attacker.Stress?.AddStress(CostValue);
                break;
            case CostType.HG:
                attacker.Humanity?.ReduceHumanity(CostValue);
                break;
        }

        attacker.UpdateUI();
    }
}

[Serializable]
public class AttackSkill : SkillInstance
{
    public AttackSkill(SkillData skillData) : base(skillData)
    {
    }

    public override async Task Use(BattleEntities atttacker, IEnumerable<BattleEntities> targets)
    {
        List<Task> tasks = new List<Task>();
        foreach (BattleEntities target in targets)
        {
            if (AttackType == AttackType.Physical)
            {
                tasks.Add(target.ApplyAttack(atttacker, Power, SP));
            }
            else if (AttackType == AttackType.Special)
            {
                tasks.Add(target.ApplySpecialAttack(atttacker, Power, SP));
            }
            else if (AttackType == AttackType.Flare)
            {
                tasks.Add(target.ApplyFlareAttack(atttacker, Power, SP));
            }
        }
        await Task.WhenAll(tasks);

        await base.Use(atttacker, targets);
    }
}

[Serializable]
public class HealSkill : SkillInstance
{
    public HealSkill(SkillData skillData) : base(skillData)
    {
    }

    public override async Task Use(BattleEntities atttacker, IEnumerable<BattleEntities> targets)
    {

        float power = IsUltimate ? Power * ((float)atttacker.SP / 10f) : Power;

        List<Task> tasks = new List<Task>();
        foreach (BattleEntities target in targets)
        {
            if(atttacker.IsPlayer) tasks.Add(target.HealByPercent(power * ActiveJoy(atttacker.Emotion)));
            else tasks.Add(target.HealByPercent(power));
        }
        await Task.WhenAll(tasks);
        await base.Use(atttacker, targets);
    }

}

[Serializable]
public class BuffSkill : SkillInstance
{
    public BuffSkill(SkillData skillData) : base(skillData) 
    {
    }

    public override async Task Use(BattleEntities atttacker, IEnumerable<BattleEntities> targets)
    {

        List<Task> tasks = new List<Task>();
        for (int i = 0; i < BuffTypes.Count; i++)
        {
            float buffPower = BuffPowers[i];

            foreach (BattleEntities target in targets)
            {
                if (BuffTypes[i] == BuffType.HealBuff)
                {
                    tasks.Add(target.HealByPercent(buffPower * ActiveJoy(atttacker.Emotion)));
                }

                Buff statbuff;

                if (atttacker.IsPlayer) statbuff = BuffFactory.CreateBuff(BuffTypes[i], buffPower * ActiveAffection(atttacker.Emotion), Duration + 1, target);
                else statbuff = BuffFactory.CreateBuff(BuffTypes[i], buffPower, Duration + 1, target);

                target.ApplyBuff(statbuff);
            }
        }
        await Task.WhenAll(tasks);
        await base.Use(atttacker, targets);
    }
}

[Serializable]
public class DeBuffSkill : SkillInstance
{
    public DeBuffSkill(SkillData skillData) : base(skillData) 
    {
    }

    public override async Task Use(BattleEntities atttacker, IEnumerable<BattleEntities> targets)
    {

        List<Task> tasks = new List<Task>();
        for (int i = 0; i < BuffTypes.Count; i++)
        {
            float buffPower = BuffPowers[i];

            foreach (BattleEntities target in targets)
            {
                if (BuffTypes[i] == BuffType.BleedBuff)
                {
                    tasks.Add(target.DealDamageByyPercent(buffPower));
                }

                Buff statbuff;

                if (atttacker.IsPlayer) statbuff = BuffFactory.CreateBuff(BuffTypes[i], buffPower * ActiveDisgust(atttacker.Emotion), Duration + 1, target);
                else statbuff = BuffFactory.CreateBuff(BuffTypes[i], buffPower, Duration + 1, target);

                target.ApplyBuff(statbuff);
            }
        }
        await Task.WhenAll(tasks);
        await base.Use(atttacker, targets);
    }
}

[Serializable]
public class StressSkill : SkillInstance
{
    public StressSkill(SkillData skillData) : base(skillData)
    {
    }

    public override async Task Use(BattleEntities atttacker, IEnumerable<BattleEntities> targets)
    {

        foreach (BattleEntities target in targets)
        {
            if(target.Stress != null)
            {
                target.Stress.AddStress((int)Power);
            }
        }
        await base.Use(atttacker, targets);
    }

}

