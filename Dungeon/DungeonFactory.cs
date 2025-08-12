using UnityEngine;

public static class DungeonFactory
{
    public static DungeonInstance Create(int dungeonKey)
    {
        var data = GameManager.Instance.DataManager.DungeonDataLoader.GetByKey(dungeonKey);
        if (data == null)
        {
            Debug.LogError($"[DungeonFactory] 던전 키 {dungeonKey} 에 해당하는 데이터를 찾을 수 없습니다.");
            return null;
        }

        return new DungeonInstance(data);
    }
  
}
