using UnityEngine;
using System.Collections;

public class Portal_OnPress : Portal, IInteractable
{
    [Header("인터랙트 아이콘")]
    public GameObject pressE;
    private bool isVFXPlaying = false;

    private void Start()
    {
        pressE.SetActive(false);
    }

    public void Interact()
    {
        if (isOnCooldown) return;

        UsePortal();
    }

    public void ShowPrompt(bool show)
    {
        // # 방어로직 
        if (pressE == null) return;

        if (show) pressE.SetActive(show);
        else if (!show) pressE.SetActive(false);
    }
    private void UsePortal()
    {
        // 포탈 쿨다운 플래그 활성화
        isOnCooldown = true;

        string sourcePortalID = portalID;

        SceneLoader.Instance.LoadSceneWithPortal(targetScene, targetPortalID, () =>
        {
            SceneLoader.Instance.StartPortalCooldown(sourcePortalID, cooldownTime);
        });
    }
}