using UnityEngine;

public class NPC : Interactable_NPC
{
    // 인터랙션 관련
    private PlayerInteractionHandler playerInteractionHandler;

    // 다이얼로그 관련
    private DialogueEvents dialogueEvents;
    private DialogueDataHandler dialogueDataHandler;
    private DialogueData defaultDialogue;
    private DialogueActivator defaultDialogueActivator;

    // 퀘스트 관련
    private QuestEvents questEvents;
    private QuestDataHandler questDataHandler;

    [SerializeField] private NPC_Data npcData;
    [SerializeField] private Vector3 spawnPoint;

    // 프로퍼티
    public NPC_Data NPCData => npcData;
    public Vector3 SpawnPoint => spawnPoint;

    private void OnEnable()
    {
        dialogueEvents = GameEventsManager.Instance.dialogueEvents;
        dialogueEvents.onQuestDialogueStarted += QuestDialogueStarted;
    }

    private void Start()
    {
        // 컴포넌트 그랩
        dialogueDataHandler = GetComponentInChildren<DialogueDataHandler>();
        questDataHandler = GetComponentInChildren<QuestDataHandler>();
        playerInteractionHandler = FindFirstObjectByType<PlayerInteractionHandler>();

        // 캐싱
        defaultDialogue = dialogueDataHandler.DefaultDialogue;
        defaultDialogueActivator = dialogueDataHandler.DefaultDialogue.DialogueActivator;

        questEvents = GameEventsManager.Instance.questEvents;
    }

    private void OnDisable()
    {
        // #방어로직
        if (GameEventsManager.Instance == null) return;
        dialogueEvents.onQuestDialogueStarted -= QuestDialogueStarted;
    }

    private void OnValidate()
    {
        npcData.NPCName = gameObject.name;
    }

    private void QuestDialogueStarted()
    {
        if ((IInteractable)this == playerInteractionHandler.CurrentInteractable)
        {
            Interact();
        }
    }

    #region [Interface Methods]
    public override void Interact()
    {
        // 현재 퀘스트가 설정되어 있지 않으면 기본 대화 시작
        if (questDataHandler.IsCurrentQuestSet == false)
        {
            if (!string.IsNullOrEmpty(defaultDialogue.DialogueKnotName))
            {
                switch (defaultDialogueActivator.CurrentDialogueState)
                {
                    case E_DialogueState.OutOfDialogue:
                        defaultDialogueActivator.EnterDialogue(defaultDialogue.DialogueKnotName);
                        break;
                    case E_DialogueState.InDialogue:
                        defaultDialogueActivator.continueOrExitStory();
                        break;
                }
            }
        }
        // 현재 퀘스트가 설정되어있다면 퀘스트 대화 시작 또는 퀘스트 상태에 따라 처리
        else if (questDataHandler.IsCurrentQuestSet)
        {
            if (!string.IsNullOrEmpty(questDataHandler.CurrentQuestData.DialogueKnotName))
            {
                switch (questDataHandler.CurrentQuestData.DialogueActivator.CurrentDialogueState)
                {
                    case E_DialogueState.OutOfDialogue:
                        questDataHandler.CurrentQuestData.DialogueActivator.EnterDialogue(questDataHandler.CurrentQuestData.DialogueKnotName);
                        break;
                    case E_DialogueState.InDialogue:
                        questDataHandler.CurrentQuestData.DialogueActivator.continueOrExitStory();
                        break;
                }
            }
        }
    }

    public override void ShowPrompt(bool show)
    {
        // 구현할 로직 없음
    }
    #endregion
}
