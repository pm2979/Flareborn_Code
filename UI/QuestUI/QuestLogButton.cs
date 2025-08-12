using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class QuestLogButton : MonoBehaviour
{
    private QuestLogUI questLogUI;
    private Quest quest;

    public Button button { get; private set; }
    private TextMeshProUGUI buttonText;
    private Color buttonColor = new Color(15 / 255f, 15 / 255f, 15 / 255f, 1f);
    private UnityAction onSelectAction;

    private QuestButton questButton;
    
    // 프로퍼티
    public Quest Quest { get { return quest; } set { quest = value; } }


    // 버튼을 Instantiate하기 때문에 처음 생성되었을 때는 disabled 상태일 가능성이 있음.
    // 때문에 직접 Initialize메서드를 호출하여 onSelectAction을 설정해야 함.
    public void Initialize(string displayName, Quest quest, QuestButton questButtonUI, QuestLogUI questLog, UnityAction onSelectAction)
    {
        // 버튼에 퀘스트의 정보를 할당
        this.questLogUI = questLog;
        this.quest = quest;
        this.questButton = questButtonUI;

        this.button = GetComponent<Button>();
        this.onSelectAction = onSelectAction;
        
        this.buttonText = GetComponentInChildren<TextMeshProUGUI>();
        this.buttonText.text = displayName;

        onSelectAction?.Invoke();

        // onClick 이벤트에 추가하여 마우스 클릭으로 버튼이 선택되도록 설정
        this.button.onClick.AddListener(OnClickButton);
    }

    #region [ OnClickButton() ]
    public void OnClickButton()
    {
        // 버튼이 선택되었을 때 버튼과 글자의 색을 변경
        ColorBlock colors = button.colors;
        colors.selectedColor = Color.black;
        button.colors = colors;

        // 각 QuestLogButton에 담긴 퀘스트의 정보를 퀘스트 로그 UI에 보여줌
        questLogUI.SetQuestLogInfo(quest);

        // 퀘스트 버튼 UI 보이기 
        if (questButton is QuestCancelButton)
        {
            HideButton_QuestStateIsCanStart();

            ShowButton_QuestStateIsInPorgress();
        }
        else if (questButton is QuestAcceptButton)
        {
            questButton.ShowButton();
        }


        // 퀘스트 버튼에 퀘스트 정보 할당
        questButton.SetQuest(quest);
    }

    private void HideButton_QuestStateIsCanStart()
    {
        // 퀘스트 상태가 CAN_START인 퀘스트들은 QuestLogButton을 클릭했을 때 퀘스트 취소 버튼을 숨겨주도록 한다
        if (Quest != null && Quest.QuestState == E_QuestState.CAN_START)
        {
            questButton.HideButton();
        }
    }

    private void ShowButton_QuestStateIsInPorgress()
    {
        if (Quest != null && Quest.QuestState == E_QuestState.IN_PROGRESS)
        {
            questButton.ShowButton();
        }
    }

    #endregion

    public void OnSelect(BaseEventData eventData)
    {        
        // 버튼이 선택되었을 때 버튼과 글자의 색을 변경
        ColorBlock colors = button.colors;
        colors.selectedColor = Color.black;
        button.colors = colors;

        // 각 QuestLogButton에 담긴 퀘스트의 정보를 퀘스트 로그 UI에 보여줌
        questLogUI.SetQuestLogInfo(quest);

        // 퀘스트 버튼 UI 보이기 
        if (questButton is QuestCancelButton)
        {
            if (Quest != null && Quest.QuestState == E_QuestState.IN_PROGRESS)
            {
                questButton.ShowButton();
            }
        }
        else
        {
            questButton.ShowButton();
        }

        // 퀘스트 버튼에 퀘스트 정보 할당
        questButton.SetQuest(quest);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // 버튼이 선택해제되었을 때 버튼과 글자의 색을 원래대로변경
        ColorBlock colors = button.colors;
        colors.selectedColor = buttonColor;
        button.colors = colors;

        // 퀘스트 버튼 UI 숨기기
        questButton.HideButton();

        // 퀘스트 버튼 퀘스트 정보 초기화
        questButton.ClearQuest();
    }

    public void SetState(E_QuestState state)
    {
        switch(state)
        {
            case E_QuestState.REQUIREMENTS_NOT_MET:
            case E_QuestState.CAN_START:
                buttonText.color = Color.red;
                break;

            case E_QuestState.IN_PROGRESS:
            case E_QuestState.CAN_FINISH:
                buttonText.color = Color.yellow;
                break;

            case E_QuestState.FINISHED:
                buttonText.color = Color.green;
                break;

            default:
                Debug.LogWarning("Quest QuestState not recognized by SetState method: " + state);
                break;
        }
    }
}
