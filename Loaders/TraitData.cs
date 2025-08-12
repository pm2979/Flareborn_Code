using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class TraitData
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 이름
    /// </summary>
    public string name;

    /// <summary>
    /// 조건ID
    /// </summary>
    public List<int> ConditionalID;

    /// <summary>
    /// 효과ID
    /// </summary>
    public List<int> EffectID;

    /// <summary>
    /// 설명
    /// </summary>
    public string Description;

}
public class TraitDataLoader
{
    public List<TraitData> ItemsList { get; private set; }
    public Dictionary<int, TraitData> ItemsDict { get; private set; }

    public TraitDataLoader(string path = "JSON/TraitData")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, TraitData>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<TraitData> Items;
    }

    public TraitData GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public TraitData GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
