using UnityEngine;
using System.Collections;

public class Portal_General : Portal
{ 
    private void OnTriggerEnter(Collider other)
    {
        if (isOnCooldown) return;

        if (other.CompareTag("Player"))
        {
            isOnCooldown = true;
            
            string sourcePortalID = portalID;

            SceneLoader.Instance.LoadSceneWithPortal(targetScene, targetPortalID, () =>
            {
                SceneLoader.Instance.StartPortalCooldown(sourcePortalID, cooldownTime);
            });
        }
    }
   
}