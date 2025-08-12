// SavedEquippedItemData.cs
using System.Collections.Generic;
using UnityEngine;
using static DesignEnums;

[System.Serializable]
public class SavedEquippedItemData
{
    public int characterID; // 장비를 장착한 캐릭터의 ID
    public List<int> equippedItemKeys;
}