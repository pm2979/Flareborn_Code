// SavedEquippedRuneData.cs
using System.Collections.Generic;
using UnityEngine;
using static DesignEnums;

[System.Serializable]
public class SavedEquippedRuneData
{
    public int characterID;
    public List<int> equippedRuneKeys;
}