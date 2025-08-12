using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class DungeonData
{
    /// <summary>
    /// 던전 ID
    /// </summary>
    public int key;

    /// <summary>
    /// 던전 이름
    /// </summary>
    public string DungeonName;

    /// <summary>
    /// 배틀 씬 이름
    /// </summary>
    public string BattleSceneName;

    /// <summary>
    /// 스폰 가능한 적 리스트
    /// </summary>
    public List<int> SpawnableEnemies;

    /// <summary>
    /// 입구 던전 구분
    /// </summary>
    public bool IsEntrance;

    /// <summary>
    /// 보스 던전 구분
    /// </summary>
    public bool IsBoss;

    /// <summary>
    /// 필드 구분
    /// </summary>
    public bool IsField;

}
public class DungeonDataLoader
{
    public List<DungeonData> ItemsList { get; private set; }
    public Dictionary<int, DungeonData> ItemsDict { get; private set; }

    public DungeonDataLoader(string path = "JSON/DungeonData")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, DungeonData>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<DungeonData> Items;
    }

    public DungeonData GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public DungeonData GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
