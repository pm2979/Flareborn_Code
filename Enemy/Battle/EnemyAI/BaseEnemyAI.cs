using static DesignEnums;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseEnemyAI : IEnemyAI
{
    protected static System.Random rng = new System.Random();

    public abstract AIAction DecideAction(EnemyEntities self, List<BattleEntities> players, List<BattleEntities> allies);

    protected List<SkillInstance> GetAvailableSkills(EnemyEntities self) // 사용 가능 스킬 반환
    {
        return self.Skills.Where(s => s.CurrentCooldown == 0).ToList();
    }

    // 주어진 스킬의 TargetType에 따라 타겟을 결정
    protected List<BattleEntities> FindTargets(EnemyEntities self, SkillInstance skill, List<BattleEntities> players, List<BattleEntities> allies)
    {
        List<BattleEntities> validTargets = new List<BattleEntities>();
        List<BattleEntities> alivePlayers = players.Where(p => !p.IsDead).ToList();
        List<BattleEntities> aliveAllies = allies.Where(a => !a.IsDead).ToList();

        if (!alivePlayers.Any()) return new List<BattleEntities>(); // 살아있는 플레이어가 없으면 타겟팅 불가

        switch (skill.TargetType)
        {
            case TargetType.OpponentSingle: // 무작위 플레이어 1명 타겟
                validTargets.Add(alivePlayers[rng.Next(alivePlayers.Count)]);
                break;

            case TargetType.OpponentAll: // 모든 적군 타겟
                validTargets.AddRange(alivePlayers);
                break;

            case TargetType.Self: // 자기 자신 타겟
                validTargets.Add(self);
                break;

            case TargetType.AllySingle: // 자기 자신을 제외한 무작위 아군 1명 타겟
                List<BattleEntities> otherAllies = aliveAllies.Where(a => a != self).ToList();
                if (otherAllies.Any())
                {
                    validTargets.Add(otherAllies[rng.Next(otherAllies.Count)]);
                }
                else // 다른 아군이 없으면 자신을 타겟
                {
                    validTargets.Add(self);
                }
                break;

            case TargetType.AllyAll: // 자신을 포함한 모든 아군 타겟
                validTargets.AddRange(aliveAllies);
                break;

            default:
                validTargets.Add(alivePlayers[rng.Next(alivePlayers.Count)]);
                break;
        }
        return validTargets;
    }
}
