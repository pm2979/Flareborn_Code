using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;

public class BattleSystem : MonoBehaviour
{
    private IBattleState currentState;

    // UI 제어용 이벤트
    public event Action OnHideAllMenus; // 모든 UI 닫기
    public event Action<SkillInstance> OnBattleMenuRequested; // 배틀메뉴 UI 연결
    public event Action<List<SkillInstance>> OnSkillSelectionRequested; // 스킬 선택 UI 연결
    public event Action<List<string>> OnEnemySelectionRequested; // 적 선택 UI 연결
    public event Action<List<string>> OnPartySelectionRequested; // 파티 선택 UI 연결
    public event Action<PartyEntities, int> OnBattleUIConnection; // 캐릭터 상태 UI 연결
    public event Action<SkillInstance> OnFlareSelectionRequested;
    public event Action<List<CharacterInstance>, int, int, List<ItemInstance>> OnBattleReward;

    [Header("Spawn Points")] 
    [SerializeField] private Transform[] partySpawnPoints;
    [SerializeField] private Transform[] enemySpawnPoints;

    [field: SerializeField] public Transform PartyBattlePoint { get; private set; }

    public PartyManager partyManager;
    public EnemyManager enemyManager;
    private EffectManager effectManager;

    [field: SerializeField] public List<BattleEntities> AllBattlers { get; private set; } = new List<BattleEntities>();

    [field: SerializeField]
    public List<BattleEntities> PartyBattlers { get; private set; } = new List<BattleEntities>();

    [field: SerializeField]
    public List<BattleEntities> EnemyBattlers { get; private set; } = new List<BattleEntities>();

    [field: SerializeField] public int CurrentTurnIndex { get; private set; } = 0;
    public BattleActionController ActionController { get; private set; }

    public void Init()
    {
        partyManager = GameManager.Instance.PartyManager;
        enemyManager = GameManager.Instance.EnemyManager;
        effectManager = BattleManager.Instance.EffectManager;
        ActionController = new BattleActionController();
        OnHideAllMenus?.Invoke();
        ChangeState(new BattleStartState());
    }

