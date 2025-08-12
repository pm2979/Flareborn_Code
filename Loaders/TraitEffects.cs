using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class TraitEffects
{
    /// <summary>
    /// 조건ID
    /// </summary>
    public int key;

    /// <summary>
    /// 효과 타입
    /// </summary>
    public DesignEnums.TraitEffect EffectType;

    /// <summary>
    /// 파라미터
    /// </summary>
    public int Parameter;

    /// <summary>
    /// 감정 타입
    /// </summary>
    public DesignEnums.EmotionType EmotionType;

}
public class TraitEffectsLoader
{
    public List<TraitEffects> ItemsList { get; private set; }
    public Dictionary<int, TraitEffects> ItemsDict { get; private set; }

    public TraitEffectsLoader(string path = "JSON/TraitEffects")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, TraitEffects>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<TraitEffects> Items;
    }

    public TraitEffects GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public TraitEffects GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
