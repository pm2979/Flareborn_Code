using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger_SubMenu : SceneChanger
{
    [SerializeField] private string startNewGameScene;
    [SerializeField] private string loadGameScene;
    [SerializeField] private Button StartNewGameButton;
    [SerializeField] private Button LoadGameButton;
    [SerializeField] private Button BackToMainMenuButton;

    private void Start()
    {
        StartNewGameButton.onClick.AddListener(OnStartNewGameButton);
        LoadGameButton.onClick.AddListener(OnLoadGameButton);
        BackToMainMenuButton.onClick.AddListener(OnBackToMainMenuButton);
    }
    private void OnDestroy()
    {
        StartNewGameButton.onClick.RemoveListener(OnStartNewGameButton);
        LoadGameButton.onClick.RemoveListener(OnLoadGameButton);
        BackToMainMenuButton.onClick.RemoveListener(OnBackToMainMenuButton);
    }

    public void OnStartNewGameButton()
    {
        if (isLoading) return;
        ChangeScene(startNewGameScene);
    }

    public void OnLoadGameButton()
    {
        if (isLoading) return;
        ChangeScene(loadGameScene);
    }
    public void OnBackToMainMenuButton()
    {
        if (isLoading) return;
        fadeAnim.Play("FadeToBlack");
        ChangeScene(sceneToLoad);
    }


}
