using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class LoadingScene : MonoBehaviour
{
    public static string nextSceneName;
    public static string targetPortalID;
    public static System.Action onComplete;

    public Slider progressBar; // UI 연결
    public TMP_Text progressText;
    
    private static LoadingScene instance;
    private void Awake()
    {
        instance = this;
    }

    public static void BeginLoading()
    {
        if (instance != null)
        {
            instance.StartCoroutine(instance.LoadNextScene());
        }
        else
        {
            Debug.LogError("[LoadingScene] 인스턴스가 존재하지 않습니다.");
        }
    }

    IEnumerator LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogError("[LoadingScene] nextSceneName이 비어 있음.");
            yield break;
        }

        Debug.Log($"[LoadingScene] 로딩 시작: {nextSceneName}");
    
        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = false;

        while (op.progress < 0.9f)
        {
            float p = Mathf.Clamp01(op.progress / 0.9f);

            if (progressBar != null)
            {
                progressBar.value = p;
            }
            else
            {
                Debug.LogWarning("[LoadingScene] progressBar가 null입니다.");
            }

            if (progressText != null)
            {
                progressText.text = $"{(int)(p * 100)}%";
            }
            else
            {
                Debug.LogWarning("[LoadingScene] progressText가 null입니다.");
            }

            yield return null;
        }


        // 최소 대기 시간
        yield return new WaitForSeconds(1f);

        op.allowSceneActivation = true;

        // 로딩 끝난 후 콜백 호출
        onComplete?.Invoke();
    }
}
