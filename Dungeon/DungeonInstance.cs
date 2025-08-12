using System.Collections.Generic;
using UnityEngine;

public class DungeonInstance
{
    public DungeonData Data { get; private set; }

    public string Name => Data.DungeonName;
    public string BattleSceneName => Data.BattleSceneName;
    public List<int> SpawnableEnemies => Data.SpawnableEnemies;
    public bool IsEntrance => Data.IsEntrance;
    public bool IsBoss => Data.IsBoss;
    public bool IsField => Data.IsField;

    public DungeonInstance(DungeonData data)
    {
        this.Data = data;
    }
}
