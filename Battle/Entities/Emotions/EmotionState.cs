using static DesignEnums;

public class EmotionState
{
    public EmotionType Type {  get; private set; }
    public bool IsUnlocked { get; private set; } // 특성으로 활성화되었는지
    public bool IsUsed { get; private set; }     // 이번 전투에서 사용했는지

    public bool IsUsable => IsUnlocked && !IsUsed; // 사용 가능 최종 판단

    public EmotionState(EmotionType type)
    {
        Type = type;
        Reset();
    }

    public void Unlock(bool isUnlocked) // 특성으로 활성화되었는지
    {
        IsUnlocked = isUnlocked;
    }

    public void Use(bool isUsed) // 이번 전투에서 사용했는지
    {
        IsUsed = isUsed;
    }

    public void Reset() // 감정 상태 초기화
    {
        IsUnlocked = false;
        //IsUnlocked = true; // 테스트 용도로 true
        IsUsed = false;
    }
}