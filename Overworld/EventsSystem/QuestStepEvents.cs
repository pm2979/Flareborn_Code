using System;
using NUnit.Framework.Interfaces;

public class QuestStepEvents
{
    // CollectLogsQuest Step Event
    public event Action onLogCollected;
    public void LogCollectedEvent()
    {
        onLogCollected?.Invoke();
    }
}
