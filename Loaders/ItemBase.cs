using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ItemBase
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
    /// 아이템 타입
    /// </summary>
    public DesignEnums.ItemType ItemType;

    /// <summary>
    /// 설명
    /// </summary>
    public string Description;

    /// <summary>
    /// 최대 스택
    /// </summary>
    public int MaxStack;

    /// <summary>
    /// 사용 가능 여부
    /// </summary>
    public bool IsUsable;

    /// <summary>
    /// 장비 타입
    /// </summary>
    public DesignEnums.EquipType EquipType;

    /// <summary>
    /// 판매 여부
    /// </summary>
    public bool IsSale;

    /// <summary>
    /// 판매가
    /// </summary>
    public int SalePrice;

    /// <summary>
    /// 구매가
    /// </summary>
    public int CostPrice;

    /// <summary>
    /// 아이콘
    /// </summary>
    public string Icon;

}
public class ItemBaseLoader
{
    public List<ItemBase> ItemsList { get; private set; }
    public Dictionary<int, ItemBase> ItemsDict { get; private set; }

    public ItemBaseLoader(string path = "JSON/ItemBase")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, ItemBase>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<ItemBase> Items;
    }

    public ItemBase GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public ItemBase GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
