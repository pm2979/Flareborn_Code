using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTestLauncher : MonoBehaviour
{
    [Header("배틀 씬 이름")]
    [SerializeField] private string battleSceneName = "BattleScene_PM";
    [SerializeField] private string unLoadSceneName = "Test_PM";

    [Header("테스트용 파티 멤버 이름")]
    [SerializeField] private int[] testPartyMemberKey;

    [Header("테스트용 적 조우 설정")]
    [SerializeField] private Encounter[] testEncounters;

    private void Start()
    {
        SetupParty();
        SetupEnemies();

        ChangeToBattleScene(unLoadSceneName, battleSceneName);
    }

    // 이 함수를 호출하여 씬 전환을 시작합니다.
    public void ChangeToBattleScene(string unLoadSceneName, string battleSceneName)
    {
        StartCoroutine(ChangeSceneRoutine(unLoadSceneName, battleSceneName));
    }

    private IEnumerator ChangeSceneRoutine(string unLoadSceneName, string battleSceneName)
    {
        // unLoadSceneName에 해당하는 씬을 비동기 방식으로 언로드하고 완료될 때까지 대기합니다.
        if (!string.IsNullOrEmpty(unLoadSceneName) && SceneManager.GetSceneByName(unLoadSceneName).isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(unLoadSceneName);
        }

        // battleSceneName에 해당하는 씬을 Additive 모드로 비동기 로드하고 완료될 때까지 대기합니다.
        yield return SceneManager.LoadSceneAsync(battleSceneName, LoadSceneMode.Additive);
        yield return new WaitForSeconds(2f);
        // 여기에 씬 로드가 완료된 후 실행할 코드를 추가할 수 있습니다.
        //BattleManager.Instance.BattleSystem.LoadAll();
    }

    private void SetupParty()
    {
        PartyManager partyManager = FindAnyObjectByType<PartyManager>();
        Party party = partyManager.Party;
        if (partyManager == null)
        {
            Debug.LogError("PartyManager가 씬에 없습니다.");
            return;
        }

        // 기존 파티 초기화
        var partyField = typeof(Party).GetField("members", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        partyField.SetValue(party, new List<CharacterInstance>());

        // 테스트 멤버 추가
        foreach (var id in testPartyMemberKey)
        {
            partyManager.AddMemberByKey(id);
        }
    }

    private void SetupEnemies()
    {
        EnemyManager enemyManager = FindAnyObjectByType<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogError("EnemyManager가 씬에 없습니다.");
            return;
        }

        List<int> ids = new();
        foreach (var encounter in testEncounters)
        {
            ids.Add(encounter.enemyKey);
        }

        enemyManager.GenerateEnemiesByEncounter(ids);
    }
}
