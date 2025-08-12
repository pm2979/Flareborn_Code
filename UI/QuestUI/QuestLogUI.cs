using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum QuestLogState
{
    AllQuests,
    NotStarted,
    InProgress,
    Completed
}

public class QuestLogUI : MonoBehaviour
{
    protected QuestEvents questEvents;
    protected QuestDataBase questDataBase;

    [Header("Components")]
    [SerializeField] protected GameObject contentParent;
    [SerializeField] protected QuestLogScrollingList scrollingList;

    [Header("QuestDisplay")]
    [SerializeField] protected TextMeshProUGUI questDisplayNameText;
    [SerializeField] protected TextMeshProUGUI questStatusText;
    [SerializeField] protected TextMeshProUGUI rewardsTitleText;
    [SerializeField] protected TextMeshProUGUI goldRewardsText;
    [SerializeField] protected TextMeshProUGUI experienceRewardsText;
    [SerializeField] protected TextMeshProUGUI extraRewardsText;
    [SerializeField] protected TextMeshProUGUI questRequirementsTitleText;
    [SerializeField] protected TextMeshProUGUI levelRequirementsText;
    [SerializeField] protected TextMeshProUGUI questRequirementsText;

    [Header("Quest Sub Menus")]
    [SerializeField] protected Button allQuestsButton;
    [SerializeField] protected Button notStartedButton;
    [SerializeField] protected Button inProgressButton;
    [SerializeField] protected Button completedButton;
    [SerializeField] protected Color originalButtonColor;
    [SerializeField] protected Color selectedButtonColor;
    protected TextMeshProUGUI allQuestsButtonText;
    protected TextMeshProUGUI notStartedButtonText;
    protected TextMeshProUGUI inProgressButtonText;
    protected TextMeshProUGUI completedButtonText;

    [Header("Quest Accept / Cancel Button")]
    [SerializeField] protected QuestButton questButton; // 퀘스트 정보 할당의 역할

    [Header("Current Quest Log State")]
    [SerializeField] protected QuestLogState currentQuestLogState;

    protected Button firstSelectedButton;

    protected bool isInitializedOnStart = false;

    // 프로퍼티
    public QuestButton QuestButton => questButton;

    protected virtual void Awake()
    {
        GetButtonTextComponents();
    }

    protected virtual void OnEnable()
    {
        questEvents = GameEventsManager.Instance.questEvents;

        questEvents.onQuestStateChange += QuestStateChange;
    }

    protected virtual void Start()
    {
        questDataBase = GameManager.Instance.QuestManager.QuestDataBase;        
    }

    protected virtual void OnDisable()
    {
        // #방어로직
        if (GameEventsManager.Instance == null) return;

        questEvents.onQuestStateChange -= QuestStateChange;
    }

    protected virtual void OnApplicationQuit()
    {
        // 퀘스트 서브메뉴 버튼에 클릭 이벤트 제거
        RemoveClickListner();
    }

    protected void QuestStateChange(Quest quest)
    {
        // 퀘스트 리스트에 퀘스트 정보를 확인할 수 있는 퀘스트 버튼을 추가 (이미 추가되어있지 않다면)
        QuestLogButton questLogButton = scrollingList.CreateButtonIfNotExists(quest, () =>
        {
            SetQuestLogInfo(quest);
        });

        // 가장 처음 만들어진 퀘스트 리스트 버튼을 선택 상태로 설정
        if (firstSelectedButton == null)
        {
            firstSelectedButton = questLogButton.button;
        }

        // 퀘스트 상태에 따라 버튼의 색상을 변경
        questLogButton.SetState(quest.QuestState);
    }

    protected virtual void AddOnClickListener()
    {
        // 퀘스트 서브메뉴 버튼에 클릭 이벤트 추가
        allQuestsButton.onClick.AddListener(OnAllQuestsButton);
        notStartedButton.onClick.AddListener(OnNotStartedButton);
        completedButton.onClick.AddListener(OnCompletedButton);
    }

