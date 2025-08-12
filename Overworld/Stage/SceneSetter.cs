using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public static class NPCQuestMemory
{
    // Key = NPC 이름, Value = 퀘스트 ID
    public static Dictionary<string, string> currentQuests = new Dictionary<string, string>();

    public static void SaveQuest(string npcName, string questId)
    {
        currentQuests[npcName] = questId;
        Debug.Log($"NPCQuestMemory: {npcName} has been assigned quest {questId}.");
    }

    public static string GetQuest(string npcName)
    {
        return currentQuests.TryGetValue(npcName, out string questId) ? questId : null;
    }

    public static void ClearQuest(string npcName)
    {
        if (currentQuests.ContainsKey(npcName))
            currentQuests.Remove(npcName);
    }
}


public static class NPCMemoryTracker
{
    public static HashSet<string> destroyedNPCs = new HashSet<string>();

    public static void MarkNPCAsDestroyed(string npcName, bool isPermenant)
    {
        if (isPermenant)
        {
            // 영구적으로 파괴된 NPC는 destroyedNPCs에 추가
            destroyedNPCs.Add(npcName);
        }
    }
}

public class SceneSetter : MonoBehaviour
{
    [SerializeField] private NPCManager npcManager;
    [SerializeField] private DontDestroyOnLoad dontDestroyOnLoadObject;
    [SerializeField] private NPC[] npcs;
    [SerializeField] private PlayerRespawn playerRespawn;
    [SerializeField] private string soundName = "Start";

    private bool hasSpawned = false;

    // 프로퍼티 
    public NPC[] Npcs => npcs;

    private void Awake()
    {
        if (npcManager == null)
            npcManager = FindFirstObjectByType<NPCManager>();

        SpawnDontDestoryOnLoadObject();
        
    }

    private void Start()
    {
        if (!string.IsNullOrEmpty(soundName))
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlayBGM(soundName);
        }
    }

    private void OnEnable()
    {
        Debug.Log($"[SceneSetter] OnEnable 호출됨 - 현재 씬: {SceneManager.GetActiveScene().name}");
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (!hasSpawned && SceneManager.GetActiveScene() == gameObject.scene)
        {
            hasSpawned = true;
            SpawnNpcs();
        }

        var uiSceneName = GameManager.Instance.uiScene;
        Debug.Log($"[SceneSetter] UIScene 존재 여부 확인: {uiSceneName}");

        if (!SceneManager.GetSceneByName(uiSceneName).isLoaded)
        {
            Debug.Log("[SceneSetter] UIScene 로드 시도");
            GameManager.Instance.LoadUIScene();
        }
        else
        {
            Debug.Log("[SceneSetter] UIScene 이미 로드됨");
        }

        playerRespawn.SetPlayerPosition();
    }

    private void OnDisable()
    {
        // 씬이 언로드될 때마다 NPC 메모리 트래커 초기화
        SceneManager.sceneLoaded -= OnSceneLoaded;
        hasSpawned = false;
        GameManager.Instance.isUISceneLoaded = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 로드될 때마다 NPC 스폰
        if (!hasSpawned && scene == gameObject.scene)
        {
            hasSpawned = true;
            SpawnNpcs();
        }
    }

    private void SpawnNpcs()
    {
        foreach (NPC npc in npcs)
        {
            // 이미 파괴된 NPC는 스폰하지 않음
            if (NPCMemoryTracker.destroyedNPCs.Contains(npc.NPCData.NPCName)) continue;

            // NPC의 스폰 위치를 가져와서 NPC를 해당 위치에 스폰 (npcManager의 자식으로 스폰)
            Vector3 spawnPosition = npc.SpawnPoint;

            NPC npcInstance = Instantiate(npc, spawnPosition, Quaternion.identity, npcManager.transform);

            // "(Clone)"이 붙은 NPC 이름을 제거
            npcInstance.name = npc.NPCData.NPCName;
        }
    }

    private void SpawnDontDestoryOnLoadObject()
    {
        var clone = GameObject.Find("DontDestroyOnLoad(Clone)");
        if (clone != null)
        {
            return;
        }
        Instantiate(dontDestroyOnLoadObject, Vector3.zero, Quaternion.identity);
    }
}
