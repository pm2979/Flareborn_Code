using System.Collections.Generic;
using System.Linq;

public class BossEnemyAI : BaseEnemyAI
{
    private bool isEnraged = false;
    private readonly float healthThreshold;

    public BossEnemyAI(float rageThreshold = 0.5f) // 0.5f는 체력 50%를 의미
    {
        this.healthThreshold = rageThreshold;
    }

    public override AIAction DecideAction(EnemyEntities self, List<BattleEntities> players, List<BattleEntities> allies)
    {
        // 격노 상태 확인
        if (!isEnraged && (self.CurrHP / (float)self.MaxHP) <= healthThreshold)
        {
            isEnraged = true;
            self.BattleVisuals.EnrageEffect(true);
            Logger.Log($"{self.Name}이(가) 격노 상태에 돌입했습니다!");
        }

        SkillInstance skillToUse;
        List<SkillInstance> availableSkills = GetAvailableSkills(self);

        // 사용 가능한 스킬이 없는 경우
        if (!availableSkills.Any())
        {
            availableSkills.Add(self.Skills[0]);
        }

        if (isEnraged)
        {
            // 격노 패턴 > 사용 가능한 스킬 중에서 리스트의 가장 높은 인덱스를 가진 스킬을 우선적으로 사용
            skillToUse = availableSkills.OrderByDescending(s => self.Skills.IndexOf(s)).First();
        }
        else
        {
            // 일반 패턴

            // 가장 높은 인덱스를 가진 스킬 찾음 > 격노 스킬
            SkillInstance ultimateSkill = self.Skills.Count > 1 ? self.Skills[self.Skills.Count - 1] : null;

            // 사용 가능한 스킬 목록에서 격노 전용 스킬을 제외한 새로운 리스트를 생성
            List<SkillInstance> normalPhaseSkills = availableSkills;

            if (ultimateSkill != null)
            {
                normalPhaseSkills = availableSkills.Where(s => s != ultimateSkill).ToList();
            }

            // 만약 격노 스킬을 제외하고 나니 사용 가능한 스킬이 없다면
            if (!normalPhaseSkills.Any())
            {
                normalPhaseSkills.Add(availableSkills.First());
            }

            // 격노 스킬을 제외한 스킬 하나를 무작위로 선택
            skillToUse = normalPhaseSkills[rng.Next(normalPhaseSkills.Count)];
        }

        List<BattleEntities> targets = FindTargets(self, skillToUse, players, allies);
        return new AIAction(skillToUse, targets);
    }
}