using System.Collections.Generic;
using System.Linq;

public class NormalEnemyAI : BaseEnemyAI
{
    public override AIAction DecideAction(EnemyEntities self, List<BattleEntities> players, List<BattleEntities> allies)
    {
        List<SkillInstance> availableSkills = GetAvailableSkills(self);

        List<SkillInstance> usableSkills = availableSkills.Where(s => self.Skills.IndexOf(s) <= 1).ToList();
        if (!usableSkills.Any()) usableSkills.Add(self.Skills[0]); // 사용할 스킬이 없으면 기본 공격을 선택

        SkillInstance skillToUse = usableSkills[rng.Next(usableSkills.Count)];
        List<BattleEntities> targets = FindTargets(self, skillToUse, players, allies);

        return new AIAction(skillToUse, targets);
    }
}
