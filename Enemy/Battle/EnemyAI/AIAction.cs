using System.Collections.Generic;


// AI의 결정을 담는 간단한 클래스
public class AIAction
{
    public SkillInstance Skill { get; }
    public List<BattleEntities> Targets { get; }

    public AIAction(SkillInstance skill, List<BattleEntities> targets)
    {
        Skill = skill;
        Targets = targets;
    }
}
