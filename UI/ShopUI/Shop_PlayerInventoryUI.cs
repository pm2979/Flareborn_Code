using System.Collections.Generic;
using UnityEngine;
using static DesignEnums;

public class Shop_PlayerInventoryUI : MonoBehaviour
{
    [Header("필요 컴포넌트")]
    [SerializeField] private ShopUIController shopUIController; // 상점 UI 컨트롤러
    [SerializeField] private ItemInfoUI itemInfoUI;

    [Header("슬롯 배치할 부모 [장착 / 소모품 / 특수]")]
    [SerializeField] private Transform slotParent;

    // 각각의 카테고리별 인벤토리 슬롯이 배치될 부모 오브젝트
    [Header("슬롯 프리팹")]
    [SerializeField] private GameObject slotPrefab;


    // UI 상에서 관리할 슬롯 목록
    [SerializeField] private List<ShopItemSlotUI> shopItemSlotUIs = new();

    [Header("플레이어의 정보")]
    [field: SerializeField] public Inventory Inventory { get; private set; }
    public IconCacheManager IconCacheManager { get; protected set; }

    // 프로퍼티 
    public ItemInfoUI ItemInfoUI => itemInfoUI;

    public void Start()
    {
        shopUIController = GetComponent<ShopUIController>();

        // 플레이어의 인벤토리 할당
        Inventory = GameManager.Instance.PartyManager.Party.Inventory;
        IconCacheManager = GameManager.Instance.IconCacheManager;

        ItemInfoUI.Init(IconCacheManager);
    }

    public void Init()
    {
        // 시작 시 장비 UI만 보이도록 설정
        slotParent.gameObject.SetActive(true);

        Inventory.UIUpdate += RefreshUI;

        // 일반 아이템 슬롯이 기본적으로 보이도록 설정
        OnGeneralButton();
    }

    private void OnDestroy()
    {
        if (Inventory == null) return;

        Inventory.UIUpdate -= RefreshUI;
    }

    // 외부에서 인벤토리 데이터를 받아 UI 갱신
    public void RefreshUI(List<ItemInstance> slots, ItemType type)
    {
        while (shopItemSlotUIs.Count < slots.Count)
        {
            ShopItemSlotUI newSlot = Instantiate(slotPrefab, slotParent).GetComponent<ShopItemSlotUI>();
            newSlot.InitSlot(IconCacheManager, itemInfoUI);
            shopItemSlotUIs.Add(newSlot);
        }

        // 모든 슬롯을 순회하며 데이터를 설정하거나 비활성화
        for (int i = 0; i < shopItemSlotUIs.Count; i++)
        {
            if (i < slots.Count)
            {
                shopItemSlotUIs[i].gameObject.SetActive(true); // 슬롯 활성화
                shopItemSlotUIs[i].SetSlot(slots[i]);
                shopItemSlotUIs[i].ShopItemSlotInit(shopUIController.BuyButton, shopUIController.SellButton); // 아이템 정보 UI 설정
            }
            else
            {
                shopItemSlotUIs[i].gameObject.SetActive(false); // 슬롯 비활성화
            }
        }
    }

    public void OnEquipButton()
    {
        ItemType type = ItemType.Weapon;

        RefreshUI(Inventory.GetItems(type), type);
    }

    public void OnConsumableButton()
    {
        ItemType type = ItemType.Consumable;

        RefreshUI(Inventory.GetItems(type), type);
    }

    public void OnGeneralButton()
    {
        ItemType type = ItemType.General;

        RefreshUI(Inventory.GetItems(type), type);
    }

    public void OnRuneButton()
    {
        ItemType type = ItemType.Rune;
        RefreshUI(Inventory.GetItems(type), type);
    }

    public void OnArmorButton()
    {
        ItemType type = ItemType.Armor;
        RefreshUI(Inventory.GetItems(type), type);
    }

    public void OnAccessoryButton()
    {
        ItemType type = ItemType.Accessory;
        RefreshUI(Inventory.GetItems(type), type);
    }
}