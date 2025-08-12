using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CharacterData
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
    public DesignEnums.JobType JobType;

    /// <summary>
    /// 최대 체력
    /// </summary>
    public int MaxHp;

    /// <summary>
    /// 공격력
    /// </summary>
    public int ATK;

    /// <summary>
    /// 특수 공격력
    /// </summary>
    public int SATK;

    /// <summary>
    /// 방어력
    /// </summary>
    public int DEF;

    /// <summary>
    /// 특수 방어력
    /// </summary>
    public int SDEF;

    /// <summary>
    /// 속도
    /// </summary>
    public int SPD;

    /// <summary>
    /// 크리티컬
    /// </summary>
    public int Critical;

    /// <summary>
    /// 회피력
    /// </summary>
    public int EVA;

    /// <summary>
    /// 플레어 여부
    /// </summary>
    public bool IsFlare;

    /// <summary>
    /// 플레어 스탯
    /// </summary>
    public int FS;

    /// <summary>
    /// 스킬 리스트
    /// </summary>
    public List<int> Skills;

    /// <summary>
    /// 플레어 스킬
    /// </summary>
    public int FlareSkill;

    /// <summary>
    /// 특성 리스트
    /// </summary>
    public List<int> Traits;

    /// <summary>
    /// 추가 능력 타입
    /// </summary>
    public DesignEnums.AbilityType AbilityType;

    /// <summary>
    /// 배틀 프리팹
    /// </summary>
    public string BattlePrefab;

    /// <summary>
    /// 필드 프리팹
    /// </summary>
    public string OverworldPrefab;

}
public class CharacterDataLoader
{
    public List<CharacterData> ItemsList { get; private set; }
    public Dictionary<int, CharacterData> ItemsDict { get; private set; }

    public CharacterDataLoader(string path = "JSON/CharacterData")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, CharacterData>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<CharacterData> Items;
    }

    public CharacterData GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public CharacterData GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
