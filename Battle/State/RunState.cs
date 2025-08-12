using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunState : IBattleState
{
    public void EnterState(BattleSystem system)
    {
        EncounterSystem encounterSystem = GameObject.FindFirstObjectByType<EncounterSystem>();
        
        system.SyncCharacterDataFromBattle(); // 필요한 데이터 저장
        encounterSystem.OnBattleEnd();
        system.StartCoroutine(HandleRun(system));
    }

    public void ExitState(BattleSystem system) { }

    private IEnumerator HandleRun(BattleSystem system)
    {
        // 도망 UI 띄우기
        
        yield return new WaitForSeconds(2f);
        
        system.ReturnToVillage();
    }
}