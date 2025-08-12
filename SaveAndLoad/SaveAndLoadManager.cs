using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Ink.Parsed;

public class SaveAndLoadManager : MonoSingleton<SaveAndLoadManager>
{
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;

    private void Start()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        //InventorySaver 수동 등록
        var partyManager = GameManager.Instance?.PartyManager;
        if (partyManager?.Party?.Inventory != null)
        {
            var inventory = partyManager.Party.Inventory;
            dataPersistenceObjects.Add(new InventorySaver(inventory));
            Debug.Log("[SaveAndLoadManager] InventorySaver 수동 추가 완료");
        }

        // PlayerPositionSaver 수동 등록
        GameObject playerObj = GameObject.Find("Player_Overworld");
        if (playerObj != null)
        {
            dataPersistenceObjects.Add(new PlayerPositionSaver(playerObj.transform));
            Debug.Log("[SaveAndLoadManager] PlayerPositionSaver 수동 추가 완료 (이름 기반)");
        }

        // PartyMemberSaver 수동 등록
        if (partyManager != null)
        {
            dataPersistenceObjects.Add(new PartyMemberSaver(partyManager));
            Debug.Log("[SaveAndLoadManager] PartyMemberSaver 수동 추가 완료");
        }

        // EquippedItemSaver 수동 등록
        if (partyManager != null)
        {
            dataPersistenceObjects.Add(new EquippedItemSaver(partyManager));
            Debug.Log("[SaveAndLoadManager] EquippedItemSaver 수동 추가 완료");
        }

        // EquippedRuneSaver 수동 등록
        if (partyManager != null)
        {
            dataPersistenceObjects.Add(new EquippedRuneSaver(partyManager));
            Debug.Log("[SaveAndLoadManager] EquippedRuneSaver 수동 추가 완료");
        }

        LoadGame();
    }

    //private void OnApplicationQuit()
    //{
    //    SaveGame();
    //}

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        // TODO : 데이터 핸들러를 사용하여 저장된 데이터를 불러옴
        // 저장된 데이터가 없다면 NewGame()을 호출하여 새 게임을 시작함
        if (this.gameData == null)
        {
            Debug.Log("No saved game data found. Starting a new game.");
        }

        // TODO : 불러온 데이터가 필요한 다른 스크립트들에게 불러온 데이터를 제공한다
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }

    //public void SaveGame()
    //{
    //    // TODO : 데이터 핸들러를 사용하여 현재 데이터를 저장함
    //    foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
    //    {
    //        dataPersistenceObject.SaveData(ref gameData);
    //    }
    //}

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public void SaveToSlot(string slotId)
    {
        if (gameData == null)
            gameData = new GameData();

        Debug.Log($"[저장 전] dataPersistenceObjects 개수: {dataPersistenceObjects.Count}");

        foreach (var obj in dataPersistenceObjects)
        {
            Debug.Log($"[저장 중] 처리 중인 객체: {obj.GetType().Name}");
            obj.SaveData(ref gameData);
        }

        // 디버그 출력
        Debug.Log($"=== 저장 데이터 확인 ===");
        Debug.Log($"playerPosition: {gameData.playerPosition}");
        Debug.Log($"lastUpdated: {gameData.lastUpdated}");
        Debug.Log($"======================");

        SaveDataHandler.SaveToFile(gameData, slotId);
    }

}
