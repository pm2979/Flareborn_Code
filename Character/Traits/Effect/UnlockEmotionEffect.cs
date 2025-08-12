using static DesignEnums;

public class UnlockEmotionEffect : ITraitEffect // 감정 해제
{
    private EmotionType emotion; // 해제할 감정 타입

    public UnlockEmotionEffect(EmotionType emotion)
    {
        this.emotion = emotion;
    }

    // 특성 효과를 적용
    public void Apply(PartyEntities owner)
    {
        if (owner.Emotion != null)
        {
            owner.Emotion.UnlockEmotion(emotion, true);
        }
    }

    // 적용된 특성 효과를 해제
    public void UnApply(PartyEntities owner)
    {
        if (owner.Emotion != null)
        {
            owner.Emotion.UnlockEmotion(emotion, false);
        }
    }
}
