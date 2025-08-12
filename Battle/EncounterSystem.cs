using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EncounterSystem : MonoSingleton<EncounterSystem>
{
    [Header("조우 설정")]
    [SerializeField] private int maxAdditionalEnemies = 2;

    [Header("UI 씬")]
    [SerializeField] private string uiSceneName = "UIScene";
    [SerializeField] private string soundName = "Dungeon";
    
    [Header("카메라")]
    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private GameObject exitPortal;

    private bool isBattling = false;

    public GameObject overworldRoot;

    private List<GameObject> deactivatedObjects = new();
    
    private GameObject collidedEnemyGO;
    private EnemyInstance collidedEnemy;

    protected override void Awake()
    {
        base.Awake();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Start()
    {
        if(!string.IsNullOrEmpty(soundName))
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlayBGM(soundName);
        }
    }

    public void TriggerEncounter(EnemyInstance enemyInstance, GameObject enemyGO)
    {
        if (isBattling || enemyInstance == null || enemyGO == null) return;

        SoundManager.Instance.StopBGM();

        isBattling = true;
        collidedEnemy = enemyInstance;
        collidedEnemyGO = enemyGO;

        StartCoroutine(StartEncounter());
    }

    private IEnumerator StartEncounter()
    {
        isBattling = true;

        string unloadSceneName = SceneManager.GetActiveScene().name;

        // 카메라 비활성화
        if (mainCamera != null)
        {
            mainCamera.gameObject.SetActive(false);
        }

        CameraManager.Instance?.SetCameraActive(false);

        // UI 씬 로드
        if (!SceneManager.GetSceneByName(uiSceneName).isLoaded)
        {
            var uiLoadOp = SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);
            while (!uiLoadOp.isDone) yield return null;
        }

        // 오버월드 루트 오브젝트 비활성화
        if (overworldRoot != null)
        {
            foreach (Transform child in overworldRoot.transform)
            {
                if (child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                    deactivatedObjects.Add(child.gameObject);
                }
            }
        }
        
        // 전투 진입 전 Animator 상태 초기화
        var playerObj = PlayerManager.Instance.gameObject;

        // 전투 진입 전 Animator 상태 초기화
        var movement = playerObj.GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.IsAttacking = false;
            movement.CurrentState = MovementState.Idle;
            var animator = playerObj.GetComponent<PlayerController>()?._anim;
            if (animator != null)
            {
                string idleAnim = "Idle_" + movement.CurrentDirection.ToString();
                animator.Play(idleAnim, 0, 0f); // 강제 실행
            }
        }

        // DontDestroyOnLoad 플레이어 비활성화
        if (playerObj != null && playerObj.activeSelf)
        {
            playerObj.SetActive(false);
            deactivatedObjects.Add(playerObj);
        }

        // 스폰된 Enemy 비활성화
        foreach (var enemy in FindObjectsByType<OverWorldEnemy>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (enemy.gameObject.activeSelf)
            {
                enemy.gameObject.SetActive(false);
                deactivatedObjects.Add(enemy.gameObject);
            }
        }

        // 전투 참여 적 리스트 생성
        List<int> selectedEnemyIDs = new();
        selectedEnemyIDs.Add(collidedEnemy.ID);

        var currentDungeon = DungeonManager.Instance.CurrentDungeon;
        if (currentDungeon == null)
        {
            Debug.LogError("[EncounterSystem] 현재 던전 정보가 없습니다.");
            yield break;
        }

        List<int> allEnemyIDs = currentDungeon.SpawnableEnemies;

        int currentKey = DungeonManager.Instance.CurrentDungeonKey;

        if (currentKey == 101)
        {
            // 1층: 랜덤 일반 몬스터 3마리
            for (int i = 0; i < 2; i++)
            {
                int randIndex = UnityEngine.Random.Range(0, allEnemyIDs.Count);
                selectedEnemyIDs.Add(allEnemyIDs[randIndex]);
            }
        }
        else if (currentKey == 102)
        {
            // 2층: 엘리트 몬스터 2마리 고정
            selectedEnemyIDs.AddRange(allEnemyIDs);
        }
        else if (currentKey == 103)
        {
            // 3층: 보스 몬스터 1마리 고정
            selectedEnemyIDs.AddRange(allEnemyIDs);
        }
        
        // EnemyManager 초기화 및 적 생성
        var enemyManager = GameManager.Instance.EnemyManager;
        if (enemyManager == null)
        {
            Debug.LogError("EnemyManager not found");
            yield break;
        }

        enemyManager.GenerateEnemiesByEncounter(selectedEnemyIDs);

        // 전투 씬 로드
        string battleScene = currentDungeon.BattleSceneName;

        SceneManager.sceneLoaded += OnBattleSceneLoaded;
        yield return SceneManager.LoadSceneAsync(battleScene, LoadSceneMode.Additive);
    }

    private void OnBattleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var currentDungeon = DungeonManager.Instance.CurrentDungeon;
        if (currentDungeon == null) return;

        if (scene.name == currentDungeon.BattleSceneName)
        {
            SceneManager.sceneLoaded -= OnBattleSceneLoaded;
            // 전투 씬 로드 완료
        }
    }

    public void OnBattleEnd()
    {
        // 비활성화된 오브젝트들 다시 활성화
        foreach (var obj in deactivatedObjects)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        deactivatedObjects.Clear();

        // 카메라 다시 활성화
        if (mainCamera != null && !mainCamera.gameObject.activeSelf)
        {
            mainCamera.gameObject.SetActive(true);
        }
        CameraManager.Instance?.SetCameraActive(true);

        // 플레이어 따라가도록 재설정
        var playerObj = PlayerManager.Instance.gameObject;
        if (playerObj != null)
        {
            CameraManager.Instance?.SetFollowTarget(playerObj.transform);

            // 플레이어 상태 초기화
            var movement = playerObj.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.IsAttacking = false;
                movement.CurrentState = MovementState.Idle;

                // Idle 애니메이션 실행    
                movement.PlayAnimation(MovementState.Idle, movement.CurrentDirection);
            }

            // Animator 파라미터 초기화
            var playerController = playerObj.GetComponent<PlayerController>();
            if (playerController != null && playerController._anim != null)
            {
                var anim = playerController._anim;

                anim.SetBool(playerController.animationData.attackParameterHash, false);
                anim.SetBool(playerController.animationData.walkParameterHash, false);
            }
            
            // 히트박스 비활성화
            var hitboxes = playerObj.GetComponentsInChildren<Hitbox>(true);
            foreach (var hitbox in hitboxes)
            {
                hitbox.gameObject.SetActive(false);
            }
        }

        // 전투에 참여한 적 제거
        if (collidedEnemyGO != null)
        {
            Destroy(collidedEnemyGO);
            collidedEnemyGO = null;
            collidedEnemy = null;
        }
        
        ResetAllEnemies();

        // 던전이 필드가 아니라면 출구 포탈 활성화
        if (!DungeonManager.Instance.CurrentDungeon.IsField && exitPortal != null)
        {
            exitPortal.SetActive(true);
        }

        // UI 다시 오버월드 모드로
        UIManager.Instance?.SetOverworld();

        if (!string.IsNullOrEmpty(soundName))
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlayBGM(soundName);
        }

        isBattling = false;
    }
    
    private void ResetAllEnemies()
    {
        var enemies = GameObject.FindObjectsByType<OverWorldEnemy>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.Init(enemy.enemyInstance); // 상태머신 재초기화
            }
        }
    }
}

[System.Serializable]
public class Encounter
{
    public int enemyKey;
}