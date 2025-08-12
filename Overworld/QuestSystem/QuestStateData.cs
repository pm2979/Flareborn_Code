using UnityEngine;

[System.Serializable]
public class QuestStateData
{
    public E_QuestState state;
    public int questStepIndex;
    public QuestStepState[] questStepStates;

    public QuestStateData(E_QuestState state, int questStepIndex, QuestStepState[] questStepStates)
    {
        this.state = state;
        this.questStepIndex = questStepIndex;
        this.questStepStates = questStepStates;
    }
}

