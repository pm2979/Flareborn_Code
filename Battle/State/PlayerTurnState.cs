using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerTurnState : IBattleState
{
    public void EnterState(BattleSystem system)
    {
        PartyEntities player = system.CurrentPartyEntity();

        if(player.IsFlare)
        {
            if (player.Humanity.IsTranscendent)
            {
                // 초월 상태일 경우, 자동 행동 실행
                player.BattleVisuals.BattlePosition(system.PartyBattlePoint, () => system.StartCoroutine(PerformTranscendentAction(system, player)));
            }
            else
            {
                // 일반 상태일 경우, 메뉴 요청
                player.BattleVisuals.BattlePosition(system.PartyBattlePoint, () => system.RequestBattleMenu());
            }
        }
        else
        {
            player.BattleVisuals.BattlePosition(system.PartyBattlePoint, () => system.RequestBattleMenu());
        }
    }

    public void ExitState(BattleSystem system)
    {
        // 메뉴 숨기기
        system.HideAllMenus();
    }

    public void OnSkillSelected(BattleSystem system, SkillInstance skill) // 스킬 선택 시 호출
    {
        PartyEntities player = system.CurrentPartyEntity();
        SkillInstance _skill = skill;

        if (_skill.CurrentCooldown > 0)
        {
            Logger.Log($"아직 '{_skill.Name}' 스킬을 사용할 수 없습니다. ({_skill.CurrentCooldown} 턴 남음)");
            system.RequestSkillSelection();
            return;
        }
        
        player.SelectedSkill = _skill;

        TargetMenu(system, player, _skill);
    }

    private void TargetMenu(BattleSystem system, PartyEntities player, SkillInstance skill) // 스킬 타겟 타입에 따라 메뉴 또는 즉시 실행
    {
        system.HideAllMenus();

        switch (skill.TargetType)
        {
            case DesignEnums.TargetType.Self:
                system.StartCoroutine(PerformSkill(system, player, null));
                break;
            case DesignEnums.TargetType.OpponentSingle:
                system.RequestEnemySelection();
                break;
            case DesignEnums.TargetType.OpponentAll:
                system.StartCoroutine(PerformSkill(system, player, null));
                break;
            case DesignEnums.TargetType.AllySingle:
                system.RequestPartySelection();
                break;
            case DesignEnums.TargetType.AllyAll:
                system.StartCoroutine(PerformSkill(system, player, null));
                break;
        }
    }

    public void OnEnemySelected(BattleSystem system, int enemyIndex) // 적 선택 시 호출
    {
        PartyEntities player = system.CurrentPartyEntity();

        system.StartCoroutine(PerformSkill(system, player, enemyIndex));
    }

    public void OnPartySelected(BattleSystem system, int partyIndex) // 아군 선택 시 호출
    {
        // 단일 아군 스킬만 처리
        system.StartCoroutine(PerformSkill(system, system.CurrentTurnEntity(), partyIndex));
    }

    private IEnumerator PerformSkill(BattleSystem system, BattleEntities user, int? targetIndex)
    {
        SkillInstance skill = user.SelectedSkill;
        user.EmotionAdditionalEffect();
        yield return new WaitForSeconds(0.5f);

        IEnumerable<BattleEntities> targets = GetTargets(system, skill.TargetType, user, targetIndex);

        // PlaySkill 코루틴이 완전히 끝날 때까지 기다림
        yield return system.ActionController.PlaySkill(user, skill, targets);

        // 턴 종료 로직 또한 코루틴으로 만들어 모든 작업이 끝날 때까지 기다리도록 함
        yield return system.EndCurrentTurn();
    }


    private IEnumerator PerformTranscendentAction(BattleSystem system, PartyEntities user) // 초월 행동 코루틴
    {
        yield return new WaitForSeconds(1f);

        // 랜덤으로 타겟 지정
        List<BattleEntities> livingEnemies = system.EnemyBattlers.Where(e => !e.IsDead).ToList();
        if (livingEnemies.Any())
        {
            BattleEntities randomTarget = livingEnemies[Random.Range(0, livingEnemies.Count)];
            IEnumerable<BattleEntities> targets = new[] { randomTarget };

            yield return system.ActionController.PlaySkill(user, user.BasicAttack, targets);
        }

        yield return new WaitForSeconds(1f);
        yield return system.EndCurrentTurn();
    }

    private IEnumerable<BattleEntities> GetTargets(BattleSystem system, DesignEnums.TargetType type, BattleEntities user, int? index)
    {
        bool userIsPlayer = user.IsPlayer;

        var enemyList = userIsPlayer ? system.EnemyBattlers : system.PartyBattlers;
        var partyList = userIsPlayer ? system.PartyBattlers : system.EnemyBattlers;

        switch (type)
        {
            case DesignEnums.TargetType.Self:
                return new[] { user };
            case DesignEnums.TargetType.OpponentSingle:
                return new[] { enemyList[index.Value] };
            case DesignEnums.TargetType.OpponentAll:
                return enemyList;
            case DesignEnums.TargetType.AllySingle:
                return new[] { partyList[index.Value] };
            case DesignEnums.TargetType.AllyAll:
                return partyList;
            default:
                return Enumerable.Empty<BattleEntities>();
        }
    }
}