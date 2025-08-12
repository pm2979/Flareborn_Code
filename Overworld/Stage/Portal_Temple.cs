using UnityEngine;
using System.Collections;

public class Portal_Temple: Portal, IInteractable
{
    [Header("포탈 VFX / 인터랙트 아이콘")]
    public ParticleSystem portalVFX;
    public GameObject pressE;
    private bool isVFXPlaying = false;

    private void OnEnable()
    {
        // 시작시 포탈 VFX 재생 X
        portalVFX.Stop();
    }

    public void Interact()
    {
        if (isOnCooldown) return;

        UsePortal();
    }

    public void ShowPrompt(bool show)
    {
        // # 방어로직 
        if (portalVFX == null) return;
        if (pressE == null) return;

        // 포탈 VFX 활성화/비활성화 
        if (show && !isVFXPlaying)
        {
            isVFXPlaying = true;
            portalVFX.Play();
        }
        else if (!show && isVFXPlaying)
        {
            isVFXPlaying = false;
            portalVFX.Stop();
        }
        portalVFX.gameObject.SetActive(show);
        pressE.SetActive(show);
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