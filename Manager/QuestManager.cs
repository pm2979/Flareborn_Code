using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// QuestState의 상태관리를 위한 퀘스트 매니저 클래스 (예 : 퀘스트 시작, 진행, 완료 등등을 체크하며 관리한다.)
public class QuestManager : MonoBehaviour
{
    private QuestEvents questEvents;
    private QuestDataBase questDataBase;
    private QuestRewardSystem questRewardSystem;

    [Header("Config")]
    [SerializeField] private bool loadQuestState = true;

    // 프로퍼티
    public QuestEvents QuestEvents => questEvents;
    public QuestDataBase QuestDataBase => questDataBase;

    private void OnEnable()
    {
        // 컴포넌트 그랩
        questDataBase = GetComponent<QuestDataBase>();
        questRewardSystem = GetComponent<QuestRewardSystem>();

        // 싱글톤 캐싱
        questEvents = GameEventsManager.Instance.questEvents;

        // 이벤트 구독
        questEvents.onStartQuest += StartQuest;
        questEvents.onAdvanceQuest += AdvanceQuest;
        questEvents.onCompleteQuest += CompleteQuest;

        questEvents.onQuestStepStateChange += QuestStepStateChange;
        questEvents.onUpdateQuestDataBase += UpdateQuestDataBase;
        questEvents.onCancelQuest += CancelQest;

        if (questDataBase.AllQuests != null) UpdateAllQuestStates();
    }

    private void Start()
    {
        UpdateAllQuestStates();
        UpdateAllQuestTo_CanStart();
    }

    private void OnDisable()
    {
        // #방어로직
        if (GameEventsManager.Instance == null) return;

        // 이벤트 구독 해제
        questEvents.onStartQuest -= StartQuest;
        questEvents.onAdvanceQuest -= AdvanceQuest;
        questEvents.onCompleteQuest -= CompleteQuest;

        questEvents.onQuestStepStateChange -= QuestStepStateChange;
        questEvents.onUpdateQuestDataBase -= UpdateQuestDataBase;
        questEvents.onCancelQuest -= CancelQest;
    }

    public void UpdateQuestDataBase()
    {
        // 퀘스터 데이터 베이스를 퀘스트의 상태에 따라 업데이트 한다.
        questDataBase.SortQuestByQuestState();
    }

    public void UpdateAllQuestStates()
    {
        // 모든 퀘스트들의 시작 상태를 브로드캐스트 한다.
        foreach (Quest quest in questDataBase.AllQuests.Values)
        {
            // # QuestStateChange Event 호출
            questEvents.QuestStateChangeEvent(quest);
        }
    }

    public void UpdateAllQuestTo_CanStart()
    {
        foreach (Quest quest in questDataBase.AllQuests.Values)
        {
            // 퀘스트가 시작 조건이 충족되지 않은 상태에서 요구사항이 충족되면 퀘스트를 시작 가능한 상태로 바꿔준다.
            if (quest.QuestState == E_QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.QuestInfo.id, E_QuestState.CAN_START);
            }
        }
    }

    public void ChangeQuestState(string id, E_QuestState state)
    {
        Quest quest = GetQuestByID(id);
        quest.QuestState = state;
        questEvents.QuestStateChangeEvent(quest);
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        // TODO - 퀘스트의 요구사항을 확인하는 로직을 구현한다.
        bool meetsRequirements = true;

        // 플레이어의 레벨 요구사항을 체크한다

        // 선행 퀘스트의 완료 여부를 체크한다
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.QuestInfo.questPrerequisites)
        {
            if (GetQuestByID(prerequisiteQuestInfo.id).QuestState != E_QuestState.FINISHED)
            {
                meetsRequirements = false;
                break;
            }
        }

        return meetsRequirements;
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestByID(id);
        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(id, E_QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestByID(id);

        // 다음 퀘스트 단계로 이동한다.
        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
        {
            // 현재 단계가 존재하면 해당 단계의 퀘스트 스텝을 인스턴스화한다.
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        else
        {
            // 모든 퀘스트 단계를 완료했으면 퀘스트를 완료 상태로 변경한다.
            ChangeQuestState(id, E_QuestState.CAN_FINISH);
        }
    }

    private void CompleteQuest(string id)
    {
        Quest quest = GetQuestByID(id);
        ClaimRewards(quest);
        ChangeQuestState(id, E_QuestState.FINISHED);
    }

    private void ClaimRewards(Quest quest)
    {
        int expReward = quest.QuestInfo.expReward;
        int goldReward = quest.QuestInfo.goldReward;

        // 퀘스트 보상을 지급한다.
        questRewardSystem.ClaimRewards(quest, expReward, goldReward);
    }

    private void CancelQest(Quest quest)
    {
        string id = quest.QuestInfo.id;

        foreach (Transform child in transform)
        {
            QuestStep questStep = child.GetComponent<QuestStep>();

            if (questStep.QuestId == id)
            {
                // 퀘스트 스텝을 제거한다.
                Destroy(child.gameObject);
            }
        }

        questDataBase.ResetQuest(id);
    }


    private void QuestStepStateChange(string id, int stepIndex, QuestStepState state)
    {
        Quest quest = GetQuestByID(id);
        quest.StoreQuestStepState(state, stepIndex);
        ChangeQuestState(id, quest.QuestState);
    }

    public Quest GetQuestByID(string id)
    {
        Quest quest = questDataBase.AllQuests[id];
        if (quest == null)
        {
            Debug.LogError("Quest with ID" + id + " not found.");
        }

        return quest;
    }
}
