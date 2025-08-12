using System.Collections.Generic;
using System.Linq;

public class EliteEnemyAI : BaseEnemyAI
{
    public override AIAction DecideAction(EnemyEntities self, List<BattleEntities> players, List<BattleEntities> allies)
    {
        List<SkillInstance> availableSkills = GetAvailableSkills(self);
        if (!availableSkills.Any()) availableSkills.Add(self.Skills[0]); // 만약을 위한 대비

        // 사용 가능한 모든 스킬 중 하나 무작위 사용
        SkillInstance skillToUse = availableSkills[rng.Next(availableSkills.Count)];
        List<BattleEntities> targets = FindTargets(self, skillToUse, players, allies);

        return new AIAction(skillToUse, targets);
    }
}
