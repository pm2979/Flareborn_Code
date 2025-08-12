using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class EnemyData
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
    /// 몬스터 타입
    /// </summary>
    public DesignEnums.EnemyType EnemyType;

    /// <summary>
    /// 최대 체력
    /// </summary>
    public int MaxHP;

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
    /// 무력화 수치
    /// </summary>
    public int SG;

    /// <summary>
    /// 스킬
    /// </summary>
    public List<int> Skills;

    /// <summary>
    /// 배틀 프리팹
    /// </summary>
    public string BattlePrefab;

    /// <summary>
    /// 필드 프리팹
    /// </summary>
    public string OverworldPrefab;

    /// <summary>
    /// 드랍 경험치
    /// </summary>
    public int DropExp;

    /// <summary>
    /// 드랍 골드
    /// </summary>
    public int DropGold;

    /// <summary>
    /// 드랍 아이템 키
    /// </summary>
    public List<int> ItemKey;

    /// <summary>
    /// 아이템 드랍 확률
    /// </summary>
    public List<float> DropRate;

}
public class EnemyDataLoader
{
    public List<EnemyData> ItemsList { get; private set; }
    public Dictionary<int, EnemyData> ItemsDict { get; private set; }

    public EnemyDataLoader(string path = "JSON/EnemyData")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, EnemyData>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<EnemyData> Items;
    }

    public EnemyData GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public EnemyData GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
