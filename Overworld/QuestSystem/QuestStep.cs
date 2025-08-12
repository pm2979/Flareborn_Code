using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;
    private int stepIndex;

    // 프로퍼티
    public bool IsFinished => isFinished;
    public string QuestId => questId;
    public int StepIndex => stepIndex;


    public void InitializeQuestStep(string questId, int stepIndex, string questStepState)
    {
        this.questId = questId;
        this.stepIndex = stepIndex;

        if (questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        } 
    }

    public void ResetQuestStep (string questId, int stepIndex, string questStepState)
    {
        this.questId = questId;
        this.stepIndex = stepIndex;
        isFinished = false;
        if (questStepState != null && questStepState != "")
        {
            SetQuestStepState(questStepState);
        }
    }

    protected virtual void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;

            GameEventsManager.Instance.questEvents.AdvanceQuestEvent(questId);

            Destroy(this.gameObject);
        }
    }

    protected void ChangeState(string newState, string newStatus)
    {
        QuestStepState questStepState = new QuestStepState(newState, newStatus);
        GameEventsManager.Instance.questEvents.QuestStepStateChange(questId, stepIndex, questStepState);
    }

    protected abstract void UpdateState();

    protected abstract void SetQuestStepState(string state);
}
