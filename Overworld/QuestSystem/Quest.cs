using UnityEngine;

public class Quest
{
    // Static Info
    private QuestInfoSO questInfo;
    private string dialogueKnotName;

    // QuestState Info
    private E_QuestState questState;
    private E_NPCList questNPC; // 퀘스트를 제공하는 NPC
    private int currentQuestStepIndex;
    private QuestStepState[] questStepStates;

    // 프로퍼티
    public QuestInfoSO QuestInfo => questInfo;
    public string DialogueKnotName => dialogueKnotName;
    public E_NPCList QuestNPC => questNPC;
    public QuestStepState[] QuestStepStates => questStepStates;
    public int CurrentQuestStepIndex { get { return currentQuestStepIndex; } set { currentQuestStepIndex = value; } }
    public E_QuestState QuestState {get { return questState; } set { questState = value; } }



    public Quest(QuestInfoSO questInfo)
    {
        this.questInfo = questInfo;
        this.dialogueKnotName = questInfo.id;
        this.questState = E_QuestState.REQUIREMENTS_NOT_MET;
        this.questNPC = questInfo.questNPC;
        this.currentQuestStepIndex = 0;
        this.questStepStates = new QuestStepState[questInfo.questStepPrefabs.Length];

        for (int i = 0; i < questStepStates.Length; i++)
        {
            questStepStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestInfoSO questInfo, E_QuestState state, int currentQuestStepIndex, QuestStepState[] questStepStates)
    {
        this.questInfo = questInfo;
        this.questState = state;
        this.currentQuestStepIndex = currentQuestStepIndex;
        this.questStepStates = questStepStates;

        if (this.questStepStates.Length != questInfo.questStepPrefabs.Length)
        {
            Debug.LogError("Quest step states length does not match quest step prefabs length.");
        }
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < questInfo.questStepPrefabs.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();

        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform).
                                  GetComponent<QuestStep>();
            questStep.InitializeQuestStep(questInfo.id, currentQuestStepIndex, questStepStates[currentQuestStepIndex].state);
        }   
    }

    public void ResetQuest()
    {
        // 퀘스트 상태를 초기화
        questState = E_QuestState.CAN_START;
        currentQuestStepIndex = 0;

        // 퀘스트 스텝 상태를 초기화
        for (int i = 0; i < questStepStates.Length; i++)
        {
            questStepStates[i] = new QuestStepState();
        }

        // 퀘스트 스텝 게임 오브젝트들을 초기화
        int questStepIndex = 0;
        foreach (GameObject questStepGO in QuestInfo.questStepPrefabs)
        {
            QuestStep questStep = questStepGO.GetComponent<QuestStep>();

            questStep.ResetQuestStep(questInfo.id, questStepIndex, questStepStates[questStepIndex].state);

            questStepIndex++;
        } 
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentStepExists())
        {
            questStepPrefab = questInfo.questStepPrefabs[currentQuestStepIndex];
            Debug.Log($"Current quest step prefab: {questStepPrefab.name} at index {currentQuestStepIndex}");
        }
        else
        {
            Debug.LogError("No current quest step exists for this quest.");
        }

        return questStepPrefab;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
    {
        if (stepIndex < questStepStates.Length)
        {
            questStepStates[stepIndex].state = questStepState.state;
            questStepStates[stepIndex].status = questStepState.status;
        }
        else
        {
            Debug.LogWarning($"Attempted to store questState for step index {stepIndex}, but it exceeds the length of questStepStates array.");
        }
    }

    public QuestStateData GetQuestStateData()
    {
        return new QuestStateData(questState, currentQuestStepIndex, questStepStates);
    }

    public string GetQuestGiverName(E_NPCList questGiver)
    {
        // 퀘스트를 제공하는 NPC의 이름을 반환
        string formattedName = "";

        switch (questGiver)
        {
            case E_NPCList.NPC_Mother:
                formattedName = "어머니";
                break;

            case E_NPCList.NPC_Dian:
                formattedName = "디안";
                break;

            case E_NPCList.NPC_Nepin:
                formattedName = "네핀";
                break;
        }

        return formattedName;
    }

    public string GetFullStatusText()
    {
        string fullStatus = "";

        if (questState == E_QuestState.REQUIREMENTS_NOT_MET)
        {
            fullStatus = "<color=red>퀘스트를 시작하기위한 조건을 만족하지 못했습니다.</color>";
        }
        else if (questState == E_QuestState.CAN_START)
        {
            fullStatus = "퀘스트 시작 가능";
        }
        else
        {
            // 지금까지 완료한 퀘스트 스텝들을 스트라이크 처리하여 표시
            for (int i = 0; i < currentQuestStepIndex; i++)
            {
                fullStatus += "<s>" + questStepStates[i].status + "</s>\n";
            }

            // 만약 존재 한다면, 현재 퀘스트 스텝의 상태를 추가
            if (CurrentStepExists())
            {
                fullStatus += questStepStates[currentQuestStepIndex].status;
            }

            // 퀘스트가 완료되거나 완료 가능한 경우
            if (questState == E_QuestState.CAN_FINISH)
            {
                fullStatus += "\n퀘스트를 완료 가능";
            }
            else if (questState == E_QuestState.FINISHED)
            {
                fullStatus += "\n<color=green>퀘스트를 완료하였습니다.</color>";
            }

        }

        return fullStatus;
    }
}
