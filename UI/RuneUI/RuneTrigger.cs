using UnityEngine;

public class RuneStatue : Interactable_NPC
{
    [SerializeField] GameObject pressE;
    [SerializeField] ParticleSystem runeCraftVFX;
    private MiscEvents miscEvents;
    private bool isVFXPlaying = false;


    void OnEnable()
    {
        miscEvents = GameEventsManager.Instance.miscEvents;

        // 시작시 VFX 재생 X
        runeCraftVFX.Stop();
    }


    public override void Interact()
    {
        miscEvents.EnableRunePanelEvent();
    }

    public override void ShowPrompt(bool show)
    {
        // # 방어로직
        if (pressE == null) return;
        if (runeCraftVFX == null) return;

        // VFX 활성화/비활성화
        if (show && !isVFXPlaying)
        {
            isVFXPlaying = true;
            runeCraftVFX.Play();
        }
        else if (!show && isVFXPlaying)
        {
            isVFXPlaying = false;
            runeCraftVFX.Stop();
        }

        pressE.SetActive(show);
        runeCraftVFX.gameObject.SetActive(show);
    }
}
