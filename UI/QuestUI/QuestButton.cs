using System.Collections;
using UnityEngine;

public class QuestButton : MonoBehaviour
{
    protected QuestEvents questEvents;
    protected DialogueEvents dialogueEvents;
  
    protected Quest quest;
    protected bool isInitialized = false;

    // 프로퍼티 
    public Quest Quest => quest;

    protected virtual void OnEnable()
    {
        // ShowButton메서드가 호출되면 OnEnable이 호출된다.

        if (isInitialized) return;

        questEvents = GameEventsManager.Instance.questEvents;
        dialogueEvents = GameEventsManager.Instance.dialogueEvents;

        // Initialize 완료
        isInitialized = true;
    }

    protected void OnDisable()
    {
        // #방어로직
        if (GameEventsManager.Instance == null) return;

        // DeInitialize 완료
        isInitialized = false;
    }

    public void SetQuest(Quest quest)
    {
        this.quest = quest;
    }

    public void ClearQuest()
    { 
        this.quest = null;
    }

    public void ShowButton()
    {
        this.gameObject.SetActive(true);
    }

    public void HideButton()
    {
        this.gameObject.SetActive(false);
    }

    // 각 버튼 override용
    public virtual void onClickButton() { }
}
