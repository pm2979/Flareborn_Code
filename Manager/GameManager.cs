using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public DataManager DataManager { get; private set; }
    public PartyManager PartyManager { get; private set; }
    public EnemyManager EnemyManager { get; private set; }
    public QuestManager QuestManager { get; private set; }
    public IconCacheManager IconCacheManager { get; private set; }
    public DungeonManager DungeonManager { get; private set; }

    [SerializeField] public string uiScene = "UIScene";

    public bool isUISceneLoaded = false;

    protected override void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 데이터 초기화
        DataManager = new DataManager();
        DataManager.Init();

        // 팩토리 초기화
        SkillFactory.Init(DataManager.SkillDataLoader);
        CharacterFactory.Init(DataManager.CharacterDataLoader, DataManager.JobDataLoader);
        ItemFactory.Init(DataManager.ItemBaseLoader, DataManager.EquipItemStatsLoader, DataManager.ConsumableEffectsLoader, DataManager.RuneDataLoader);
        BuffFactory.Init(DataManager.BuffDataLoader);
        TraitFactory.Init(DataManager.TraitConditionsLoader, DataManager.TraitEffectsLoader, DataManager.TraitDataLoader);

        // 컴포넌트 연결
        PartyManager = GetComponentInChildren<PartyManager>();
        EnemyManager = GetComponentInChildren<EnemyManager>();
        QuestManager = GetComponentInChildren<QuestManager>();
        DungeonManager = GetComponentInChildren<DungeonManager>();
        IconCacheManager = GetComponentInChildren<IconCacheManager>();

        PartyManager.Init();
        EnemyManager.Init();

        LoadUIScene();
    }

    public void LoadUIScene()
    {
        var uiSceneObj = SceneManager.GetSceneByName(uiScene);

        if (isUISceneLoaded) return;

        if (!uiSceneObj.isLoaded)
        {
            SceneManager.LoadSceneAsync(uiScene, LoadSceneMode.Additive);
            isUISceneLoaded = true;
        }  
    }

    private async void Start()
    {
        // 아이콘 프리로딩
        await IconCacheManager.PreloadAllIconsAsync();
    }
}