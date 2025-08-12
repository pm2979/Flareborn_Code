using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;
using static DesignEnums;

public class Shop_NPCInventoryUI : MonoBehaviour
{
    [Header("필요 컴포넌트")]
    [SerializeField] private ShopNPCInventoryManager shopNPCInventoryManager;
    [SerializeField] private ShopUIController shopUIController; // 상점 UI 컨트롤러
    [SerializeField] private ItemInfoUI itemInfoUI;

    [Header("슬롯 배치할 부모 [장착 / 소모품 / 특수]")]
    [SerializeField] private Transform slotParent;

    // 각각의 카테고리별 인벤토리 슬롯이 배치될 부모 오브젝트
    [Header("슬롯 프리팹")]
    [SerializeField] private GameObject slotPrefab;

    // UI 상에서 관리할 슬롯 목록
    [SerializeField] private List<ShopItemSlotUI> shopItemSlotUIs = new();

    [Header("상점 NPC 정보")]
    // 상점 NPC의 이름 및 인벤토리 (판매하는 물건들이 담긴) 정보
    [field: SerializeField] public string ShopNPCName { get; private set; }
    [field: SerializeField] public Inventory ShopInventory { get; set; }
    public IconCacheManager IconCacheManager { get; protected set; }
    public bool HasShopInventoryLoaded { get; set; } = false;

    // 프로퍼티
    public ItemInfoUI ItemInfoUI => itemInfoUI;

    private void OnEnable()
    {
        shopUIController = GetComponent<ShopUIController>();
    }

    public void Init()
    {
        // ShopNPCInventoryManager에서 현재 상점 NPC의 인벤토리 및 NPC의 이름을 가져옴
        shopNPCInventoryManager = ShopNPCInventoryManager.Instance;
        ShopNPCInventory npcInventory = shopNPCInventoryManager.CurrentShopNPCInventory;
        IconCacheManager = GameManager.Instance.IconCacheManager;

        ItemInfoUI.Init(IconCacheManager);

        // UI에 로딩할 상점 NPC의 인벤토리 할당
        ShopInventory = npcInventory.ShopNPC_Inventory;

        // 시작 시 장비 UI만 보이도록 설정
        slotParent.gameObject.SetActive(true);

        ShopInventory.UIUpdate += RefreshUI;

        // 일반 아이템 슬롯이 기본적으로 보이도록 설정
        OnGeneralButton();

        // 상점 NPC의 인벤토리 로드가 완료되었음을 표시
        HasShopInventoryLoaded = true;
    }

    private void OnDisable()
    {
        if (ShopInventory == null) return;

        ShopInventory.UIUpdate -= RefreshUI;
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
                // 슬롯 생성 하며 슬롯에 필요한 컴포넌트 ShopItemSlotInit
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

        RefreshUI(ShopInventory.GetItems(type), type);
    }

    public void OnConsumableButton()
    {
        ItemType type = ItemType.Consumable;

        RefreshUI(ShopInventory.GetItems(type), type);
    }

    public void OnGeneralButton()
    {
        ItemType type = ItemType.General;

        RefreshUI(ShopInventory.GetItems(type), type);
    }

    public void OnRuneButton()
    {
        ItemType type = ItemType.Rune;
        RefreshUI(ShopInventory.GetItems(type), type);
    }

    public void OnArmorButton()
    {
        ItemType type = ItemType.Armor;
        RefreshUI(ShopInventory.GetItems(type), type);
    }

    public void OnAccessoryButton()
    {
        ItemType type = ItemType.Accessory;
        RefreshUI(ShopInventory.GetItems(type), type);
    }
}