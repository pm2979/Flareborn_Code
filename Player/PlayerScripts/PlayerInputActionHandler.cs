using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputActionHandler : MonoBehaviour
{
    // 메뉴 토글 버튼 메서드
    public void OnMenuToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameEventsManager.Instance.miscEvents.MenuToggleEvent();
        }
    }
}
