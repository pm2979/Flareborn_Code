using UnityEngine;

[RequireComponent(typeof(Collider))]
[System.Serializable]
public class QuestData
{
    private QuestEvents questEvents;

    [Header("Dialogue")]
    [SerializeField] private string dialogueKnotName;
    [SerializeField] private DialogueActivator dialogueActivator;

    [Header("Quest")]
    [SerializeField] private string questId;
    [SerializeField] private QuestInfoSO questInfoSO;
    [SerializeField] private E_QuestState currentQuestState;

    [Header("Configuration")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = false;   

    // 프로퍼티
    public string DialogueKnotName { get { return dialogueKnotName; } set { dialogueKnotName = value; } }
    public DialogueActivator DialogueActivator => dialogueActivator;
    public QuestInfoSO QuestInfoSO => questInfoSO;
    public string QuestId => questId;
    public bool StartPoint => startPoint;
    public bool FinishPoint => finishPoint;

    public E_QuestState CurrentQuestState
    {
        get { return currentQuestState; }
        set { currentQuestState = value; }
    }

    public void Init()
    {
        questId = questInfoSO.id;
        dialogueKnotName = questId;
        dialogueActivator.Init();
        questEvents = GameEventsManager.Instance.questEvents;
        questEvents.onQuestStateChange += QuestStateChange;
    }

   public void Terminate()
    {
        if (questEvents == null) return;
        
        dialogueActivator.Terminate();
        questEvents.onQuestStateChange -= QuestStateChange;
   }

    private void QuestStateChange(Quest quest)
    {
        if (quest.QuestInfo.id.Equals(QuestId))
        {
            currentQuestState = quest.QuestState;
        }
    }
}
