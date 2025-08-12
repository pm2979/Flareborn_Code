using UnityEngine;
using System.Collections;
using System.Linq;

public class DungeonManager : MonoSingleton<DungeonManager>
{
    public DungeonInstance CurrentDungeon { get; set; }
    public int CurrentDungeonKey { get; set; }

    private void Start()
    {
        LoadDungeonFromScene();
        StartCoroutine(InitializePlayerPosition());
        StartCoroutine(InitializePlayerRotation());
    }
    
    public void LoadDungeonFromScene()
    {
        int keyToLoad = 0;

        if (PlayerPrefs.HasKey("NextDungeonKey"))
        {
            keyToLoad = PlayerPrefs.GetInt("NextDungeonKey");
            PlayerPrefs.DeleteKey("NextDungeonKey");
        }
        else
        {
            var maker = Object.FindFirstObjectByType<DungeonSceneMaker>();
            if (maker != null)
            {
                keyToLoad = maker.dungeonKey;
            }
        }

        var data = GameManager.Instance.DataManager.DungeonDataLoader.GetByKey(keyToLoad);

        if (data == null)
        {
            Debug.LogError($"[DungeonManager] 잘못된 dungeonKey: {keyToLoad}");
            return;
        }

        CurrentDungeonKey = keyToLoad;
        CurrentDungeon = new DungeonInstance(data);

        if (data.IsField)
        {
            Debug.Log($"[DungeonManager] 필드 던전 로드: {data.DungeonName} (Key: {keyToLoad})");
        }
        else
        {
            Debug.Log($"[DungeonManager] 던전 로드: {CurrentDungeon.Name} (Key: {CurrentDungeonKey})");
        }
    }
    
    private IEnumerator InitializePlayerPosition()
    {
        yield return null;

        var player = PlayerManager.Instance.playerMovement;
        if (player == null)
        {
            Debug.LogError("PlayerManager.playerMovement 가 등록되지 않았습니다.");
            yield break;
        }

        if (PlayerPrefs.HasKey("NextSpawnPointID"))
        {
            string spawnID = PlayerPrefs.GetString("NextSpawnPointID");
            PlayerPrefs.DeleteKey("NextSpawnPointID");

            var spawnPoint = GameObject.FindObjectsOfType<SpawnPoint>()
                .FirstOrDefault(sp => sp.spawnID == spawnID);

            if (spawnPoint != null)
            {
                player.transform.position = spawnPoint.transform.position;
                player.transform.rotation = spawnPoint.transform.rotation;
                Debug.Log($"[DungeonManager] 플레이어를 스폰 포인트 '{spawnID}'로 이동시켰습니다.");
            }
            else
            {
                Debug.LogWarning($"[DungeonManager] 스폰 포인트 '{spawnID}'를 찾지 못했습니다.");
            }
        }
    }
    
    private IEnumerator InitializePlayerRotation()
    {
        yield return null;

        var player = PlayerManager.Instance.playerMovement;
        if (player == null)
        {
            Debug.LogError("Player를 찾을 수 없습니다.");
            yield break;
        }

        player.transform.rotation = Quaternion.identity;
        Debug.Log("플레이어 회전 초기화 완료");
    }
}
