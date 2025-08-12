using UnityEngine;

public class DialogueDataHandler : MonoBehaviour
{
    [Header("Default Dialogue")]
    [SerializeField] private DialogueData defaultDialogue;

    // 프로퍼티
    public DialogueData DefaultDialogue => defaultDialogue;

    private void Start()
    {
        defaultDialogue.Init();
    }

    private void OnDisable()
    {
        defaultDialogue.Terminate();
    }

    private void OnValidate()
    {
        defaultDialogue.DialogueKnotName = defaultDialogue.DialogueActivator.dialogueJson.name;
        defaultDialogue.DialogueActivator.DialogueKnotName = defaultDialogue.DialogueActivator.dialogueJson.name;
    }
}
