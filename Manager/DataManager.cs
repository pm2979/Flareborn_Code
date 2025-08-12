public class DataManager
{
    public SkillDataLoader SkillDataLoader {  get; private set; }
    public CharacterDataLoader CharacterDataLoader { get; private set; }
    public EnemyDataLoader EnemyDataLoader { get; private set; }
    public JobDataLoader JobDataLoader { get; private set; }

    // 아이템 관련 로더
    public ItemBaseLoader ItemBaseLoader { get; private set; }
    public EquipItemStatsLoader EquipItemStatsLoader { get; private set; }
    public ConsumableEffectsLoader ConsumableEffectsLoader { get; private set; }

    public RuneDataLoader RuneDataLoader { get; private set; }

    // 특성 관련 로더
    public TraitConditionsLoader TraitConditionsLoader { get; private set; }
    public TraitEffectsLoader TraitEffectsLoader { get; private set; }
    public TraitDataLoader TraitDataLoader { get; private set; }

    // 버프 로더
    public BuffDataLoader BuffDataLoader { get; private set; }
    
    // 던전 로더
    public DungeonDataLoader DungeonDataLoader { get; private set; }

    public void Init()
    {
        SkillDataLoader = new SkillDataLoader();
        CharacterDataLoader = new CharacterDataLoader();
        EnemyDataLoader = new EnemyDataLoader();
        JobDataLoader = new JobDataLoader();

        // 특성 관련 로더
        TraitConditionsLoader = new TraitConditionsLoader();
        TraitEffectsLoader = new TraitEffectsLoader();
        TraitDataLoader = new TraitDataLoader();

        // 아이템 관련 로더
        ItemBaseLoader = new ItemBaseLoader();
        EquipItemStatsLoader = new EquipItemStatsLoader();
        ConsumableEffectsLoader = new ConsumableEffectsLoader();
        RuneDataLoader = new RuneDataLoader();

        // 버프 로더
        BuffDataLoader = new BuffDataLoader();
        
        // 던전 로더
        DungeonDataLoader = new DungeonDataLoader();
    }
}