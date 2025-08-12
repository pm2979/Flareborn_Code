using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[Serializable]
public class EnemyInstance
{
    public EnemyData EnemyData {  get; private set; }

    // 기본 정보
    public int ID => EnemyData.key;
    public string Name => EnemyData.Name;
    public DesignEnums.EnemyType EnemyType => EnemyData.EnemyType;

    // 기본 스탯
    public int MaxHp => EnemyData.MaxHP;
    public int ATK => EnemyData.ATK;
    public int SATK => EnemyData.SATK;
    public int DEF => EnemyData.DEF;
    public int SDEF => EnemyData.SDEF;
    public int SPD => EnemyData.SPD;
    public int Critical => EnemyData.Critical;
    public int EVA => EnemyData.EVA;
    public bool IsFlare => EnemyData.IsFlare;
    public int FS => EnemyData.FS;

    public int SG => EnemyData.SG;

    public int CurrentHp { get; private set; }

    [field: SerializeField] public List<SkillInstance> Skills { get; private set; }

    public EnemyInstance(EnemyData data)
    {
        EnemyData = data;
        CurrentHp = data.MaxHP;
        Skills = new List<SkillInstance>();

        foreach (int skill in EnemyData.Skills)
        {
            SetSkill(skill);
        }
    }

    public void SetSkill(int skillKey) // 스킬 장착
    {
        SkillInstance instance = SkillFactory.Create(skillKey);
        AddSkill(instance);
    }

    public void AddSkill(SkillInstance skill)
    {
        Skills.Add(skill);
    }

    public EnemyInstance Clone()
    {
        return new EnemyInstance(EnemyData);
    }

    public void LoadEnemyBattlePrefab(BattleSystem system, Vector3 pos, Action<GameObject> onLoaded) // 배틀 프리팹 생성
    {
        Addressables.LoadAssetAsync<GameObject>(EnemyData.BattlePrefab).Completed +=
            (AsyncOperationHandle<GameObject> _asyncOperation) =>
            {
                if (_asyncOperation.Status == AsyncOperationStatus.Succeeded)
                {
                    var obj = system.CreateEntities(_asyncOperation.Result, pos);
                    onLoaded?.Invoke(obj);
                }
                else
                {
                    onLoaded?.Invoke(null);
                }
            };
    }
    public void LoadOverworldPrefab(Vector3 pos, Action<GameObject> onLoaded)
    {
        Addressables.LoadAssetAsync<GameObject>(EnemyData.OverworldPrefab).Completed +=
            (AsyncOperationHandle<GameObject> _asyncOperation) =>
            {
                if (_asyncOperation.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject prefab = _asyncOperation.Result;
                    onLoaded?.Invoke(prefab);
                }
                else
                {
                    Debug.LogError($"Failed to load prefab for {Name}");
                    onLoaded?.Invoke(null);
                }
            };
    }

    public DropResult ProcessDrops()
    {
        var result = new DropResult
        {
            Exp = EnemyData.DropExp,
            Gold = EnemyData.DropGold
        };

        for (int i = 0; i < EnemyData.ItemKey.Count; i++)
        {
            int itemKey = EnemyData.ItemKey[i];
            float dropRate = EnemyData.DropRate[i];

            if (UnityEngine.Random.value <= dropRate)
            {
                result.ItemKeys.Add(itemKey);
            }
        }

        return result;
    }
}
