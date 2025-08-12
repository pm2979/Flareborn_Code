using System;

public class StressController
{
    public int CurrentStress { get; private set; }
    public int MaxStress {  get; private set; }

    public bool IsCollapse { get; private set; } // 붕괴 상태 확인
    public bool IsAwaken { get; private set; } // 붕괴 상태 확인
    public Action OnStressMaxed; 

    public HumanityController Humanity { get; private set; }

    public StressController(int stress , HumanityController humanity, bool isCollapse = false)
    {
        CurrentStress = stress;
        MaxStress = 100;
        Humanity = humanity;
        IsCollapse = isCollapse;
    }

    public void AddStress(int amount) // 스트레스 증가
    {
        if (CurrentStress + amount < MaxStress)
        {
            CurrentStress = Math.Min(CurrentStress + amount, MaxStress);
        }
        else
        {
            int _amount = CurrentStress + amount - MaxStress;
            CurrentStress = MaxStress;
            Humanity?.ReduceHumanity(_amount);
        }

        if(CurrentStress >= MaxStress && !IsCollapse)
        {
            if(Humanity != null)
            {
                if (Humanity.IsTranscendent) return;

                OnStressMaxed?.Invoke();
            } 
            else
            {
                OnStressMaxed?.Invoke();
            }

        }
    }

    public void ReduceStress(int amount) // 스트레스 감소
    {
        CurrentStress = Math.Max(0, CurrentStress - amount);
    }

    public void SetStress(int value) // 스트레스 값을 특정 수치로 설정
    {
        CurrentStress = Math.Max(0, Math.Min(value, MaxStress));
    }

    public void SetCollapsed(bool isCollapse)
    {
        IsCollapse = isCollapse;
    }

    public void SetAwaken(bool isAwaken)
    {
        IsAwaken = isAwaken;
    }
}
