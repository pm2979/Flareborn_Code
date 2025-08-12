using System;
using UnityEngine;

public class TutorialEvents : MonoBehaviour
{
    public Action onTutorialStepClear;
    public void TutorialStepClearEvent()
    {
        onTutorialStepClear?.Invoke();
    }
}
