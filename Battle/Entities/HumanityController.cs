using System;

public class HumanityController
{
    public int CurrentHumanity { get; private set; }
    public int MaxHumanity { get; private set; }

    public bool IsTranscendent { get; private set; } // 초월 상태 확인
    public Action OnHumanityDepleted;

    public HumanityController(int humanity, bool isTranscendent = false)
    {
        CurrentHumanity = humanity;
        MaxHumanity = 100;
        IsTranscendent = isTranscendent;
    }

    public void RecoverHumanity(int amount) // 인간성 회복
    {
        CurrentHumanity = Math.Min(CurrentHumanity + amount, MaxHumanity);
    }

    public void ReduceHumanity(int amount) // 인간성 감소
    {
        CurrentHumanity = Math.Max(0, CurrentHumanity - amount);

        if (CurrentHumanity == 0 && !IsTranscendent)
        {
            OnHumanityDepleted?.Invoke();
        }
    }

    public void SetTranscendent(bool isTranscendent)
    {
        IsTranscendent = isTranscendent;
    }
}
