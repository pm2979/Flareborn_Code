using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static DesignEnums;

public class BattleUI : BaseUI
{
    private Stack<GameObject> panelHistory = new Stack<GameObject>();

    [Header("Battle Menu")]
    [SerializeField] private GameObject battleMenu;
    [SerializeField] private BattleSelectUI attackButton;
    [SerializeField] private Button skillMenuButton;
    [SerializeField] private Button flareMenuButton;
    [SerializeField] private Button emotionMenuButton;
    [SerializeField] private Button statusButton;
    [SerializeField] private Button runButton;

    [Header("Skill Selection")]
    [SerializeField] private GameObject skillSelectionMenu;
    [SerializeField] private BattleSelectUI[] skillSelect;

    [Header("Enemy Selection")]
    [SerializeField] private GameObject enemySelectionMenu;
    [SerializeField] private Button[] enemyButtons;

    [Header("Party Selection")]
    [SerializeField] private GameObject partySelectionMenu;
    [SerializeField] private Button[] partyButtons;

    [Header("Emotion Selection")]
    [SerializeField] private GameObject emotionSelectionMenu;
    [SerializeField] private Button[] emotionButton;

    [Header("Character UI")]
    [SerializeField] private GameObject CharacterBattlePanel;
    [SerializeField] private CharacterBattleUI[] CharacterBattleUI;

    [Header("Flare Selection")]
    [SerializeField] private GameObject flareSelectionMenu;
    [SerializeField] private BattleSelectUI flareSelect;

    [Header("Battle Reward")]
    [SerializeField] private BattleRewardUI battleRewardUI;

    [Header("Battle Status")]
    [SerializeField] private BattleStatusUI battleStatusUI;

    public BattleSystem BattleSystem { get; private set; }

    public void Init(BattleSystem system)
    {
        BattleSystem = system;

        battleRewardUI.Init(system);

        battleStatusUI.Init(() => {
            battleStatusUI.gameObject.SetActive(false);
            battleMenu.SetActive(true);
        });

        CharacterBattlePanel.SetActive(true);
        battleMenu.SetActive(false);
        enemySelectionMenu.SetActive(false);
        partySelectionMenu.SetActive(false);
        skillSelectionMenu.SetActive(false);
        emotionSelectionMenu.SetActive(false);
        battleStatusUI.gameObject.SetActive(false);

        BattleSystem.OnHideAllMenus += HideAllMenus;
        BattleSystem.OnBattleMenuRequested += HandleBattleMenuRequested;
        BattleSystem.OnSkillSelectionRequested += HandleSkillSelectionRequested;
        BattleSystem.OnEnemySelectionRequested += HandleEnemySelectionRequested;
        BattleSystem.OnPartySelectionRequested += HandlePartySelectionRequested;
        BattleSystem.OnBattleUIConnection += CharacterBattleUIInit;
        BattleSystem.OnFlareSelectionRequested += HandleFlareSelectionRequested;
        BattleSystem.OnBattleReward += StartRewardSequence;

        attackButton.button.onClick.AddListener(OnAttackButtonClicked);
        skillMenuButton.onClick.AddListener(OnSkillMenuButtonClicked);
        emotionMenuButton.onClick.AddListener(OnEmotionButtonClicked);
        flareMenuButton.onClick.AddListener(OnFlareMenuButtonClicked);
        statusButton.onClick.AddListener(OnStatusButtonClicked);
        runButton.onClick.AddListener(OnBattleRunClicked);
    }

    private void OnDisable()
    {
        if (BattleSystem == null) return;

        battleRewardUI.Disabled();

        BattleSystem.OnHideAllMenus -= HideAllMenus;
        BattleSystem.OnBattleMenuRequested -= HandleBattleMenuRequested;
        BattleSystem.OnSkillSelectionRequested -= HandleSkillSelectionRequested;
        BattleSystem.OnEnemySelectionRequested -= HandleEnemySelectionRequested;
        BattleSystem.OnPartySelectionRequested -= HandlePartySelectionRequested;
        BattleSystem.OnBattleUIConnection -= CharacterBattleUIInit;
        BattleSystem.OnFlareSelectionRequested -= HandleFlareSelectionRequested;
        BattleSystem.OnBattleReward -= StartRewardSequence;

        attackButton.button.onClick.RemoveListener(OnAttackButtonClicked);
        skillMenuButton.onClick.RemoveListener(OnSkillMenuButtonClicked);
        emotionMenuButton.onClick.RemoveListener(OnEmotionButtonClicked);
        flareMenuButton.onClick.RemoveListener(OnFlareMenuButtonClicked);
        statusButton.onClick.RemoveListener(OnStatusButtonClicked);
        runButton.onClick.RemoveListener(OnBattleRunClicked);
    }

    // 캐릭터 UI 연결
    private void CharacterBattleUIInit(PartyEntities entities, int i)
    {
        CharacterBattleUI[i].Init(entities);
    }

    private void StartRewardSequence(List<CharacterInstance> aliveCharacters, int totalExp, int totalGold, List<ItemInstance> items)
    {
        if(battleRewardUI != null)
        {
            CharacterBattlePanel.SetActive(false);

            battleRewardUI.StartRewardSequence(aliveCharacters, totalExp, totalGold, items);
        }
    }

    private void HandleBattleMenuRequested(SkillInstance skill) // 배틀 메뉴 UI
    {
        HideAllMenus();
        panelHistory.Clear();

        attackButton.Set(skill.Name, skill.CurrentCooldown, skill.Description);

        if (BattleSystem.CurrentPartyEntity().IsFlare == false)
            flareMenuButton.gameObject.SetActive(false);
        else
            flareMenuButton.gameObject.SetActive(true);

        battleMenu.SetActive(true);
        panelHistory.Push(battleMenu);
    }

