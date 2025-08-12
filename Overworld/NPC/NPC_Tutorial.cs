using UnityEngine;

public class NPC_Tutorial : MonoBehaviour
{
    private TutorialManager tutorialManager;

    private void Start()
    {
        tutorialManager = GameProgressManager.Instance.tutorialManager;
    }

    private void OnDestroy()
    {
        if (tutorialManager != null)
        {
            tutorialManager.Tutorial_GetTeamMembers_Finished();
        }
    }
}
