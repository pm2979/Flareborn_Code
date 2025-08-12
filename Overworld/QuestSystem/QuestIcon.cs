using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject requrementsNotMetToStart_Icon;
    [SerializeField] private GameObject canStart_Icon;
    [SerializeField] private GameObject requrementsNotMetToFinish_Icon;
    [SerializeField] private GameObject canFinish_Icon;

    public void SetState(E_QuestState newState, bool startPoint, bool finishPoint)
    {
        // 모두 보이지 않도록 설정
        requrementsNotMetToFinish_Icon.SetActive(false);
        canFinish_Icon.SetActive(false);
        requrementsNotMetToStart_Icon.SetActive(false);
        canStart_Icon.SetActive(false);

        switch (newState)
        {
            case E_QuestState.REQUIREMENTS_NOT_MET:
                if (startPoint) { requrementsNotMetToStart_Icon.SetActive(true); }
                break;

            case E_QuestState.CAN_START:
                if (startPoint) { canStart_Icon.SetActive(true); }
                break;

            case E_QuestState.IN_PROGRESS:
                if (finishPoint) { requrementsNotMetToFinish_Icon.SetActive(true);  }
                
                break;

            case E_QuestState.CAN_FINISH:
                if (finishPoint) { canFinish_Icon.SetActive(true); }
                break;

            case E_QuestState.FINISHED:
                break;

            default:
                Debug.LogWarning("Quest QuestState not regnized by switch statement: " + newState);
                break;
        }
    }
}
