using UnityEngine;

public class Portal : MonoBehaviour
{
    public string targetScene;
    public string portalID;
    public string targetPortalID;
    public float cooldownTime = 1.5f;

    [Header("스폰 위치 및 방향")]
    public Transform spawnPoint;

    protected bool isOnCooldown = false;

    public virtual void Enter()
    {
        
    }

    public void ResetCooldown()
    {
        isOnCooldown = false;
    }
}
