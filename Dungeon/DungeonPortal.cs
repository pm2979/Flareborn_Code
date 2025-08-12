using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonPortal : Portal
{
    public string spawnPointID = "DefaultSpawn";
    public bool goToNext = true;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (isOnCooldown) return;
        if (!other.CompareTag("Player")) return;

        isOnCooldown = true;
        Enter();
    }

    public override void Enter()
    {
        int currentKey = DungeonManager.Instance.CurrentDungeon.Data.key;
        int nextKey = currentKey + 1;

        var data = GameManager.Instance.DataManager.DungeonDataLoader.GetByKey(nextKey);
        if (data == null)
        {
            Debug.LogError($"[DungeonPortal] 다음 층 데이터가 없습니다. Key: {nextKey}");
            return;
        }

        // 다음 층 키와 스폰 지점 ID 저장
        PlayerPrefs.SetInt("NextDungeonKey", nextKey);
        PlayerPrefs.SetString("NextSpawnPointID", spawnPointID);
        PlayerPrefs.Save();

        // 씬 재로드
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}