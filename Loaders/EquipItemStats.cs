using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class EquipItemStats
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 직업 타입
    /// </summary>
    public DesignEnums.JobType JobType;

    /// <summary>
    /// 체력
    /// </summary>
    public int HP;

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
    /// 플레어 스탯
    /// </summary>
    public int FS;

}
public class EquipItemStatsLoader
{
    public List<EquipItemStats> ItemsList { get; private set; }
    public Dictionary<int, EquipItemStats> ItemsDict { get; private set; }

    public EquipItemStatsLoader(string path = "JSON/EquipItemStats")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, EquipItemStats>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<EquipItemStats> Items;
    }

    public EquipItemStats GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public EquipItemStats GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
