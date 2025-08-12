using UnityEngine;

[CreateAssetMenu(fileName = "TutorialInfoSO", menuName = "ScriptableObjects/TutorialInfo", order = 1)]
public class TutorialInfoSO : ScriptableObject
{
    [Header("Tutorial Information")]
    public int tutorialId;
    public string tutorialName;
}
