using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestLogScrollingList : MonoBehaviour
{
    [Header("Connected Components")]    
    [SerializeField] private QuestLogUI questLogUI;
    private bool isInitialized = false;

    [Header("Components")]
    [SerializeField] private GameObject contentParent;

    [Header("Rect Transforms")]
    [SerializeField] private RectTransform scrollRectTransform;
    [SerializeField] private RectTransform contentRectTransform;

    [Header("Quest Log Button")]
    [SerializeField] private GameObject questLogButtonPrefab;

    private Dictionary<string, QuestLogButton> questLogButtons = new Dictionary<string, QuestLogButton>();

    // TEST : 테스트용 코드
    // 테스트 하려면 QuestInfoSO의 id 필드를 퍼블릭 세터로 변경해야함.
    //private void Start()
    //{
    //    for (int i = 0; i < 20; i++)
    //    {
    //        QuestInfoSO questInfoTest = ScriptableObject.CreateInstance<QuestInfoSO>();
    //        questInfoTest.id = "Test" + i;
    //        questInfoTest.displayName = "Test Quest " + i;
    //        questInfoTest.questStepPrefabs = new GameObject[0];
    //        Quest quest = new Quest(questInfoTest);

    //        QuestLogButton questLogButton = CreateButtonIfNotExists(quest, () =>
    //        {
    //            Debug.Log("Selected Quest: " + quest.questInfo.displayName);
    //        });

    //        if (i == 0)
    //        {
    //            questLogButton.button.Select(); // 첫 번째 버튼을 선택 상태로 설정
    //        }
    //    }
    //}

    public QuestLogButton CreateButtonIfNotExists(Quest quest, UnityAction selectAction)
    {
        QuestLogButton questLogButton = null;

        if (!questLogButtons.ContainsKey(quest.QuestInfo.id))
        {
            // 버튼이 존재하지 않으면 새로 생성
            questLogButton = InstantiateQuestLogButton(quest, selectAction);
        }
        else
        {
            // 이미 존재하는 버튼을 가져옴
            questLogButton = questLogButtons[quest.QuestInfo.id];
        }
        selectAction?.Invoke();

        return questLogButton;
    }

    private QuestLogButton InstantiateQuestLogButton(Quest quest, UnityAction selectAction)
    {
        // 버튼을 생성
        QuestLogButton questLogButton = Instantiate(questLogButtonPrefab, contentParent.transform).
                                        GetComponent<QuestLogButton>();

        // 씬 내에서의 게임오브젝트 이름 설정
        questLogButton.gameObject.name = quest.QuestInfo.id;

        RectTransform buttonRectTransform = questLogButton.GetComponent<RectTransform>();

        // 버튼을 초기화하고 버튼이 선택되었을 때 호출될 액션을 설정 (퀘스트 이름, 퀘스트, 퀘스트 버튼 UI, 선택 액션)
        questLogButton.Initialize(quest.QuestInfo.displayName, quest, questLogUI.QuestButton, questLogUI, () => {
            selectAction();
            UpdateScrolling(buttonRectTransform);
        });

        // 버튼을 딕셔너리에 추가 (버튼 중복추가 방지용)
        questLogButtons[quest.QuestInfo.id] = questLogButton;

        return questLogButton;
    }

    private void UpdateScrolling(RectTransform buttonRectTransform)
    {
        // 선택한 버튼의 최소와 최대 Y사이즈를 계산한다
        float buttonYMin = Mathf.Abs(buttonRectTransform.anchoredPosition.y);
        float buttonYMax = buttonYMin + buttonRectTransform.rect.height;

        // 버튼들이 담겨있는 컨텐츠 영역의 최소와 최대 Y사이즈를 계산한다
        float contentYMin = contentRectTransform.anchoredPosition.y;
        float contentYMax = contentYMin + scrollRectTransform.rect.height;

        // 스크롤 다운 액션을 관리한다
        if (buttonYMax > contentYMax)
        {
            contentRectTransform.anchoredPosition = new Vector2(contentRectTransform.anchoredPosition.x,
                                                                buttonYMax - scrollRectTransform.rect.height);
        }
        // 스크롤 업 액션을 관리한다
        else if (buttonYMin < contentYMin)
        {
            contentRectTransform.anchoredPosition = new Vector2(contentRectTransform.anchoredPosition.x,
                                                                buttonYMin);
        }
    }

    public void ClearButtons()
    {
         foreach (var button in questLogButtons.Values)
        {
            Destroy(button.gameObject);
        }

         questLogButtons.Clear();
    }

}
