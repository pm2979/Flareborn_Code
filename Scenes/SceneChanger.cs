using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SceneChanger : MonoBehaviour
{
    // Components
    [SerializeField] protected Animator fadeAnim;

    // Serialized Fields
    [SerializeField] protected string sceneToLoad;
    [SerializeField] protected float FadeDuration = 1f;

    protected bool isLoading = false;

    protected WaitForSeconds waitForSceneChange;

    protected void Awake()
    {
        fadeAnim = GetComponentInChildren<Animator>();
        waitForSceneChange = new WaitForSeconds(FadeDuration);
    }

    protected void ChangeScene(string sceneName)
    {
        fadeAnim.Play("FadeToBlack");
        StartCoroutine(WaitForFadeDuration(sceneName));
    }

    protected IEnumerator WaitForFadeDuration(string sceneName)
    {
        isLoading = true;
        yield return waitForSceneChange;
        SceneManager.LoadScene(sceneName);
        isLoading = false;
    }

    protected void ChangeSceneOfChoice(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

