using System;
using System.Collections.Generic;
using Ink.Runtime;

public class DialogueEvents
{
    public event Action<string> onEnterDialogue;
    public void EnterDialogueEvent (string knotName)
    {
        onEnterDialogue?.Invoke(knotName);
    }


    public event Action onDialogueStarted;
    public void DialogueStartedEvent()
    {
        onDialogueStarted?.Invoke();
    }

    public event Action onQuestDialogueStarted;
    public void QuestDialogueStartedEvent()
    {
        onQuestDialogueStarted?.Invoke();
    }

    public event Action onDialogueFinished;
    public void DialogueFinishedEvent()
    {
        onDialogueFinished?.Invoke();
    }

    public event Action<string, List<Choice>> onDisplayDialogue;
    public void DisplayDialogueEvent(string dialogueLine, List<Choice> dialogueChoices)
    {
        onDisplayDialogue?.Invoke(dialogueLine, dialogueChoices);
    }

    public event Action onContinueOrExitStory;
    public void ContinueOrExitStoryEvent()
    {
        onContinueOrExitStory?.Invoke();
    }

    public event Action<string> onOpenShop;
    public void OpenShopEvent(string npcName)
    {
        onOpenShop?.Invoke(npcName);

        // OpenShopUIEvent를 호출해 ShopUI를 보여줌
        OpenShopUIEvent();
    }

    public event Action onOpenShopUI;
    public void OpenShopUIEvent()
    {
        onOpenShopUI?.Invoke(); 
    }

    public event Action<string> onShowQuests;
    public void ShowQuestsEvent(string npcName)
    {
        onShowQuests?.Invoke(npcName);
    }

    public event Action onShowDialogueOptions_NormalNPC;
    public void ShowDialogueOptions_NormalNPCEvent()
    {
        onShowDialogueOptions_NormalNPC?.Invoke();
    }

    public event Action onWrappingUpDialogue;
    public void WrappingUpDialogueEvent()
    {
        onWrappingUpDialogue?.Invoke();
    }

    public event Action<int> onUpdateChoiceIndex;
    public void UpdateChoiceIndexEvent(int choiceIndex)
    {
        onUpdateChoiceIndex?.Invoke(choiceIndex);
    }

    public event Action<string, Ink.Runtime.Object> onUpdateInkDialogueVariable;
    public void UpdateInkDialogueVariableEvent(string name, Ink.Runtime.Object value)
    {
        onUpdateInkDialogueVariable?.Invoke(name, value);
    }
}
