using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class SceneChanger_CutScene01 : SceneChanger
{
    public PlayableDirector playableDirector;

    public void LoadNextScene()
    {
        ChangeScene(sceneToLoad);
    }
}
