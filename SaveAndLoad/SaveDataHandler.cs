using UnityEngine;
using System.IO;

public static class SaveDataHandler
{
    private static readonly string savePath = Application.persistentDataPath;

    public static void SaveToFile(GameData data, string slotId)
    {
        data.lastUpdated = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        string json = JsonUtility.ToJson(data, true);

        Debug.Log($"=== JSON 저장 확인 ===");
        Debug.Log($"Position: {data.playerPosition}");
        Debug.Log($"Scene: {data.currentSceneName}");
        Debug.Log($"JSON 길이: {json.Length} 문자");
        Debug.Log($"====================");

        string fullPath = Path.Combine(Application.persistentDataPath, slotId + ".json");
        File.WriteAllText(fullPath, json);

        Debug.Log($"파일 저장 완료: {fullPath}");
    }

    public static GameData LoadFromFile(string slotId)
    {
        string path = Path.Combine(savePath, slotId + ".json");
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<GameData>(json);
    }
}


