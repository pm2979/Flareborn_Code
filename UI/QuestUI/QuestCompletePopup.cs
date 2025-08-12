using System.Collections;
using TMPro;
using UnityEngine;

public class QuestCompletePopup : MonoBehaviour
{
    private Animator animator;
    private WaitForSeconds waitForSeconds;
    private Coroutine questCompleteCoroutine;
    [SerializeField] private TextMeshProUGUI completeQuestPopup;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        completeQuestPopup = GetComponentInChildren<TextMeshProUGUI>();
        waitForSeconds = new WaitForSeconds(2f);
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void Start()
    {
        completeQuestPopup.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
        }
    }

    private void QuestStateChange(Quest quest)
    {
       if (quest.QuestState == E_QuestState.CAN_FINISH)
       {
           if (questCompleteCoroutine != null)
               StopCoroutine(questCompleteCoroutine);

           questCompleteCoroutine = StartCoroutine(QuestCompletePopupCoroutine());
       }
    }

    private IEnumerator QuestCompletePopupCoroutine()
    {
        completeQuestPopup.gameObject.SetActive(true);
        animator.Play("CompleteQuestPopup");
        yield return waitForSeconds; // 2초 후에 팝업을 숨김
        completeQuestPopup.gameObject.SetActive(false);
    }
}
