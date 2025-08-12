using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    [SerializeField] private AttackHitboxController hitboxController;

    public void ActivateHitbox()
    {
        hitboxController.EnableHitbox();
    }

    public void DeactivateHitbox()
    {
        hitboxController.DisableHitbox();
    }
}
