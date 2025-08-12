public enum BattleAction 
{ 
    Attack, 
    Run, 
    Skill 
}

public enum UIState
{
    Battle,
    Overworld
}

public enum InputEventContext
{
    DEFAULT,
    DIALOGUE,
}

public enum TextType
{
    Damage,
    Eva,
    Critical,
    Heal
}

#region [퀘스트 관련 Enums]
public enum E_QuestState
{
    REQUIREMENTS_NOT_MET,
    CAN_START,
    IN_PROGRESS,
    CAN_FINISH,
    FINISHED,
}
#endregion

