using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Collections.Generic;


public class DialoguePanelUI : MonoBehaviour
{
    private DialogueEvents dialogueEvents;

    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private GameObject darkeningScreen;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceButton[] dialogueChoiceButtons;
    [SerializeField] private GameObject dialogueOptions_ShopNPC;
    [SerializeField] private GameObject dialogueOptions_NormalNPC;


    private void Awake()
    {
        contentParent.SetActive(false);
        ResetPanel();
    }

    private void OnEnable()
    {
        dialogueEvents = GameEventsManager.Instance.dialogueEvents;

        dialogueEvents.onDialogueStarted += DialogueStarted;
        dialogueEvents.onDialogueFinished += DialgoueFinished;
        dialogueEvents.onDisplayDialogue += DisplayDialogue;
    }
    private void OnDisable()
    {
        // #방어로직
        if (GameEventsManager.Instance == null) return;

        dialogueEvents.onDialogueStarted -= DialogueStarted;
        dialogueEvents.onDialogueFinished -= DialgoueFinished;
        dialogueEvents.onDisplayDialogue -= DisplayDialogue;
    }

    private void DialogueStarted()
    {
        contentParent.SetActive(true);
    }

    private void DialgoueFinished()
    {
        contentParent.SetActive(false);

        // 다이얼로그가 끝나면 패널을 초기화
        ResetPanel();
    }

    private void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices)
    {
        dialogueText.text = dialogueLine;

        // 다이얼로그 선택지 버튼을 업데이트
        if (dialogueChoices.Count > dialogueChoiceButtons.Length)
        {
            Debug.LogWarning("Not enough dialogue choice buttons available. Some choices will not be displayed.");
        }

        // 모든 다이얼로그 선택지 버튼을 비활성화
        foreach (DialogueChoiceButton choicebutton in dialogueChoiceButtons)
        {
            choicebutton.gameObject.SetActive(false);
        }

        int choiceButtonIndex = dialogueChoices.Count - 1;
        for (int inkChoiceIndex = 0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
         {
            Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
            DialogueChoiceButton choiceButton = dialogueChoiceButtons[choiceButtonIndex];

            choiceButton.gameObject.SetActive(true);
            choiceButton.SetChoiceText(dialogueChoice.text);
            choiceButton.SetChoiceIndex(inkChoiceIndex);

            if (inkChoiceIndex == 0)
            {
                // 첫 번째 선택지는 자동으로 선택
                choiceButton.SelectButton();
                GameEventsManager.Instance.dialogueEvents.UpdateChoiceIndexEvent(0);
            }

            choiceButtonIndex--;
        }
    }

    private void ResetPanel()
    {
        dialogueText.text = string.Empty;
    }
}
