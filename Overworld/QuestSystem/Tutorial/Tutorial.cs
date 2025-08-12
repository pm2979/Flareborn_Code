using UnityEngine;

// dynamic data that's loaded from a ScriptableObject (static data)
public class Tutorial : MonoBehaviour
{
    [SerializeField] private TutorialInfoSO tutorialInfoSO; 
    [SerializeField] private int tutorialId;
    [SerializeField] private string tutorialName;
    [SerializeField] private bool isFinished;
    protected TutorialUI tutorialUI;

    // 프로퍼티
    public int TutorialId => tutorialId;

    protected virtual void Start()
    {
        tutorialUI = FindFirstObjectByType<TutorialUI>();
    }

    private void OnValidate()
    {
        tutorialId = tutorialInfoSO.tutorialId;
        tutorialName = tutorialInfoSO.tutorialName;
    }

    public virtual void FinishTutorial()
    {
        isFinished = true;
        tutorialUI.SetTutorialText(ClearTutorialText());
    }

    private string ClearTutorialText()
    {
        return string.Empty;
    }
}
