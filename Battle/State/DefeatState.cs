using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatState : IBattleState // 패배 상태
{
    public void EnterState(BattleSystem system)
    {
        EncounterSystem encounterSystem = EncounterSystem.Instance;
        
        system.SyncCharacterDataFromBattle();
        encounterSystem.OnBattleEnd();
        system.StartCoroutine(HandleDefeat(system));
    }

    public void ExitState(BattleSystem system) { }
    
    private IEnumerator HandleDefeat(BattleSystem system)
    {
        // 패배 UI 띄우기

        yield return new WaitForSeconds(2f);
        
        system.ReturnToVillage();
    }
}
