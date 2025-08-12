using UnityEngine;

public class QuestCancelButton : QuestButton
{
    private QuestLogUI_PlayerQuestLogUI questLogUI;

    protected override void OnEnable()
    {
        base.OnEnable();
        // questLogUI가 null인 경우에만 할당을 한다.
        if (questLogUI == null)
            questLogUI = GetComponentInParent<QuestLogUI_PlayerQuestLogUI>();   
    }

    public override void onClickButton()
    {
        base.onClickButton();

        // CancelQuestEvent 이벤트 발생
        questEvents.CancelQuestEvent(quest);

        // 현재 퀘스트로 설정되어있는 퀘스트를 제거
        questEvents.UnsetCurrentQuestEvent(quest.QuestInfo.id);

        // 현재 대화중인 NPC와의 대화를 종료
        dialogueEvents.DialogueFinishedEvent();

        // 퀘스트 상태 변화 이벤트 발생
        questEvents.UpdateQuestDataBaseEvent();

        // 퀘스트 취소 후 현재 퀘스트 로그 UI로 퀘스트 UI상태 갱신
        questLogUI.OnAllQuestsButton();

        // 퀘스트 취소 후 취소 버튼을 비활성화
        HideButton();
    }
}
