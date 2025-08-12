using System.Diagnostics;
using UnityEngine;

public class MainUI : BaseMenuUI
{
    MenuUIController controller;

    public void Init(MenuUIController menuUIController)
    {
        controller = menuUIController;
    }

    public void OnClickInventory() // 인벤토리 버튼
    {
        controller.ChangeState(MenuState.Inventory);
    }

    public void OnClickEquipment() // 장비 버튼
    {
        controller.ChangeState(MenuState.Equipment);
    }

    public void OnClickState() // 상태창 버튼
    {
        controller.ChangeState(MenuState.Status);
    }

    public void OnClickAbility() // 어빌리티 버튼
    {
        controller.ChangeState(MenuState.Rune);
    }

    public void OnClickQuest() // 퀘스트 버튼
    {
        // 먼저 퀘스트 상태들을 모두 업데이트 한다
        GameManager.Instance.QuestManager.UpdateAllQuestStates();

        // 업데이트된 퀘스트 상태들을 토대로 QuestDataBase를 업데이트한다
        GameEventsManager.Instance.questEvents.UpdateQuestDataBaseEvent();
        controller.ChangeState(MenuState.Quest);
    }

    public void OnClickOption() // 옵션 버튼
    {
        controller.ChangeState(MenuState.Option);
    }

    protected override MenuState GetMenuState()
    {
        return MenuState.Main;
    }
}
