using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    [field: SerializeField] public InventoryUI InventoryUI { get; private set; }
    [field: SerializeField] public StatusUI StatusUI { get; private set; }
    [field: SerializeField] public MainUI MainUI { get; private set; }
    [field: SerializeField] public EquipmentUI EquipmentUI { get; private set; }
    [field: SerializeField] public RuneEquipUI RuneEquipUI { get; private set; }
    [field: SerializeField] public QuestLogUIToggle QuestLogUIToggle { get; private set; }
    [field: SerializeField] public GoldUI GoldUI { get; private set; }
    [field: SerializeField] public GameObject PartyStateMenu { get; private set; } // 파티 스탯 메뉴
    [field: SerializeField] public OptionUI OptionUI { get; private set; }
    [field: SerializeField] public CharacterStatUI[] CharacterStatUI { get; private set; }

    [field: SerializeField] private ConfirmUse confirmUse;

    public Party Party { get; private set; }

    private MenuState currentState;

    public void Init() // 초기 설정
    {
        Party = GameManager.Instance.PartyManager.Party;
        CharacterStat();

        MainUI.Init(this);
        InventoryUI.Init();
        EquipmentUI.Init(CharacterStatUI, Party);
        RuneEquipUI.Init(CharacterStatUI, Party);
        StatusUI.Init(CharacterStatUI);
        if (GoldUI != null)
            GoldUI.SetWallet(Party.CurrencyWallet);

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ChangeState(MenuState.Main);

        CharacterStat();
    }

    private void CharacterStat()
    {
        if (Party == null) return;

        for (int i = 0; i < CharacterStatUI.Length; i++)
        {
            if(Party.Members.Count <= i)
            {
                CharacterStatUI[i].gameObject.SetActive(false);
            }
            else
            {
                CharacterStatUI[i].gameObject.SetActive(true);
                CharacterStatUI[i].Set(Party.Members[i]);
            }
        }
    }

    public void OnClickBack() // 뒤로가기 버튼
    {
        if(currentState == MenuState.Main)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            ChangeState(MenuState.Main);
        }
        confirmUse.HideAll();
    }

    public void ChangeState(MenuState state) // 메뉴 상태 변화
    {
        currentState = state;

        if (currentState == MenuState.Quest || currentState == MenuState.Option)
        {
            PartyStateMenu.SetActive(false);
        }
        else
        {
            PartyStateMenu.SetActive(true);
        }

        MainUI.SetActive(currentState);
        InventoryUI.SetActive(currentState);
        StatusUI.SetActive(currentState);
        EquipmentUI.SetActive(currentState);
        OptionUI.SetActive(currentState);
        RuneEquipUI.SetActive(currentState);
        QuestLogUIToggle.SetActive(currentState);
    }
}

public enum MenuState
{
    Main,
    Inventory,
    Equipment,
    Status,
    Rune,
    Quest,
    Option
}