using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ConsumableEffects
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 양
    /// </summary>
    public int Amount;

    /// <summary>
    /// 타겟 타입
    /// </summary>
    public DesignEnums.TargetType TargetType;

    /// <summary>
    /// 소비 아이템 타입
    /// </summary>
    public DesignEnums.ConsumableType ConsumableType;

}
public class ConsumableEffectsLoader
{
    public List<ConsumableEffects> ItemsList { get; private set; }
    public Dictionary<int, ConsumableEffects> ItemsDict { get; private set; }

    public ConsumableEffectsLoader(string path = "JSON/ConsumableEffects")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, ConsumableEffects>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<ConsumableEffects> Items;
    }

    public ConsumableEffects GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public ConsumableEffects GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
