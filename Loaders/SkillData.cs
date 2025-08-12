using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class SkillData
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 이름
    /// </summary>
    public string Name;

    /// <summary>
    /// 직업 타입
    /// </summary>
    public List<DesignEnums.JobType> JobType;

    /// <summary>
    /// 스킬 타입
    /// </summary>
    public DesignEnums.SkillType SkillType;

    /// <summary>
    /// 공격 타입
    /// </summary>
    public DesignEnums.AttackType AttackType;

    /// <summary>
    /// 버프 타입
    /// </summary>
    public List<DesignEnums.BuffType> BuffType;

    /// <summary>
    /// 타겟 타입
    /// </summary>
    public DesignEnums.TargetType TargetType;

    /// <summary>
    /// 스킬 파워
    /// </summary>
    public float Power;

    /// <summary>
    /// 버프 파워
    /// </summary>
    public List<float> BuffPower;

    /// <summary>
    /// 무력화 수치
    /// </summary>
    public int SP;

    /// <summary>
    /// 코스트 타입
    /// </summary>
    public DesignEnums.CostType CostType;

    /// <summary>
    /// 설명
    /// </summary>
    public string Description;

    /// <summary>
    /// 코스트 값
    /// </summary>
    public int CostValue;

    /// <summary>
    /// 공격 횟수
    /// </summary>
    public int HitCount;

    /// <summary>
    /// 지속시간
    /// </summary>
    public int Duration;

    /// <summary>
    /// 재사용시간
    /// </summary>
    public int Cooldown;

    /// <summary>
    /// 궁극기 여부
    /// </summary>
    public bool IsUltimate;

    /// <summary>
    /// 애니메이션 이름
    /// </summary>
    public string AnimationKey;

    /// <summary>
    /// 이펙트 프리팹
    /// </summary>
    public string EffectPrefab;

    /// <summary>
    /// 이펙트 발생 위치
    /// </summary>
    public DesignEnums.EffectSpawn EffectSpawn;

}
public class SkillDataLoader
{
    public List<SkillData> ItemsList { get; private set; }
    public Dictionary<int, SkillData> ItemsDict { get; private set; }

    public SkillDataLoader(string path = "JSON/SkillData")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, SkillData>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<SkillData> Items;
    }

    public SkillData GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public SkillData GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
