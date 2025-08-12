using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoSingleton<BattleManager>
{
    [field:SerializeField] public BattleSystem BattleSystem { get; private set; }
    [field:SerializeField] public EffectManager EffectManager { get; private set; }
    [SerializeField] private string battleSceneName = "Battle";
    [SerializeField] private string battleBGM = "Battle";

    protected override void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if(BattleSystem == null) BattleSystem = GetComponentInChildren<BattleSystem>();
        if (EffectManager == null) EffectManager = GetComponentInChildren<EffectManager>();

        base.Awake();

        if (UIManager.Instance != null)
        {
            UIManager.Instance.SetBattle();
            UIManager.Instance.BattleUI.Init(BattleSystem);
        }

        BattleSystem.Init();
        EffectManager.Init();
    }

    private void Start()
    {
        Scene battleScene = SceneManager.GetSceneByName(battleSceneName);
        SceneManager.SetActiveScene(battleScene);

        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(battleBGM);
        }
    }
}
