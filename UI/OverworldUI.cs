using UnityEngine;

public class OverworldUI : BaseUI
{
    [SerializeField]private MenuUIController menuUIController;

    public void Start()
    {
        GameEventsManager.Instance.miscEvents.onMenuToggle += ToggleMenu;
    }

    public void OnDestroy()
    {
        // #방어로직
        if (GameEventsManager.Instance == null) return;

        GameEventsManager.Instance.miscEvents.onMenuToggle -= ToggleMenu;
    }

    public void Init()
    {
        menuUIController.Init();
    }

    protected override UIState GetUIState()
    {
        return UIState.Overworld;
    }

    public void ToggleMenu()
    {
        if (menuUIController.gameObject.activeSelf)
        {
            menuUIController.gameObject.SetActive(false);
        }
        else
        {
            menuUIController.gameObject.SetActive(true);
            menuUIController.ChangeState(MenuState.Main);
        }
    }
}