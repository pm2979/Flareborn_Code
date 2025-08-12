using UnityEngine;
using UnityEngine.Playables;

public class CutScene_SkipToDialogue : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public float skippableThreshold;
    public float skipTime;

    // Update is called once per frame
    void Update()
    {
        if (playableDirector.time < skippableThreshold)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SkipToPart();
            }
        }
    }

    public void SkipToPart()
    {
        if (playableDirector == null) return;

        playableDirector.time = skipTime;
        playableDirector.Evaluate();
    }
}
