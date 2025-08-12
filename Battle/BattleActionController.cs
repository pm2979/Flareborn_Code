using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BattleActionController
{
    public EffectManager effectManager = BattleManager.Instance.EffectManager;

    public IEnumerator PlaySkill(BattleEntities attacker, SkillInstance skill, IEnumerable<BattleEntities> targets)
    {
        bool animationDone = false;
        attacker.BattleVisuals.animationHandler.PlayAttackAnimation(() =>
        {
            animationDone = true;
        });
        
        yield return new WaitUntil(() => animationDone);

        if (!targets.Any())
        {
            yield break;
        }

        // 모든 타겟에 대한 스킬 사용(Use) Task를 저장할 리스트
        List<Task> skillTasks = new List<Task>();

        // 이펙트 재생과 스킬 로직 실행을 분리하여 관리
        if (!string.IsNullOrEmpty(skill.EffectPrefab))
        {
            // 이펙트 재생 Task
            Task effectTask = effectManager.PlayEffectAsync(
                skill.EffectPrefab,
                attacker,
                skill.EffectSpawn,
                targets,
                () => // 이펙트가 특정 시점에 도달했을 때 로직 실행
                {
                    // skill.Use가 Task를 반환하므로 리스트에 추가
                    skillTasks.Add(skill.Use(attacker, targets));
                }
            );

            // 이펙트 Task와 모든 스킬 로직 Task가 완료될 때까지 대기
            yield return new WaitUntil(() => effectTask.IsCompleted);

            // skill.Use 호출로 생성된 모든 Task가 완료될 때까지 기다림
            if (skillTasks.Any())
            {
                Task allSkillTasks = Task.WhenAll(skillTasks);
                yield return new WaitUntil(() => allSkillTasks.IsCompleted);
            }
        }
        else // 이펙트가 없는 경우
        {
            skillTasks.Add(skill.Use(attacker, targets));

            // 모든 스킬 로직 Task가 완료될 때까지 대기
            if (skillTasks.Any())
            {
                Task allSkillTasks = Task.WhenAll(skillTasks);
                yield return new WaitUntil(() => allSkillTasks.IsCompleted);
            }
        }
    }
}
