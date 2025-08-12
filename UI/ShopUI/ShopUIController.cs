using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum ShopPanelType
{
    Buy,
    Sell
}

public class ShopUIController : MonoBehaviour
{
    [Header("Connected Component")]
    [SerializeField] private Shop_NPCInventoryUI shopNPCInventoryUI;
    [SerializeField] private Shop_PlayerInventoryUI playerInventoryUI;
    [SerializeField] private Shop_BuyButton buyButton;
    [SerializeField] private Shop_SellButton sellButton;
    [SerializeField] private GoldUI goldUI;

    [Header("Shop_NPCInventoryUI Configuration")]
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject contentParent;
    [SerializeField] private Button buyPanelButton;
    [SerializeField] private TextMeshProUGUI buyPanelButtonText;
    [SerializeField] private GameObject buyUI;
    [SerializeField] private Button sellPanelButton;
    [SerializeField] private TextMeshProUGUI sellPanelButtonText;
    [SerializeField] private GameObject sellUI;
    [SerializeField] private Color selectedButtonColor = new Color(180 / 255, 169 / 255, 169 / 255, 1f);
    [SerializeField] private Color deSelectedButtonColor = new Color(118 / 255, 105 / 255, 105 / 255, 1f);

    // 파티 정보
    private Party party;

    private ShopPanelType currentShopPanelType;

    // 프로퍼티
    public Shop_PlayerInventoryUI PlayerInventoryUI => playerInventoryUI;
    public Shop_NPCInventoryUI ShopNPCInventoryUI => shopNPCInventoryUI;
    public Shop_BuyButton BuyButton => buyButton;
    public Shop_SellButton SellButton => sellButton;
    public GoldUI GoldUI => goldUI;

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.dialogueEvents.onOpenShopUI += OpenShopUI;
    }

    private void OnDisable()
    {
        // #방어로직
        if (GameEventsManager.Instance == null) return;

        GameEventsManager.Instance.dialogueEvents.onOpenShopUI -= OpenShopUI;
    }

    private void Init()
    {
        // 전체적인 ShopUI는 안보이도록 처리
        contentParent.SetActive(false);

        // 각종 버튼 및 UI 컴포넌트 ShopInit
        party = GameManager.Instance.PartyManager.Party;
        shopNPCInventoryUI = GetComponent<Shop_NPCInventoryUI>();
        playerInventoryUI = GetComponent<Shop_PlayerInventoryUI>();

        // 파티에 연결되어있는 CurrencyWallet 객체를 GoldUI에 설정
        CurrencyWalletInit();
    }

    private void CurrencyWalletInit()
    {
        if (goldUI != null)
            goldUI.SetWallet(party.CurrencyWallet);
    }

    public void OpenShopUI()
    {
        // ShopPanelUI 활성화
        this.contentParent.SetActive(true);

        // 처음 시작시에는 구매 UI가 보이도록 설정
        buyPanelButtonText.color = selectedButtonColor;
        buyPanelButtonText.alpha = 1f;
        SetUIActive(ShopPanelType.Buy);

        // 구매하기 화면을 디폴트로 활성화
        OnBuyPanelButton();

    }

    public void OnBuyPanelButton()
    {
        // 상점 인벤토리 ShopInit
        shopNPCInventoryUI.Init();

        // 구매 버튼 && 판매 버튼 ShopInit
        // PlayerInventoryUI내에 위치해 있는 playerInventory 객체의 정보를 넘겨줌
        // Shop_NPCInventoryUI 내에 위치해 있는 shopNPCInventory 객체의 정보를 넘겨줌
        // GoldUI 컴포넌트 내에 위치해 있는 wallet 객체의 정보를 넘겨줌
        buyButton.Init(PlayerInventoryUI.Inventory, ShopNPCInventoryUI.ShopInventory, GoldUI.Wallet);

        // currentShopPanelTyhpe 설정
        currentShopPanelType = ShopPanelType.Buy;

        // Buy 버튼 클릭 시 ShopPanelUI 활성화
        SetUIActive(ShopPanelType.Buy);

        // 상점 인벤토리의 일반 아이템 카테고리가 기본으로 보이도록 설정
        shopNPCInventoryUI.OnGeneralButton();

        // ItemInfoUI에 보이는 정보 클리어 && 구매 버튼 비활성화
        shopNPCInventoryUI.ItemInfoUI.ClearItemInfo();
        buyButton.HideButton();
    }

    public void OnSellPanelButton()
    {
        // 플레이어 인벤토리 ShopInit
        playerInventoryUI.Init();

        // 판매 버튼 ShopInit
        // PlayerInventoryUI내에 위치해 있는 playerInventory 객체의 정보를 넘겨줌
        // Shop_NPCInventoryUI 내에 위치해 있는 shopNPCInventory 객체의 정보를 넘겨줌
        // GoldUI 컴포넌트 내에 위치해 있는 wallet 객체의 정보를 넘겨줌
        sellButton.Init(PlayerInventoryUI.Inventory, ShopNPCInventoryUI.ShopInventory, GoldUI.Wallet);

        // currentShopPanelType 설정
        currentShopPanelType = ShopPanelType.Sell;

        // Sell 버튼 클릭 시 ShopPanelUI 활성화
        SetUIActive(ShopPanelType.Sell);

        // 파티 인벤토리의 일반 아이템 카테고리가 기본으로 보이도록 설정
        playerInventoryUI.OnGeneralButton();

        // ItemInfoUI에 보이는 정보 클리어 && 판매 버튼 비활성화
        playerInventoryUI.ItemInfoUI.ClearItemInfo();
        sellButton.HideButton();
    }

    public void OnExitButton()
    {
        // Exit 버튼 클릭 시 ShopPanelUI 비활성화
        this.contentParent.SetActive(false);

        // 상점 인벤토리 초기화
        shopNPCInventoryUI.HasShopInventoryLoaded = false;
        shopNPCInventoryUI.ShopInventory = null;

        // 버튼들 비활성화 DeInit
        buyButton.DeInit();
        sellButton.DeInit();
    }

    public void SetUIActive(ShopPanelType panelType)
    {
        switch (panelType)
        {
            case ShopPanelType.Buy:
                // Buy UI 활성화
                this.buyUI.SetActive(true);
                this.sellUI.SetActive(false);
                // 버튼 색상 변경
                buyPanelButtonText.color = selectedButtonColor;
                sellPanelButtonText.color = deSelectedButtonColor;
                break;
            case ShopPanelType.Sell:
                // Sell UI 활성화
                this.sellUI.SetActive(true);
                this.buyUI.SetActive(false);
                // 버튼 색상 변경
                sellPanelButtonText.color = selectedButtonColor;
                buyPanelButtonText.color = deSelectedButtonColor;
                break;
        }

        SetTMPAlpha();
    }

    private void SetTMPAlpha()
    {
        // TextMeshProUGUI 컴포넌트의 알파 값을 설정하는 메서드
        buyPanelButtonText.alpha = 1f;
        sellPanelButtonText.alpha = 1f;
    }
}
