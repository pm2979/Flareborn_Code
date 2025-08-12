using UnityEngine;

public class QuestDataHolder : MonoBehaviour
{
    [Header("Quest Datas")]
    [SerializeField] private QuestData[] questDatas;

    // 프로퍼티
    public QuestData[] QuestDatas { get { return questDatas; } set { questDatas = value; } }

    private void Start()
    {
        // questDatas 안의 questData들을 Init해준다. 
        foreach (QuestData questData in questDatas)
        {
            if (questData != null)
            {
                questData.Init();
            }
        }
    }

    private void OnDisable()
    {
        foreach (QuestData questData in questDatas)
        {
            // 퀘스트 포인트 Termination
            questData.Terminate();
        }
    }

    private void OnValidate()
    {
        foreach (QuestData questData in questDatas)
        {
            if (questData != null)
            {
                // 각 questData의 DialogueKnotName은 QuestId와 동일하게 설정한다.
                questData.DialogueKnotName = questData.QuestId;
                questData.DialogueActivator.DialogueKnotName = questData.DialogueActivator.dialogueJson.name;
            }
        }
    
    }
}
