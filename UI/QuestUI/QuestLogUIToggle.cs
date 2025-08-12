using UnityEngine;

public class QuestLogUIToggle : BaseMenuUI
{
    [SerializeField] private QuestLogUI questLogUI;

    private void Awake()
    {
        questLogUI = GetComponentInParent<QuestLogUI>();
    }

    private void OnEnable()
    {
        // 퀘스트 창을 열때는 항상 전체 퀘스트로 설정 (해당 스크립트는 PlayerQuestLogUI에서 사용됨)
        questLogUI.OnAllQuestsButton();
    }

    protected override MenuState GetMenuState()
    {
        return MenuState.Quest;
    }
}
