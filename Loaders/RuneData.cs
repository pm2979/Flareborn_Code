using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class RuneData
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 노말 값
    /// </summary>
    public int NormalValue;

    /// <summary>
    /// 레어 값
    /// </summary>
    public int RareValue;

    /// <summary>
    /// 에픽 값
    /// </summary>
    public int EpicValue;

    /// <summary>
    /// 레전더리 값
    /// </summary>
    public int LegendaryValue;

    /// <summary>
    /// 능력 타입
    /// </summary>
    public DesignEnums.AbilityType AbilityType;

    /// <summary>
    /// 중앙값 수정자
    /// </summary>
    public float MedianModifier;

}
public class RuneDataLoader
{
    public List<RuneData> ItemsList { get; private set; }
    public Dictionary<int, RuneData> ItemsDict { get; private set; }

    public RuneDataLoader(string path = "JSON/RuneData")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, RuneData>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<RuneData> Items;
    }

    public RuneData GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public RuneData GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
