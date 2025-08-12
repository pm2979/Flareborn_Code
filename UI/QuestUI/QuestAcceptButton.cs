using UnityEngine;

public class QuestAcceptButton : QuestButton
{
    public override void onClickButton()
    {
        base.onClickButton();

        // 현재 대화중인 NPC와의 대화를 종료
        dialogueEvents.DialogueFinishedEvent();

        // AcceptQuestEvent 이벤트 발생
        questEvents.AcceptQuestEvent(quest);

        // 퀘스트 수락으로 인한 퀘스트 상태 변화 이벤트 발생 (Can_Start => In_Progress)
        questEvents.QuestStateChangeEvent(quest);

        // 퀘스트 다이얼로그를 바로 시작
        dialogueEvents.QuestDialogueStartedEvent();
    }
}
