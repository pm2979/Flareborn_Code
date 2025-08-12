using System;
using UnityEngine;

public class GameEventsManager : MonoSingleton<GameEventsManager>
{
    public MiscEvents miscEvents;
    public DialogueEvents dialogueEvents;
    public QuestEvents questEvents;
    public QuestStepEvents questStepEvents;
    public TutorialEvents tutorialEvents;

    protected override void Awake()
    {
        base.Awake();

        // 모든 이벤트들 초기화
        miscEvents = new MiscEvents();
        dialogueEvents = new DialogueEvents();
        questEvents = new QuestEvents();
        questStepEvents = new QuestStepEvents();
        tutorialEvents = new TutorialEvents();
    }
}
