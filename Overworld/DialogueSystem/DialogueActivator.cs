using UnityEngine;
using Ink.Runtime;
public enum E_DialogueState
{
    InDialogue,
    OutOfDialogue,
}

[System.Serializable]
public class DialogueActivator
{
    private DialogueEvents dialogueEvents;
    private QuestEvents questEvents;

    [Header("Ink Story")]
    [SerializeField] private string dialogueKnotName;
    [SerializeField] public TextAsset dialogueJson;

    private Story story;
    private int currentChoiceIndex = -1;

    private InkExternalFunctions inkExternalFunctions;
    private InkDialogueVariables inkDialogueVariables;

    private bool isDialoguePlaying = false;

    private E_DialogueState currentDialogueState = E_DialogueState.OutOfDialogue;

    // 프로퍼티
    public E_DialogueState CurrentDialogueState => currentDialogueState;
    public string DialogueKnotName
    {
        get => dialogueKnotName;
        set => dialogueKnotName = value;
    }

    public void Init()
    {
        story = new Story(dialogueJson.text);
        inkExternalFunctions = new InkExternalFunctions();
        inkExternalFunctions.Bind(story);
        inkDialogueVariables = new InkDialogueVariables(story);

        SubscribeToEvents();
    }
    
    public void Terminate()
    {
        // # 방어로직
        if (inkExternalFunctions == null) return;
        inkExternalFunctions.Unbind(story);

        UnSubscribeToEvents();
    }

    public void SubscribeToEvents()
    {
        dialogueEvents = GameEventsManager.Instance.dialogueEvents;
        questEvents = GameEventsManager.Instance.questEvents;

        dialogueEvents.onEnterDialogue += EnterDialogue;
        dialogueEvents.onUpdateChoiceIndex += UpdateChoiceIndex;
        dialogueEvents.onUpdateInkDialogueVariable += UpdateInkDialogueVariable;
        dialogueEvents.onContinueOrExitStory += continueOrExitStory;
        dialogueEvents.onDialogueFinished += DialogueFinished;
        questEvents.onQuestStateChange += QuestStateChange;
        questEvents.onCancelQuest += CancelQuest;
    }

    public void UnSubscribeToEvents()
    {
        // #방어로직: GameEventsManager가 null인 경우를 방지
        if (GameEventsManager.Instance == null) return;

        dialogueEvents.onEnterDialogue -= EnterDialogue;
        dialogueEvents.onUpdateChoiceIndex -= UpdateChoiceIndex;
        dialogueEvents.onUpdateInkDialogueVariable -= UpdateInkDialogueVariable;
        dialogueEvents.onContinueOrExitStory -= continueOrExitStory;
        questEvents.onQuestStateChange -= QuestStateChange;
        questEvents.onCancelQuest -= CancelQuest;
    }

    public void CancelQuest(Quest quest)
    {
        // 스토리 상태 리셋
        story.ResetState();
    }

    private void QuestStateChange(Quest quest)
    {
        dialogueEvents.UpdateInkDialogueVariableEvent(
            quest.QuestInfo.id + "State",
            new StringValue(quest.QuestState.ToString()));
    }

    private void DialogueFinished()
    {
        // 대화가 종료되면 대화 상태를 OutOfDialogue로 변경
        ChangeDialogueState(E_DialogueState.OutOfDialogue);
        isDialoguePlaying = false;
    }

    private void UpdateInkDialogueVariable(string name, Ink.Runtime.Object value)
    {
        // 변수 동기화 및 업데이트
        inkDialogueVariables.UpdateVariableState(name, value);
    }

    private void UpdateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }

    public void EnterDialogue(string knotName)
    {
        // 대화가 이미 진행 중이면 중복 실행 방지
        if (isDialoguePlaying) return;

        isDialoguePlaying = true;

        // 대화시작 이벤트 발생
        dialogueEvents.DialogueStartedEvent();

        // knot 이름이 비어있지 않으면 대화 시작
        if (!knotName.Equals(string.Empty))
        {
            // 대화 시작
            story.ChoosePathString(knotName);

            // 대화 상태 변경
            ChangeDialogueState(E_DialogueState.InDialogue);
        }

        // 대화 시작 후 변수 동기화 및 리스닝 시작
        inkDialogueVariables.SyncVariablesAndStartListening(story);

        continueOrExitStory();
    }

    public void continueOrExitStory()
    {
        // 선택지가 존재 하고, 그 선택지중 하나가 현재 선택된 상태라면
        if (story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            // 현재 선택된 선택지로 대화 진행
            story.ChooseChoiceIndex(currentChoiceIndex);

            // 선택 후 선택지 인덱스 초기화
            currentChoiceIndex = -1; 
        }

        if (story.canContinue)
        {
            string dialogueLine = story.Continue();

            // 다이얼로그 라인이 비어있을 시 스킵
            while (IsLineBlank(dialogueLine) && story.canContinue)
                dialogueLine = story.Continue();

            // 마지막 줄의 다이얼로그가 비어있는 상태라면 대화 종료
            if (IsLineBlank(dialogueLine) && !story.canContinue)
            {
                ExitDialogue();
                return;
            }
            else
            {
                // 대화를 진행하며 대화내용을 보여주는 이벤트 발생
                dialogueEvents.DisplayDialogueEvent(dialogueLine, story.currentChoices);
            }

        }
        // 더 이상 선택해야할 선택지가 없다면 대화 종료
        else if (story.currentChoices.Count == 0)
        {
            ExitDialogue();
        }
    }

    private void ExitDialogue()
    {
        Debug.Log("Exiting Dialogue");

        // 대화 종료 이벤트 발생
        dialogueEvents.DialogueFinishedEvent();

        // 변수 동기화 및 리스닝 중지
        inkDialogueVariables.StopListening(story);

        // 스토리 상태 리셋
        story.ResetState();
    }

    public void ChangeDialogueState(E_DialogueState state)
    {
        currentDialogueState = state;
    }

    private bool IsLineBlank(string dialogueLine)
    {
        return dialogueLine.Trim().Equals(string.Empty) || dialogueLine.Trim().Equals("\n");
    }
}