    private void OnEmotionButtonClicked() // 감정 버튼
    {
        HideAllMenus();

        for (int i = 0; i < emotionButton.Length; i++)
        {
            emotionButton[i].gameObject.SetActive(false);

            if (BattleSystem.CurrentPartyEntity().Emotion.IsEmotionSelectable((EmotionType)i + 1))
            {
                emotionButton[i].gameObject.SetActive(true);
            }
        }

        emotionSelectionMenu.SetActive(true);
        panelHistory.Push(emotionSelectionMenu);
    }

    private void HandleSkillSelectionRequested(List<SkillInstance> skills) // 스킬 선택 버튼
    {
        HideAllMenus();
        for (int i = 0; i < skillSelect.Length; i++)
        {
            bool show = i < skills.Count;
            skillSelect[i].gameObject.SetActive(show);
            if (show)
            {
                skillSelect[i].Set(skills[i].Name, skills[i].CurrentCooldown, skills[i].Description);
                int idx = i;
                skillSelect[i].button.onClick.RemoveAllListeners();

                if (skills[i].CurrentCooldown == 0)
                    skillSelect[i].button.onClick.AddListener(() => { BattleSystem.OnSkillSelected(idx); });
            }
        }
        skillSelectionMenu.SetActive(true);
        panelHistory.Push(skillSelectionMenu);
    }

    private void HandleFlareSelectionRequested(SkillInstance skill) // 플레어 선택 버튼
    {
        HideAllMenus();
        flareSelect.gameObject.SetActive(true);

        flareSelect.Set(skill.Name, skill.CurrentCooldown, skill.Description);
        flareSelect.button.onClick.RemoveAllListeners();

        if (skill.CurrentCooldown == 0)
            flareSelect.button.onClick.AddListener(() => { BattleSystem.OnFlareSeleted(); });

        flareSelectionMenu.SetActive(true);
        panelHistory.Push(flareSelectionMenu);
    }

    private void HandleEnemySelectionRequested(List<string> names) // 적 선택 버튼
    {
        HideAllMenus();
        for (int i = 0; i < enemyButtons.Length; i++)
        {
            bool show = i < names.Count;
            enemyButtons[i].gameObject.SetActive(show);
            if (show)
            {
                enemyButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = names[i];
                int idx = i;
                enemyButtons[i].onClick.RemoveAllListeners();
                enemyButtons[i].onClick.AddListener(() => { BattleSystem.OnEnemySelected(idx); enemySelectionMenu.SetActive(false); });
            }
        }
        enemySelectionMenu.SetActive(true);
        panelHistory.Push(enemySelectionMenu);
    }

    private void HandlePartySelectionRequested(List<string> names) // 파티 선택 버튼
    {
        HideAllMenus();
        for (int i = 0; i < partyButtons.Length; i++)
        {
            bool show = i < names.Count;
            partyButtons[i].gameObject.SetActive(show);
            if (show)
            {
                partyButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = names[i];
                int idx = i;
                partyButtons[i].onClick.RemoveAllListeners();
                partyButtons[i].onClick.AddListener(() => { BattleSystem.OnPartySelected(idx); partySelectionMenu.SetActive(false); });
            }
        }
        partySelectionMenu.SetActive(true);
        panelHistory.Push(partySelectionMenu);
    }

    public void GoBack() // 뒤로가기
    {
        panelHistory.Peek().SetActive(false);

        panelHistory.Pop();

        panelHistory.Peek().SetActive(true);
    }

    private void HideAllMenus() // 모든 메뉴 숨기기
    {
        battleMenu.SetActive(false);
        skillSelectionMenu.SetActive(false);
        enemySelectionMenu.SetActive(false);
        partySelectionMenu.SetActive(false);
        flareSelectionMenu.SetActive(false);
    }

    // 버튼 클릭 시 호출
    private void OnAttackButtonClicked() => BattleSystem.OnAttackSelected();
    private void OnSkillMenuButtonClicked() => BattleSystem.RequestSkillSelection();
    private void OnFlareMenuButtonClicked() => BattleSystem.RequestFlareSelection();
    private void OnBattleRunClicked() => BattleSystem.OnRunSelected();

    private void OnStatusButtonClicked()
    {
        battleMenu.SetActive(false);
        battleStatusUI.SetTarget(BattleSystem.CurrentPartyEntity());
    }

    protected override UIState GetUIState()
    {
        return UIState.Battle;
    }

    // ------------감정 연결---------------
    public void OnSelectEmotion(EmotionType type)
    {
        BattleSystem.CurrentPartyEntity().SelectEmotion(type);
    }

    public void OnClick_SelectJoy() // 기쁨 감정 버튼
    {
        OnSelectEmotion(EmotionType.Joy);
    }

    public void OnClick_SelectAnger() // 분노 감정 버튼
    {
        OnSelectEmotion(EmotionType.Anger);
    }

    public void OnClick_SelecSorrow() // 슬픔 감정 버튼
    {
        OnSelectEmotion(EmotionType.Sorrow);
    }

    public void OnClick_SelectPleasure() // 즐거움 감정 버튼
    {
        OnSelectEmotion(EmotionType.Pleasure);
    }

    public void OnClick_SelectAffection() // 애정 감정 버튼
    {
        OnSelectEmotion(EmotionType.Affection);
    }

    public void OnClick_SelectDisgusst() // 미움 감정 버튼
    {
        OnSelectEmotion(EmotionType.Disgust);
    }

    public void OnClick_SelectDesire() // 욕망 감정 버튼
    {
        OnSelectEmotion(EmotionType.Desire);
    }
}