    protected virtual void GetButtonTextComponents()
    {
        // 퀘스트 서브메뉴 버튼의 텍스트 컴포넌트를 가져옴
        allQuestsButtonText = allQuestsButton.GetComponentInChildren<TextMeshProUGUI>();
        notStartedButtonText = notStartedButton.GetComponentInChildren<TextMeshProUGUI>();
        completedButtonText = completedButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    protected virtual void RemoveClickListner()
    {
        // 퀘스트 서브메뉴 버튼에 클릭 이벤트 제거
        allQuestsButton.onClick.RemoveListener(OnAllQuestsButton);
        notStartedButton.onClick.RemoveListener(OnNotStartedButton);
        completedButton.onClick.RemoveListener(OnCompletedButton);
    }

    public virtual void OnAllQuestsButton()
    {
        // 퀘스트 리스트 버튼들을 초기화
        scrollingList.ClearButtons();
        ResetQuestLogInfo();

        // 퀘스트 버튼 UI를 숨김
        questButton.HideButton();

        // QuestLogUI의 상태를 AllQuests로 설정
        SetQuestLogState(QuestLogState.AllQuests);

        // 선택된 버튼의 시각적 효과를 설정
        SetSelectedButtonVisual(QuestLogState.AllQuests);
    }

    public virtual void OnNotStartedButton()
    {
        // 퀘스트 리스트 버튼들을 초기화
        scrollingList.ClearButtons();
        ResetQuestLogInfo();

        // 퀘스트 버튼 UI를 숨김
        questButton.HideButton();

        // QuestLogUI의 상태를 NotStarted로 설정
        SetQuestLogState(QuestLogState.NotStarted);

        // 선택된 버튼의 시각적 효과를 설정
        SetSelectedButtonVisual(QuestLogState.NotStarted);
    }

    public virtual void OnInProgressButton()
    {
        // 퀘스트 리스트 버튼들을 초기화
        scrollingList.ClearButtons();
        ResetQuestLogInfo();

        // 퀘스트 버튼 UI를 숨김
        questButton.HideButton();

        // QuestLogUI의 상태를 InProgress로 설정
        SetQuestLogState(QuestLogState.InProgress);

        // 선택된 버튼의 시각적 효과를 설정
        SetSelectedButtonVisual(QuestLogState.InProgress);
    }

    public virtual void OnCompletedButton()
    {
        // 퀘스트 리스트 버튼들을 초기화
        scrollingList.ClearButtons();
        ResetQuestLogInfo();

        // 퀘스트 버튼 UI를 숨김
        questButton.HideButton();

        // QuestLogUI의 상태를 Completed로 설정
        SetQuestLogState(QuestLogState.Completed);

        // 선택된 버튼의 시각적 효과를 설정
        SetSelectedButtonVisual(QuestLogState.Completed);
    }

    public virtual void SetQuestLogInfo(Quest quest)
    {
        // 퀘스트 이름
        questDisplayNameText.text = quest.QuestInfo.displayName;

        // 퀘스트 상태
        questStatusText.text = quest.GetFullStatusText();

        // 퀘스트 조건
        questRequirementsTitleText.text = "퀘스트 조건";
        levelRequirementsText.text = "레벨 " + quest.QuestInfo.levelRequirement;
        questRequirementsText.text = "";
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.QuestInfo.questPrerequisites)
        {
            questRequirementsText.text += prerequisiteQuestInfo.displayName + "\n";
        }

        // 퀘스트 보상
        rewardsTitleText.text = "퀘스트 보상";
        goldRewardsText.text = quest.QuestInfo.goldReward + " 골드";
        experienceRewardsText.text = quest.QuestInfo.expReward + " EXP";
        extraRewardsText.text = quest.QuestInfo.extraRewards;
    }

    protected virtual void ResetQuestLogInfo()
    {
        // 퀘스트 정보를 초기화
        questDisplayNameText.text = "";
        questStatusText.text = "";
        rewardsTitleText.text = "";
        goldRewardsText.text = "";
        experienceRewardsText.text = "";
        extraRewardsText.text = "";
        questRequirementsTitleText.text = "";
        levelRequirementsText.text = "";
        questRequirementsText.text = "";
    }

    protected virtual void SetQuestLogState(QuestLogState state)
    {
        // 퀘스트 로그 상태를 설정
        currentQuestLogState = state;
    }

    protected virtual void SetSelectedButtonVisual(QuestLogState state)
    {
        TextMeshProUGUI selectedButtonText = null;

        // 선택된 버튼의 시각적 효과를 설정하기 전에 모든 버튼의 선택 상태를 해제
        ResetAllButtonTextVisuals();

        // 현재 퀘스트 로그 상태에 따라 선택된 버튼의 시각적 효과를 설정
        switch (state)
        {
            case QuestLogState.AllQuests:
                selectedButtonText = allQuestsButtonText;
                break;
            case QuestLogState.NotStarted:
                selectedButtonText = notStartedButtonText;
                break;
            case QuestLogState.InProgress:
                selectedButtonText = inProgressButtonText;
                break;
            case QuestLogState.Completed:
                selectedButtonText = completedButtonText;
                break;
        }

        if (selectedButtonText != null)
        {
            SelectedButtonTextVisual(selectedButtonText);
        }
    }

    protected virtual void SelectedButtonTextVisual(TextMeshProUGUI selectedButtontxt)
    {
        selectedButtontxt.color = selectedButtonColor;
    }

    protected virtual void ResetAllButtonTextVisuals()
    {
        // 모든 버튼의 선택 상태를 해제
        allQuestsButtonText.color = originalButtonColor;
        notStartedButtonText.color = originalButtonColor;
        completedButtonText.color = originalButtonColor;
    }
}
