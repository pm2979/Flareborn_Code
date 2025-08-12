using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [field:SerializeField] public BattleUI BattleUI { get; private set; }
    [field:SerializeField] public OverworldUI OverworldUI { get; private set; }

    private UIState currentState;

    protected override void Awake()
    {
        base.Awake();

        OverworldUI.Init();
        ChangeState(UIState.Overworld);
    }

    public void SetOverworld()
    {
        ChangeState(UIState.Overworld);
    }

    public void SetBattle()
    {
        ChangeState(UIState.Battle);
    }

    public void ChangeState(UIState state)
    {
        currentState = state;

        if(BattleUI != null)
        BattleUI.SetActive(currentState);

        if(OverworldUI != null)
        OverworldUI.SetActive(currentState);
    }

    // 현재 상태
    public UIState CurrentState => currentState;
}
