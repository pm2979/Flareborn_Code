using System;
using System.Collections.Generic;
using static DesignEnums;

[Serializable]
public class EmotionController
{
    public TraitController TraitController { get; private set; }
    public StressController StressController { get; private set; }
    [field:NonSerialized] public Dictionary<EmotionType, EmotionState> Emotions { get; private set; }

    public EmotionType? SelectedEmotionForTurn { get; private set; } // 이번턴에 선택된 감정

    public EmotionController(TraitController traitController, StressController stressController)
    {
        TraitController = traitController;
        StressController = stressController;

        Emotions = new Dictionary<EmotionType, EmotionState>();

        InitializeEmotions();
    }

    // 감정 초기화
    private void InitializeEmotions()
    {
        foreach (EmotionType type in Enum.GetValues(typeof(EmotionType)))
        {
            if (type != EmotionType.None)
            Emotions[type] = new EmotionState(type);
        }
    }

    // 특정 감정을 선택할 수 있는지 확인
    public bool IsEmotionSelectable(EmotionType type)
    {
        if(StressController.IsCollapse) return false;

        return Emotions.TryGetValue(type, out var state) && state.IsUsable;
    }


    // 플레이어가 이번 턴에 사용할 감정을 선택
    public void SelectEmotionForTurn(EmotionType type)
    {
        if (IsEmotionSelectable(type))
        {
            SelectedEmotionForTurn = type;
        }
    }

    public void UnSelectEmotion()
    {
        SelectedEmotionForTurn = null;
    }

    public bool IsSelectEmotion(EmotionType type) // 선택한 감정 확인
    {
        if(SelectedEmotionForTurn.HasValue)
        {
            Logger.Log($"{type}");
            return SelectedEmotionForTurn == type;
        }
        else return false;
    }

    // 감정을 사용 처리
    public void ConsumeSelectedEmotion()
    {
        if (SelectedEmotionForTurn.HasValue)
        {
            Emotions[SelectedEmotionForTurn.Value].Use(true);
            Logger.Log($"{SelectedEmotionForTurn.Value} 잠금");
        }
    }

    // 턴 종료 시 선택된 감정을 초기화
    public void OnTurnEnd()
    {
        ConsumeSelectedEmotion();
        SelectedEmotionForTurn = null;
    }

    // 전투 종료 시 모든 감정의 잠금 및 사용 상태를 초기화
    public void OnBattleEnd()
    {
        SelectedEmotionForTurn = null;
        foreach (var state in Emotions.Values)
        {
            state.Reset();
        }
    }

    // 특성에 의해 호출될 메서드들---------------------------------------
    public void UnlockEmotion(EmotionType type, bool isUnlocked)
    {
        if (Emotions.TryGetValue(type, out var state))
        {
            state.Unlock(isUnlocked);
        }
    }
}