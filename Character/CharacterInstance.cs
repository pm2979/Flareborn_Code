using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static DesignEnums;

[Serializable]
public class CharacterInstance
{
    public CharacterData CharacterData { get; private set; }
    public JobInstance Job { get; private set; }

    // 장비
    public EquipmentController EquipmentController { get; private set; }

    // 룬
    public RuneController RuneController { get; private set; }

    // 최종 합산된 능력치를 저장
    private EquipItemStats FinalStats { get; set; }

    // 기본 정보
    public int ID => CharacterData.key;

    [SerializeField] private string characterName; // 인스펙터에 표시될 필드
    public string Name => CharacterData.Name;

    // 최종 스탯
    public int MaxHp => FinalStats.HP;
    public int ATK => FinalStats.ATK;
    public int SATK => FinalStats.SATK;
    public int DEF => FinalStats.DEF;
    public int SDEF => FinalStats.SDEF;
    public int SPD => FinalStats.SPD;
    public int Critical => FinalStats.Critical;
    public int EVA => FinalStats.EVA;
    public bool IsFlare => CharacterData.IsFlare; // 플레어본 여부
    public int FS => FinalStats.FS; // 플레어 스탯

    public int SP => Job.SP; // 무력화 피해

    // 런타임 상태
    public int CurrentHp { get; private set; }
    public int CurrentStress { get; private set; }
    public int CurrentHG { get; private set; }
    public int CurrentExp { get; private set; }
    public int Level { get; private set; }

    // 상태 플래그
    public int IsAwakened { get; private set; } // 각성 여부
    public bool IsCollapsed { get; private set; } = false; // 정신 붕괴 여부

    // 특성 및 감정
    [field: SerializeField] public List<Trait> Traits { get; private set; }

    // 스킬
    public List<int> allSkills = new List<int>();
    public SkillInstance BasicAttack => Job.BasicAttack;
    public SkillInstance FlareSkill;
    [field:SerializeField] public List<SkillInstance> EquippedSkills { get; private set; }

    // 생성자
    public CharacterInstance(CharacterData data, JobData job)
    {
        CharacterData = data;
        Job = new JobInstance(job);

        Traits = new List<Trait>();
        EquippedSkills = new List<SkillInstance>();

        EquipmentController = new EquipmentController();
        RuneController = new RuneController(this);
        FinalStats = new EquipItemStats();

        // 장비 및 룬 변경 이벤트
        EquipmentController.OnEquipmentChanged += OnStatSourceChanged;
        RuneController.OnRuneChanged += OnStatSourceChanged;

        // 초기 스탯 계산
        RecalculateFinalStats();

        CurrentHp = MaxHp;  // 장비 포함된 최대 체력으로 현재 체력 설정

        Level = 1;
        CurrentExp = 0;
        CurrentHG = 100;
        CurrentStress = 0;
        characterName = data.Name;

        foreach (int skill in CharacterData.Skills)
        {
            EquippedSetSkill(skill);
            allSkills.Add(skill);
        }

        FlareSkill = SkillFactory.Create(CharacterData.FlareSkill);

        foreach (int trait in CharacterData.Traits)
        {
            AddTrait(trait);
        }
    }

    private void OnStatSourceChanged(int i) // 룬 변경 콜백 메서드
    {
        RecalculateFinalStats();
    }

    private void OnStatSourceChanged(ItemType type = default) // 장비 변경 콜백 메서드
    {
        RecalculateFinalStats();
    }

