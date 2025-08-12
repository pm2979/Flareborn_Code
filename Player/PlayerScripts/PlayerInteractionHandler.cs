using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum E_InteractableType
{
    Default,
    NPC,
    Item,
}

public class PlayerInteractionHandler : MonoBehaviour
{
    private IInteractable currentInteractable;
    private E_InteractableType currentInteractableType = E_InteractableType.Default;

    // 프로퍼티
    public IInteractable CurrentInteractable => currentInteractable;

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && currentInteractable != null)
        {
             currentInteractable.Interact();

            if (currentInteractableType == E_InteractableType.Item)
            {
                // InteractableType이 Item일경우 한 번 인터랙트 후 비워준다
                currentInteractable = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            currentInteractable = interactable;
            interactable.ShowPrompt(true);

            // 현재 인터랙터블 타입 설정
            SetInteractableType(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            if (currentInteractable == interactable)
            {
                currentInteractable.ShowPrompt(false);
                currentInteractable = null;
            }
        }
    }

    private void SetInteractableType(IInteractable interactable)
    {
        if (interactable is Interactable_NPC)
        {
            currentInteractableType = E_InteractableType.NPC;
        }
        else if (interactable is Interactable_Item)
        {
            currentInteractableType = E_InteractableType.Item;
        }
        else
        {
            currentInteractableType = E_InteractableType.Default;
        }
    }

    public void EmptyCurrentInteractable()
    {
        // 현재 인터랙터블을 비워준다
        currentInteractable = null;
        currentInteractableType = E_InteractableType.Default;
    }
}