using UnityEngine;

[System.Serializable]
public class DialogueData
{
    [Header("Default Dialogue")]
    [SerializeField] private string dialogueKnotName;
    [SerializeField] private DialogueActivator dialogueActivator;

    // 프로퍼티
    public string DialogueKnotName {  get => dialogueKnotName; set => dialogueKnotName = value; }
    public DialogueActivator DialogueActivator => dialogueActivator;

    public void Init()
    {
        dialogueActivator.Init();
        dialogueKnotName = dialogueActivator.dialogueJson.name;
    }

    public void Terminate()
    {
        dialogueActivator.Terminate();
    }
}
