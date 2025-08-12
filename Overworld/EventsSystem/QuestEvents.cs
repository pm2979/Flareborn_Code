using System;
using System.Diagnostics;

public class QuestEvents
{
    // 퀘스트 데이터베이스가 퀘스트 상태별로 업데이트되었을 때 발생하는 이벤트
    public event Action onUpdateQuestDataBase;
    public void UpdateQuestDataBaseEvent()
    {
        onUpdateQuestDataBase?.Invoke();
    }

    // 퀘스트를 수락했을 때 발생하는 이벤트
    public event Action<Quest> onAcceptQuest;
    public void AcceptQuestEvent(Quest quest)
    {
        onAcceptQuest?.Invoke(quest);
    }

    // 퀘스트를 거절했을 때 발생하는 이벤트
    public event Action<Quest> onCancelQuest;
    public void CancelQuestEvent(Quest quest)
    {
        onCancelQuest?.Invoke(quest);
    }

    // 퀘스트를 시작했을 때 발생하는 이벤트
    public event Action<string> onStartQuest;
    public void StartQuestEvent(string id)
    {
        onStartQuest?.Invoke(id);
    }

    // 퀘스트의 단계를 완료했을 때 발생하는 이벤트
    public event Action<string> onAdvanceQuest;
    public void AdvanceQuestEvent(string id)
    {
        onAdvanceQuest?.Invoke(id);
    }

    // 퀘스트를 완료했을 때 발생하는 이벤트
    public event Action<string> onCompleteQuest;
    public void CompleteQuestEvent(string id)
    {
        onCompleteQuest?.Invoke(id);
    }

    // 퀘스트의 상태가 변경되었을 때 발생하는 이벤트
    public event Action<Quest> onQuestStateChange;
    public void QuestStateChangeEvent(Quest quest)
    {
        onQuestStateChange?.Invoke(quest);
    }

    // 퀘스트를 끝냈을 때 발생하는 이벤트
    public event Action<string> onUnsetCurrentQuest;
    public void UnsetCurrentQuestEvent(string id)
    {
        onUnsetCurrentQuest?.Invoke(id);
    }

    public event Action<string, int, QuestStepState> onQuestStepStateChange;
    public void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        onQuestStepStateChange?.Invoke(id, stepIndex, questStepState);
    }
}
