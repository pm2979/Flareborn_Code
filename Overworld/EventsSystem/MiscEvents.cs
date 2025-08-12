using System;

public class MiscEvents
{
    public event Action onMenuToggle;
    public void MenuToggleEvent()
    {
        onMenuToggle?.Invoke();
    }

    public event Action onQuestLogToggle;
    public void QuestLogToggleEvent()
    {
        onQuestLogToggle?.Invoke();
    }

    public event Action onEnablePlayerMovement;
    public void EnablePlayerMovementEvent()
    {
        onEnablePlayerMovement?.Invoke();
    }

    public event Action onDisablePlayerMovement;
    public void DisablePlayerMovementEvent()
    {
        onDisablePlayerMovement?.Invoke();
    }

    public event Action onPlayerInputDisable;
    public void PlayerInputDisableEvent()
    {
        onPlayerInputDisable?.Invoke();
    }

    public event Action onPlayerInputEnable;
    public void PlayerInputEnableEvent()
    {
        onPlayerInputEnable?.Invoke();
    }

    public event Action onEnableRunePanel;
    public void EnableRunePanelEvent()
    {
        onEnableRunePanel?.Invoke();
    }
}
