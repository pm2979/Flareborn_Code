using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tutorialText;

    public void SetTutorialText(string text)
    {
        if (tutorialText != null)
            tutorialText.text = text;
    }

    public void ClearTutorialText()
    {
        if (tutorialText != null)
            tutorialText.text = string.Empty;
    }

    public void HideTutorialText()
    {
        tutorialText.gameObject.SetActive(false);
    }

}