    public void ChangeState(IBattleState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void StartTurnLoading() // 배틀 시작 로딩
    {
        int loadCompleted = 0;

        void CheckAllLoaded() // 비동기 작업 완료 체크
        {
            loadCompleted++;

            if (loadCompleted == 2)
            {
                SortBattlersBySpeed();
                StartNextTurn();
            }
        }

        CreatePartyEntities(CheckAllLoaded);
        CreateEnemyEntities(CheckAllLoaded);

    }

    public void StartNextTurn() // 턴 시작
    {
        if (CurrentTurnIndex >= AllBattlers.Count) // 새로운 턴 시작
        {
            CurrentTurnIndex = 0;
            SortBattlersBySpeed();
        }

        BattleEntities entity = CurrentTurnEntity();

        entity.OnTurnStart(); // 턴 시작 로직

        if (entity.CurrHP <= 0 || entity.IsStun > 0)
        {
            entity.ClearStun();
            CurrentTurnIndex++;
            StartNextTurn();
            return;
        }

        if (entity.IsPlayer)
        {
            ChangeState(new PlayerTurnState());
        }
        else
        {
            ChangeState(new EnemyTurnState());
        }
    }

    public IEnumerator EndCurrentTurn()
    {
        // 현재 턴의 엔티티가 비동기 턴 종료 로직을 가지고 있는지 확인
        if (CurrentTurnEntity() is PartyEntities partyEntity)
        {
            // PartyEntities에 정의한 OnTurnEndAsync 호출
            Task turnEndTask = partyEntity.OnTurnEndAsync();
            // 해당 Task가 완료될 때까지 코루틴 대기
            yield return new WaitUntil(() => turnEndTask.IsCompleted);
        }
        else
        {
            // 다른 엔티티들은 기존의 동기 메서드 호출
            CurrentTurnEntity().OnTurnEnd();
        }

        RemoveDeadBattlers();

        if (PartyBattlers.Count == 0)
        {
            ChangeState(new DefeatState());
            yield break; // 상태가 변경되었으므로 코루틴 종료
        }
        else if (EnemyBattlers.Count == 0)
        {
            ChangeState(new VictoryState());
            yield break; // 상태가 변경되었으므로 코루틴 종료
        }

        // 턴 종료 후 원래 위치로 돌아가는 로직
        if (CurrentTurnEntity().IsPlayer)
        {
            bool positionReturned = false;
            CurrentPartyEntity().BattleVisuals.BattlePosition(CurrentPartyEntity().Transform, () =>
            {
                positionReturned = true;
            });
            yield return new WaitUntil(() => positionReturned);
        }

        CurrentTurnIndex++;
        StartNextTurn();
    }

    public BattleEntities CurrentTurnEntity() // 현재 턴 유닛 반환
    {
        return AllBattlers[CurrentTurnIndex];
    }

    public PartyEntities CurrentPartyEntity() // 현재 파티원
    {
        return CurrentTurnEntity() as PartyEntities;
    }

    public EnemyEntities CurrentEnemyEntity() // 현재 적
    {
        return CurrentTurnEntity() as EnemyEntities;
    }

    public void SortBattlersBySpeed() // 배틀 유닛 턴 순서
    {
        AllBattlers = AllBattlers.OrderByDescending(b => b.SPD).ToList();
    }

    public void CreatePartyEntities(Action onComplete) // 파티 유닛 생성
    {
        IReadOnlyList<CharacterInstance> currentParty = partyManager.GetAliveParty();

        int loadedCount = 0;
        int totalCount = currentParty.Count;

        for (int i = 0; i < totalCount; i++)
        {
            int index = i;
            CharacterInstance member = currentParty[i];
            PartyEntities entity = new PartyEntities();
            entity.SetEntityValues(member.Name, member.CurrentHp, member.MaxHp, member.ATK, member.SATK, member.DEF,
                member.SDEF, member.SPD, member.Critical, member.EVA, member.Level, true, member.IsFlare);

            if (entity.IsFlare)
            {
                entity.SetFlareborn(member,member.FS, member.CurrentStress, member.CurrentHG, member.SP, member.BasicAttack, member.EquippedSkills,
                    partySpawnPoints[i], member.FlareSkill, member.Traits);
            }
            else
            {
                entity.SetAshborn(member,member.CurrentStress, member.CurrentHG, member.SP, member.BasicAttack, member.EquippedSkills, partySpawnPoints[i], member.Traits);
            }

            OnBattleUIConnection(entity, index);

            member.LoadCharacterBattlePrefab(this, partySpawnPoints[index].position, (go) =>
            {
                if (go == null)
                {
                    Logger.LogError("로드 실패");
                    return;
                }

                BattleVisuals visuals = go.GetComponent<BattleVisuals>();
                entity.SetBattleVisuals(visuals, effectManager);

                AllBattlers.Add(entity);
                PartyBattlers.Add(entity);

                loadedCount++;
                if (loadedCount == totalCount)
                    onComplete?.Invoke();
            });
        }
    }

    public void CreateEnemyEntities(Action onComplete) // 적 유닛 생성
    {
        List<EnemyInstance> currentEnemy = enemyManager.GetCurrentEnemies();

        int loadedCount = 0;
        int totalCount = currentEnemy.Count;

        for (int i = 0; i < totalCount; i++)
        {
            int index = i;
            EnemyInstance enemy = currentEnemy[i];
            EnemyEntities entity = new EnemyEntities();
            entity.SetEntityValues(enemy.Name, enemy.CurrentHp, enemy.MaxHp, enemy.ATK, enemy.SATK, enemy.DEF,
                enemy.SDEF, enemy.SPD, enemy.Critical, enemy.EVA, 0, false);

            entity.SetEnemy(enemy.SG, enemy.EnemyType, enemy.Skills);


            enemy.LoadEnemyBattlePrefab(this, enemySpawnPoints[index].position, (go) =>
            {
                if (go == null)
                {
                    Logger.LogError("로드 실패");
                    return;
                }

                BattleVisuals visuals = go.GetComponent<EnemyBattleVisuals>();
                visuals.SetStartingValues(enemy.CurrentHp, enemy.MaxHp, 0, enemy.SG, 0);
                entity.SetBattleVisuals(visuals, effectManager);

                AllBattlers.Add(entity);
                EnemyBattlers.Add(entity);

                loadedCount++;
                if (loadedCount == totalCount)
                    onComplete?.Invoke();
            });
        }
    }

    public GameObject CreateEntities(GameObject obj, Vector3 pos)
    {
        return Instantiate(obj, pos, Quaternion.identity);
    }

    public void RemoveDeadBattlers() // 죽은 유닛 삭제
    {
        PartyBattlers.RemoveAll(b => b.CurrHP <= 0);
        EnemyBattlers.RemoveAll(b => b.CurrHP <= 0);
    }

    private IEnumerator AttemptRun() // 도망 시도 로직 처리
    {
        HideAllMenus();

        // 가장 빠른 적의 속도를 기준으로 확류 계산
        int fastestEnemySpeed = EnemyBattlers.Any() ? EnemyBattlers.Max(e => e.SPD) : 0;
        int playerSpeed = CurrentTurnEntity().SPD;

        // 도망 확률을 계산
        float successChance = 50f + (playerSpeed - fastestEnemySpeed);
        successChance = Mathf.Clamp(successChance, 10f, 95f);

        int roll = UnityEngine.Random.Range(0, 100);

        if (roll < successChance)
        {
            // 도망 성공
            Logger.Log("도망 성공");
            yield return new WaitForSeconds(2f);
            ChangeState(new RunState());
        }
        else
        {
            // 도망 실패
            Logger.Log("도망 실패");
            yield return new WaitForSeconds(2f);
            yield return EndCurrentTurn();
        }
    }

    // --------------------UI 요청 메서드------------------------

    // 전투 메뉴 요청
    public void RequestBattleMenu()
    {
        OnBattleMenuRequested?.Invoke(CurrentPartyEntity().BasicAttack);
    }

    // 스킬 목록 요청
    public void RequestSkillSelection()
    {
        OnSkillSelectionRequested?.Invoke(CurrentPartyEntity().Skills);
    }

    // 플레어 목록 요청
    public void RequestFlareSelection()
    {
        OnFlareSelectionRequested?.Invoke(CurrentPartyEntity().FlareSkill);
    }

    // 적 선택 메뉴 요청
    public void RequestEnemySelection()
    {
        // 이름 리스트 수동 생성
        List<string> names = new List<string>();
        for (int i = 0; i < EnemyBattlers.Count; i++)
        {
            names.Add(EnemyBattlers[i].Name);
        }

        OnEnemySelectionRequested?.Invoke(names);
    }

    // 아군 선택 메뉴 요청
    public void RequestPartySelection()
    {
        // 이름 리스트 수동 생성
        List<string> names = new List<string>();
        for (int i = 0; i < PartyBattlers.Count; i++)
        {
            names.Add(PartyBattlers[i].Name);
        }

        OnPartySelectionRequested?.Invoke(names);
    }

    // 보상 UI 요청
    public void OnBattleRewarded(List<CharacterInstance> aliveCharacters, int totalExp, int totalGold, List<ItemInstance> items)
    {
        if (currentState is VictoryState)
        {
            OnBattleReward?.Invoke(aliveCharacters, totalExp, totalGold, items);

            if(OnBattleReward == null)
            {
                foreach (CharacterInstance member in aliveCharacters)
                {
                    member.GainExp(totalExp);
                }

                ReturnToOverworld();
            }
        }
    }

    // -----------------------UI에서 호출----------------------------

    // 공격 버튼 눌렀을 때
    public void OnAttackSelected()
    {
        PlayerTurnState state = currentState as PlayerTurnState;
        if (state != null)
        {
            SkillInstance skill = CurrentPartyEntity().BasicAttack;
            state.OnSkillSelected(this, skill);
        }
    }

    // 스킬 버튼 눌렀을 때
    public void OnSkillSelected(int idx)
    {
        PlayerTurnState state = currentState as PlayerTurnState;
        if (state != null)
        {
            SkillInstance skill = CurrentPartyEntity().Skills[idx];
            state.OnSkillSelected(this, skill);
        }
    }

    // 플레어 선택
    public void OnFlareSeleted()
    {
        PlayerTurnState state = currentState as PlayerTurnState;
        if (state != null)
        {
            SkillInstance skill = CurrentPartyEntity().FlareSkill;
            state.OnSkillSelected(this, skill);
        }

    }

    // 적 선택 버튼 눌렀을 때 PlayerTurnState.OnEnemySelected 호출
    public void OnEnemySelected(int idx)
    {
        PlayerTurnState state = currentState as PlayerTurnState;
        if (state != null)
            state.OnEnemySelected(this, idx);
    }

    // 아군 선택 버튼 눌렀을 때 PlayerTurnState.OnPartySelected 호출
    public void OnPartySelected(int idx)
    {
        PlayerTurnState state = currentState as PlayerTurnState;
        if (state != null)
            state.OnPartySelected(this, idx);
    }

    // 도망 버튼 호출 메서드
    public void OnRunSelected()
    {
        if (currentState is PlayerTurnState)
        {
            StartCoroutine(AttemptRun());
        }
    }

    public void HideAllMenus() // 모든 UI 닫기
    {
        OnHideAllMenus?.Invoke();
    }

    // -----------------------전투 종료----------------------------

    public void SyncCharacterDataFromBattle() // 능력치 동기화
    {
        // AllBattlers에서 플레이어 파티원 필터링
        foreach (PartyEntities entity in AllBattlers.OfType<PartyEntities>())
        {
            if (entity.SourceInstance == null)
            {
                continue;
            }

            // 체력 동기화
            entity.SourceInstance.SetCurrentHp(entity.CurrHP);

            // 스트레스 동기화
            entity.SourceInstance.SetCurrentStress(entity.Stress.CurrentStress);


            // 인간성 동기화
            if (entity.IsFlare && entity.Humanity != null)
            {
                entity.SourceInstance.SetCurrentHg(entity.Humanity.CurrentHumanity);
            }
        }
    }

    public void ReturnToOverworld() // 전투 종료 후 월드로 복귀
    {
        SoundManager.Instance.StopBGM();
        
        string battleSceneName = DungeonManager.Instance.CurrentDungeon?.BattleSceneName;

        if (!string.IsNullOrEmpty(battleSceneName))
        {
            Scene scene = SceneManager.GetSceneByName(battleSceneName);
            if (scene.IsValid() && scene.isLoaded)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }

        EncounterSystem encounterSystem = EncounterSystem.Instance;
        if (encounterSystem != null)
        {
            encounterSystem.OnBattleEnd();
        }
        else
        {
            Debug.LogWarning("EncounterSystem을 찾을 수 없습니다. 오버월드 씬에 존재하는지 확인하세요.");
        }
    }
    
    public void ReturnToVillage()
    {
        // 비동기 씬 로드
        StartCoroutine(LoadVillageAndMovePlayer());
    }

    private IEnumerator LoadVillageAndMovePlayer()
    {
        Debug.Log("[LoadVillageAndMovePlayer] 씬 로드 시작");

        // 1. VillageScene을 싱글 모드로 로드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("2_VillageScene", LoadSceneMode.Single);
        while (!asyncLoad.isDone)
            yield return null;

        Debug.Log("[LoadVillageAndMovePlayer] 씬 로드 완료");
    }
}