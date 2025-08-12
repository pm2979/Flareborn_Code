using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger_MainMenu : SceneChanger
{
    private void Update()
    {
        if (!isLoading && Input.anyKeyDown)
        {
            ChangeScene(sceneToLoad);
        }
    }
}
