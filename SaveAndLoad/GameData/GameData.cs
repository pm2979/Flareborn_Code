using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;

    // 플레이어 위치정보
    public Vector3 playerPosition;

    //플레이어 위치 씬 이름
    public string currentSceneName;

    // 인벤토리 전체 아이템 저장
    public List<SavedItemData> savedItems; // 인벤토리 전체 아이템 저장

    public List<SavedCharacterData> savedPartyMembers; // 파티원 정보 저장

    public List<SavedEquippedItemData> savedEquipments;

    public List<SavedEquippedRuneData> savedRunes;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        playerPosition = Vector3.zero;
        currentSceneName = "StartScene";
        savedItems = new List<SavedItemData>();
        savedPartyMembers = new List<SavedCharacterData>();
        savedEquipments = new List<SavedEquippedItemData>();
        savedRunes = new List<SavedEquippedRuneData>();
    }

    public int GetPercentageComplete()
    {
        return 0;
    }
}