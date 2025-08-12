using UnityEngine;

public interface IDataPersistence
{
    void LoadData(GameData gameData);
    void SaveData(ref GameData gameData);
}

