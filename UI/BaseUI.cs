using UnityEngine;

public abstract class BaseUI : MonoBehaviour
{
    protected abstract UIState GetUIState();

    public void SetActive(UIState state)
    {
        this.gameObject.SetActive(GetUIState() == state);
    }
}
