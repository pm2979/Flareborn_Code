using UnityEngine;

public class AttackHitboxController : MonoBehaviour
{
    [SerializeField] private GameObject hitboxObject;
    [SerializeField] private Transform frontPos, backPos, leftPos, rightPos;

    private Animator animator;

    private void Awake()
    {
        if (hitboxObject != null)
            hitboxObject.SetActive(false);
        animator = GetComponent<Animator>();
    }

    public void EnableHitbox()
    {
        UpdateHitboxPositionFromAnimation();
        hitboxObject.SetActive(true);
    }

    public void DisableHitbox()
    {
        hitboxObject.SetActive(false);
    }

    private void UpdateHitboxPositionFromAnimation()
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator not assigned.");
            return;
        }

        string currentClipName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        Transform targetPos = frontPos; // Default fallback

        if (currentClipName.Contains("Front"))
            targetPos = frontPos;
        else if (currentClipName.Contains("Back"))
            targetPos = backPos;
        else if (currentClipName.Contains("Left"))
            targetPos = leftPos;
        else if (currentClipName.Contains("Right"))
            targetPos = rightPos;
        else
            Debug.LogWarning($"Unknown direction in animation name: {currentClipName}");

        hitboxObject.transform.position = targetPos.position;
        hitboxObject.transform.rotation = targetPos.rotation;
    }
}
