using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class QuestDataHandler : MonoBehaviour
{
    private QuestEvents questEvents;
    private DialogueEvents dialogueEvents;
    private QuestManager questManager;
    private QuestDataHolder questDataHolder; // 퀘스트 SO들을 담고있는 Data Class
    private DialogueDataHandler npc_DefaultDialogue;

    [Header("Current Quest")]
    [SerializeField] private QuestData currentQuestData;
    private Quest currentQuest;
    private QuestData[] questDatas;

    [Header("Config")]
    [SerializeField] private QuestIcon questIcon;
    [SerializeField] private bool isCurrentQuestSet = false;

    // 프로퍼티
    public QuestData CurrentQuestData => currentQuestData;
    public bool IsCurrentQuestSet => isCurrentQuestSet;

    private void OnEnable()
    {
        // 싱글톤 캐싱
        questEvents = GameEventsManager.Instance.questEvents;
        dialogueEvents = GameEventsManager.Instance.dialogueEvents;

        // 이벤트 구독
        questEvents.onQuestStateChange += QuestStateChange;
        questEvents.onAcceptQuest += AcceptQuest;
        questEvents.onUnsetCurrentQuest += UnsetCurrentQuest;

        // NPC의 퀘스트 메모리에서 퀘스트를 불러옴
        SetCurrentQuestFromSave();
    }

    private void Start()
    {
        questManager = GameManager.Instance.QuestManager;

        // 컴포넌트 그랩
        questDataHolder = GetComponent<QuestDataHolder>();
        npc_DefaultDialogue = GetComponent<DialogueDataHandler>();
        questIcon = GetComponentInChildren<QuestIcon>();

        // 캐싱
        questDatas = questDataHolder.QuestDatas;

        SetQuestIcon(currentQuest); // 퀘스트 아이콘 초기화
    }

    private void OnDisable()
    {
        // #방어로직
        if (GameEventsManager.Instance == null) return;

        questEvents.onQuestStateChange -= QuestStateChange;
        questEvents.onAcceptQuest -= AcceptQuest;
        questEvents.onUnsetCurrentQuest -= UnsetCurrentQuest;
    }

    public void QuestStateChange(Quest quest)
    {
        SetQuestIcon(quest);
    }

    public void AcceptQuest(Quest quest)
    {
        foreach (QuestData questData in questDatas)
        {
            // 퀘스트 데이터의 QuestInfoSO와 현재 퀘스트의 QuestInfo를 비교하여 일치하는 경우 currentQuestData로 설정
            QuestInfoSO questInfoSO = questData.QuestInfoSO;
            if (quest.QuestInfo.id == questInfoSO.id)
            {
                currentQuestData = questData;
                currentQuest = quest; 

                // 현재 퀘스트 데이터를 설정하며 다시한번 초기화
                currentQuestData.Init();
                isCurrentQuestSet = true;

                // 해당 NPC의 currentQuestData를 저장
                NPCQuestMemory.SaveQuest(gameObject.transform.parent.name, questInfoSO.id);
                return;
            }
        }
    }

    private void UnsetCurrentQuest(string questId)
    {
        // 해당 퀘스트 아이디를 통해 퀘스트를 찾음
        Quest quest = questManager.GetQuestByID(questId);

        // 만약 퀘스트를 완료한 상태가 아니라면 (중도포기) Can_Start 상태로 변경
        if (quest.QuestState != E_QuestState.FINISHED)
        {
            questManager.ChangeQuestState(questId, E_QuestState.CAN_START);
        }

        // 변경된 퀘스트 상태를 업데이트
        questEvents.QuestStateChangeEvent(quest);

        // 현재 설정된 퀘스트 데이터를 초기화
        currentQuestData = null;
        isCurrentQuestSet = false;

        // 퀘스트 아이콘을 상태에 따라 업데이트
        SetQuestIcon(quest);

        // NPC의 퀘스트 메모리에서 해당 퀘스트를 제거
        NPCQuestMemory.ClearQuest(gameObject.transform.parent.name);
    }

    private void SetQuestIcon(Quest quest)
    {
        if (isCurrentQuestSet)
        {
            // 현재 퀘스트 포인트의 상태에 따라 아이콘 설정
            questIcon.SetState(quest.QuestState, currentQuestData.StartPoint, currentQuestData.FinishPoint);
        }
        else
        {
            // 현재 퀘스트가 설정되어 있지 않으면 아이콘을 비활성화
            questIcon.SetState(E_QuestState.FINISHED, false, false);
        }
    }

    public void SetCurrentQuestFromSave()
    {
        string rememberedQuestId = NPCQuestMemory.GetQuest(gameObject.transform.parent.name);
        if (!string.IsNullOrEmpty(rememberedQuestId))
        {
            Quest rememberedQuest = GameManager.Instance.QuestManager.GetQuestByID(rememberedQuestId);

            if (rememberedQuest != null)
                // 퀘스트가 존재하면 해당 퀘스트를 현재 퀘스트로 설정
                AcceptQuest(rememberedQuest);
        }
    }

    public void Set_IsCurrentQuest_True()
    {
        isCurrentQuestSet = true;
    }
}