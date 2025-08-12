using UnityEngine;

public abstract class BaseMenuUI : MonoBehaviour
{
    protected abstract MenuState GetMenuState();

    public void SetActive(MenuState state)
    {
        this.gameObject.SetActive(GetMenuState() == state);
    }
}
