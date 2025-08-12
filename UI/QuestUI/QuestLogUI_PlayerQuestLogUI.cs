using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestLogUI_PlayerQuestLogUI : QuestLogUI
{
    [Header("Player Quest Log UI")]
    [SerializeField] private QuestLogUIToggle questLogUIToggle;
    [SerializeField] private TextMeshProUGUI questGiverNameText;

    protected override void OnEnable()
    {
        base.OnEnable();
        AddOnClickListener();
    }

    protected override void AddOnClickListener()
    {
        base.AddOnClickListener();
        inProgressButton.onClick.AddListener(OnInProgressButton);
    }

    protected override void GetButtonTextComponents()
    {
        base.GetButtonTextComponents();

        inProgressButtonText = inProgressButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    protected override void RemoveClickListner()
    {
        base.RemoveClickListner();
        inProgressButton.onClick.RemoveListener(OnInProgressButton);
    }

    public override void OnAllQuestsButton()
    {
        base.OnAllQuestsButton();

        // 퀘스트 상태와는 관계 없이 모든 퀘스트를 표시
        foreach (Quest quest in questDataBase.AllQuests.Values)
        {
            QuestStateChange(quest);
        }

        ResetQuestLogInfo();
    }

    public override void OnNotStartedButton()
    {
        base.OnNotStartedButton();

        // 퀘스트 상태가 시작할 수 있는 상태인 퀘스트만 표시
        foreach (Quest quest in questDataBase.ByState.Red_QuestList.Values)
        {
            QuestStateChange(quest);
        }

        ResetQuestLogInfo();
    }

    public override void OnInProgressButton()
    {
        base.OnInProgressButton();

        // 퀘스트 상태가 진행 중이거나 완료할 수 있는 상태인 퀘스트만 표시
        foreach (Quest quest in questDataBase.ByState.Yellow_QuestList.Values)
        {
            QuestStateChange(quest);
        }

        ResetQuestLogInfo();
    }

    public override void OnCompletedButton()
    {
        base.OnCompletedButton();

        // 퀘스트 상태가 완료된 퀘스트만 표시
        foreach (Quest quest in questDataBase.ByState.Green_QuestList.Values)
        {
            QuestStateChange(quest);
        }

        ResetQuestLogInfo();
    }


    public override void SetQuestLogInfo(Quest quest)
    {
        base.SetQuestLogInfo(quest);

        // 퀘스트 수주 NPC
        questGiverNameText.text = quest.GetQuestGiverName(quest.QuestNPC);
    }

    protected override void ResetQuestLogInfo()
    {
         base.ResetQuestLogInfo();

        // 퀘스트 수주 NPC Text를 초기화
        questGiverNameText.text = string.Empty;
    }

    protected override void ResetAllButtonTextVisuals()
    {
        base.ResetAllButtonTextVisuals();
        inProgressButtonText.color = originalButtonColor;
    }
}
