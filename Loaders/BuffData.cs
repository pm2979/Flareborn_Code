using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class BuffData
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
    /// 버프 이름
    /// </summary>
    public DesignEnums.BuffName BuffName;

    /// <summary>
    /// 버프 타입
    /// </summary>
    public List<DesignEnums.BuffType> BuffType;

    /// <summary>
    /// 버프 파워
    /// </summary>
    public List<float> BuffPower;

    /// <summary>
    /// 설명
    /// </summary>
    public string Description;

    /// <summary>
    /// 지속시간
    /// </summary>
    public int Duration;

}
public class BuffDataLoader
{
    public List<BuffData> ItemsList { get; private set; }
    public Dictionary<int, BuffData> ItemsDict { get; private set; }

    public BuffDataLoader(string path = "JSON/BuffData")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, BuffData>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<BuffData> Items;
    }

    public BuffData GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public BuffData GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
