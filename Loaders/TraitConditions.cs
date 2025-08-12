using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class TraitConditions
{
    /// <summary>
    /// 조건ID
    /// </summary>
    public int key;

    /// <summary>
    /// 조건 타입
    /// </summary>
    public DesignEnums.TraitCondition ConditionType;

    /// <summary>
    /// 파라미터
    /// </summary>
    public int Parameter;

}
public class TraitConditionsLoader
{
    public List<TraitConditions> ItemsList { get; private set; }
    public Dictionary<int, TraitConditions> ItemsDict { get; private set; }

    public TraitConditionsLoader(string path = "JSON/TraitConditions")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, TraitConditions>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<TraitConditions> Items;
    }

    public TraitConditions GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public TraitConditions GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
