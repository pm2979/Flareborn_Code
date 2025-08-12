using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private string arrivalPortalID; // 도착 씬에서 찾을 포탈 ID

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // 로딩씬 띄우고, 타깃 씬 로딩 및 플레이어 위치 세팅 후 로딩씬 언로드
    public void LoadSceneWithPortal(string targetScene, string targetPortalID, System.Action onComplete = null)
    {
        StartCoroutine(LoadLoadingSceneThenTarget(targetScene, targetPortalID, onComplete));
    }

    private IEnumerator LoadLoadingSceneThenTarget(string targetScene, string targetPortalID, System.Action onComplete)
    {
        
        // 1. 로딩씬을 Additive 모드로 로드 (겹치게)
        AsyncOperation loadLoading = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        while (!loadLoading.isDone)
            yield return null;

        // 2. 로딩씬에 로딩할 씬 정보 전달
        LoadingScene.nextSceneName = targetScene;
        LoadingScene.targetPortalID = targetPortalID;

        // 3. 로딩 완료 콜백 설정
        LoadingScene.onComplete = () =>
        {
            arrivalPortalID = targetPortalID;

            // 4. 플레이어 위치 세팅 (arrivalPortalID 기준)
            StartCoroutine(SetPlayerPosition());

            // 5. 로딩씬 언로드
            StartCoroutine(UnloadLoadingScene());

            onComplete?.Invoke();
        };
        
        LoadingScene.BeginLoading();
    }

    private IEnumerator SetPlayerPosition()
    {
        // 다음 프레임까지 대기하여 씬 전환 완료 대기
        yield return null;

        Portal targetPortal = FindArrivalPortal();
        Vector3 spawnPosition;
        Quaternion spawnRotation;

        if (targetPortal != null && targetPortal.spawnPoint != null)
        {
            spawnPosition = targetPortal.spawnPoint.position;
            spawnRotation = targetPortal.spawnPoint.rotation;
        }
        else if (targetPortal != null)
        {
            spawnPosition = targetPortal.transform.position + targetPortal.transform.forward * 1.5f;
            spawnRotation = Quaternion.LookRotation(targetPortal.transform.forward);
            Debug.LogWarning($"SpawnPoint가 설정되지 않았습니다. 포탈 앞쪽으로 스폰합니다.");
        }
        else
        {
            Debug.LogWarning($"도착 포탈 ID '{arrivalPortalID}'를 찾을 수 없습니다. 기본 위치 사용.");
            spawnPosition = Vector3.zero;
            spawnRotation = Quaternion.identity;
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerObj.transform.position = spawnPosition;
            playerObj.transform.rotation = spawnRotation;

            if (CameraManager.Instance != null)
            {
                CameraManager.Instance.SetFollowTarget(playerObj.transform);
            }
            else
            {
                Debug.LogWarning("CameraManager 인스턴스를 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("씬 전환 후 'Player' 태그 오브젝트를 찾을 수 없습니다.");
        }
        
        if (!SceneManager.GetSceneByName("UIScene").isLoaded)
        {
            SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
        }
    }

    private IEnumerator UnloadLoadingScene()
    {
        AsyncOperation unload = SceneManager.UnloadSceneAsync("LoadingScene");
        while (!unload.isDone)
            yield return null;
    }

    private Portal FindArrivalPortal()
    {
        Portal[] portals = Object.FindObjectsByType<Portal>(FindObjectsSortMode.None);
        foreach (var portal in portals)
        {
            if (portal.portalID == arrivalPortalID)
                return portal;
        }
        return null;
    }

    /// 포탈 쿨다운 시작
    public void StartPortalCooldown(string portalID, float cooldown)
    {
        StartCoroutine(PortalCooldownCoroutine(portalID, cooldown));
    }

    private IEnumerator PortalCooldownCoroutine(string portalID, float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        Portal[] portals = Object.FindObjectsByType<Portal>(FindObjectsSortMode.None);
        foreach (var portal in portals)
        {
            if (portal.portalID == portalID)
            {
                portal.ResetCooldown();
                break;
            }
        }
    }
}