    private void RecalculateFinalStats() // 최종 능력치 계산
    {
        // 컨트롤러에서 합산된 스탯
        EquipItemStats equipmentStats = EquipmentController.GetTotalEquippedStats();
        EquipItemStats runeStats = RuneController.GetTotalEquippedRuneStats();

        // 캐릭터 기본 + 직업 + 장비 + 룬 능력치 합산
        FinalStats.HP = CharacterData.MaxHp + Job.MaxHp + equipmentStats.HP + runeStats.HP;
        FinalStats.ATK = CharacterData.ATK + Job.Atk + equipmentStats.ATK + runeStats.ATK;
        FinalStats.SATK = CharacterData.SATK + Job.SATK + equipmentStats.SATK + runeStats.SATK;
        FinalStats.DEF = CharacterData.DEF + Job.Def + equipmentStats.DEF + runeStats.DEF;
        FinalStats.SDEF = CharacterData.SDEF + Job.SDEF + equipmentStats.SDEF + runeStats.SDEF;
        FinalStats.SPD = CharacterData.SPD + Job.Spd + equipmentStats.SPD + runeStats.SPD;
        FinalStats.Critical = CharacterData.Critical + Job.Critical + equipmentStats.Critical + runeStats.Critical;
        FinalStats.EVA = CharacterData.EVA + Job.Eva + equipmentStats.EVA + runeStats.EVA;
        FinalStats.FS = CharacterData.FS + runeStats.FS;
    }


    public void AddTrait(int traitKey) // 특성 획득
    {
        Trait trait = TraitFactory.CreatTrait(traitKey);
        Traits.Add(trait);
    }

    public void EquippedSetSkill(int skillKey) // 스킬 장착
    {
        SkillInstance instance = SkillFactory.Create(skillKey);
        EquippedSkill(instance);
    }

    public void EquippedSkill(SkillInstance skill)
    {
        EquippedSkills.Add(skill);
    }

    public void LoadCharacterBattlePrefab(BattleSystem sys, Vector3 pos, Action<GameObject> onLoaded) // 배틀 프리팹 생성
    {
        Addressables.LoadAssetAsync<GameObject>(CharacterData.BattlePrefab).Completed +=
            (AsyncOperationHandle<GameObject> _asyncOperation) =>
            {
                if (_asyncOperation.Status == AsyncOperationStatus.Succeeded)
                {
                    var obj = sys.CreateEntities(_asyncOperation.Result, pos);
                    onLoaded?.Invoke(obj);
                }
                else
                {
                    onLoaded?.Invoke(null);
                }
            };
    }

    public void LoadCharacterOverworldPrefab(PlayerManager manager, Vector3 pos, Action<GameObject> onLoaded) // 월드 프리팹 생성
    {
        Addressables.LoadAssetAsync<GameObject>(CharacterData.OverworldPrefab).Completed +=
            (AsyncOperationHandle<GameObject> _asyncOperation) =>
            {
                if (_asyncOperation.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject character = manager.CreateCharacter(_asyncOperation.Result, pos);

                    // 여기에서 LoadAll 호출
                    var joinable = character.GetComponent<JoinableCharacterScript>();
                    if (joinable != null)
                    {
                        joinable.Init(this);
                    }

                    onLoaded?.Invoke(character);
                }
                else
                {
                    Debug.LogError($"Failed to load prefab for {Name}");
                    onLoaded?.Invoke(null);
                }
            };
    }
    
    public void GainExp(int amount)
    {
        CurrentExp += amount;
        Debug.Log($"{Name}이(가) {amount}의 경험치를 획득했습니다. 현재 EXP: {CurrentExp}/{ExpTable.GetRequiredExp(Level)}");

        while (CurrentExp >= ExpTable.GetRequiredExp(Level))
        {
            CurrentExp -= ExpTable.GetRequiredExp(Level);
            LevelUp();
        }
    }
    
    private void LevelUp()
    {
        Level++;

        // 능력치 증가 => 어빌리티 스톤 시스템으로 사용 시 삭제
        // CharacterData.MaxHp += 10;
        // CharacterData.ATK += 2;
        // CharacterData.DEF += 2;
        // CharacterData.SDEF += 1;
        // CharacterData.SPD += 1;
        // CharacterData.Critical += 1;

        // 풀피 회복
        CurrentHp = MaxHp;
        
        // 레벨업 이펙트, 사운드 추가 예정
    }

    public void SetCurrentHp(int value)
    {
        CurrentHp = Mathf.Clamp(value, 0, MaxHp);
    }

    public void SetCurrentStress(int value)
    {
        CurrentStress = value;
    }

    public void SetCurrentHg(int value)
    {
        CurrentHG = value;
    }
}