using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger_LoadGame: SceneChanger
{
    [SerializeField] private Button BackToSubMenuButton;

    private void Start()
    {
        BackToSubMenuButton.onClick.AddListener(OnBackToSubMenuButton);
    }

    private void OnDestroy()
    {
        BackToSubMenuButton.onClick.RemoveListener(OnBackToSubMenuButton);
    }

    public void OnBackToSubMenuButton()
    {
        Debug.Log("Back to SubMenu Button Clicked");
        if (isLoading) return;
        fadeAnim.Play("FadeToBlack");
        ChangeScene(sceneToLoad);
    }
}
