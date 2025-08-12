using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPositionSaver : IDataPersistence
{
    private Transform playerTransform;

    public PlayerPositionSaver(Transform playerTransform)
    {
        this.playerTransform = playerTransform;

        if (playerTransform == null)
        {
            Debug.LogWarning("[PlayerPositionSaver] 생성자에서 playerTransform이 null입니다.");
        }
    }

    public void SaveData(ref GameData data)
    {
        if (playerTransform == null) return;

        data.playerPosition = playerTransform.position;
        data.currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        Debug.Log($"[저장] 위치: {data.playerPosition}, 씬: {data.currentSceneName}");
    }

    public void LoadData(GameData data)
    {
        //if (data == null)
        //{
        //    Debug.LogWarning("GameData is null");
        //    return;
        //}

        //transform.position = data.playerPosition;
        //Debug.Log($"[불러오기] 위치: {data.playerPosition}");
    }
}
