using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestLogUI_NPCQuestLogUI : QuestLogUI
{
    private DialogueEvents dialogueEvents;
    private Dictionary<string, Quest> currentNPC_QuestList;

    protected override void OnEnable()
    {
        base.OnEnable();

        // dialogueEvents가 null인 경우에만 할당을 한다.
        if (dialogueEvents == null)
            dialogueEvents = GameEventsManager.Instance.dialogueEvents;  

        dialogueEvents.onShowQuests += ShowQuests;
        questEvents.onAcceptQuest += AcceptQuest;

        contentParent.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();

        // 시작시에는 UI를 비활성화
        contentParent.SetActive(false);

        AddOnClickListener();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        // dialogueEvents가 null이 아닌 경우에만 이벤트를 제거한다.
        if (dialogueEvents != null)
            dialogueEvents.onShowQuests -= ShowQuests;
    }

    private void ShowQuests(string npcName)
    {
        contentParent.SetActive(true);

        // 현재 대화중인 NPC의 퀘스트 리스트를 가져와 Set.
        currentNPC_QuestList = SetCurrentNPCQuestList(npcName);

        // 퀘스트 데이터베이스를 퀘스트 상태에 따라 초기화한다
        questEvents.UpdateQuestDataBaseEvent();

        // 퀘스트를 보여줄 때에는 항상 전체 퀘스트 탭을 보여줌
        OnAllQuestsButton();
    }

    private void AcceptQuest(Quest quest)
    {
        if (contentParent == null) return;

        // 현재 대화중인 NPC의 퀘스트로그를 비활성화
        if (contentParent.activeSelf)
            contentParent.SetActive(false);
    }

    private Dictionary<string, Quest> SetCurrentNPCQuestList(string npcName)
    {
        // 현재 대화하고 있는 NPC의 퀘스트 목록을 가져와 currentNPC_QuestList에 할당
        return questDataBase.ByNPC.NPC_QuestLists[npcName]; 
    }

    public override void OnAllQuestsButton()
    {
        base.OnAllQuestsButton();

        // 퀘스트 상태와는 관계 없이 모든 퀘스트를 표시
        foreach (Quest quest in currentNPC_QuestList.Values)
        {
            QuestStateChange(quest);
        }
    }

    public override void OnNotStartedButton()
    {
        base.OnNotStartedButton();

        // 퀘스트 상태가 시작할 수 있는 상태인 퀘스트만 표시
        foreach (Quest quest in currentNPC_QuestList.Values)
        {
            if (quest.QuestState == E_QuestState.REQUIREMENTS_NOT_MET ||
                quest.QuestState == E_QuestState.CAN_START)
            {
                QuestStateChange(quest);
            }
        }
    }

    public override void OnInProgressButton()
    {
        base.OnInProgressButton();

        // 퀘스트 상태가 진행 중이거나 완료할 수 있는 상태인 퀘스트만 표시
        foreach (Quest quest in currentNPC_QuestList.Values)
        {
            if (quest.QuestState == E_QuestState.IN_PROGRESS ||
                quest.QuestState == E_QuestState.CAN_FINISH)
            {
                QuestStateChange(quest);
            }
        }
    }

    public override void OnCompletedButton()
    {
        base.OnCompletedButton();

        // 퀘스트 상태가 완료된 퀘스트만 표시
        foreach (Quest quest in questDataBase.ByState.Green_QuestList.Values)
        {
            if (quest.QuestState == E_QuestState.FINISHED)
            {
                QuestStateChange(quest);
            }
        }
    }

    public void OnExitButton()
    {
        contentParent.SetActive(false);

        // 퀘스트 리스트 버튼들을 초기화
        scrollingList.ClearButtons();

        // 퀘스트 정보를 표시해주는 UI를 초기화
        ResetQuestLogInfo();
    }
}
