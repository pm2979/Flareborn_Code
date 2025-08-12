using System.Collections.Generic;
using UnityEngine;
using static DesignEnums;

public class InventoryUI : BaseMenuUI
{
    // 각각의 카테고리별 인벤토리 슬롯이 배치될 부모 오브젝트

    [Header("슬롯 배치할 부모 [장착 / 소모품 / 특수]")]
    [SerializeField] private Transform slotParent;

    [Header("슬롯 프리팹")]
    [SerializeField] private GameObject slotPrefab;

    [Header("아이템 정보창")]
    [SerializeField] private ItemInfoUI itemInfoUI;

    // UI 상에서 관리할 슬롯 목록
    [SerializeField] private List<InventoryItemSlotUI> itemSlotUIs = new();

    [SerializeField] private ConfirmUse confirmUse;

    public Inventory Inventory { get; private set; }
    public IconCacheManager IconCacheManager { get; private set; }

    public void Init()
    {
        confirmUse.Initialize();

        Inventory = GameManager.Instance.PartyManager.Party.Inventory;
        IconCacheManager = GameManager.Instance.IconCacheManager;
        itemInfoUI.Init(IconCacheManager);

        // 시작 시 장비 UI만 보이도록 설정
        slotParent.gameObject.SetActive(true);

        OnWeaponButton();
    }

    private void OnEnable()
    {
        Inventory.UIUpdate += RefreshUI;

        OnWeaponButton();
    }


    private void OnDisable()
    {
        if (Inventory == null) return;
        
        Inventory.UIUpdate -= RefreshUI;
    }

    // 외부에서 인벤토리 데이터를 받아 UI 갱신
    public void RefreshUI(List<ItemInstance> slots, ItemType type)
    {
        confirmUse?.HideAll();

        while (itemSlotUIs.Count < slots.Count)
        {
            InventoryItemSlotUI newSlot = Instantiate(slotPrefab, slotParent).GetComponent<InventoryItemSlotUI>();
            newSlot.InitSlot(IconCacheManager, itemInfoUI);
            newSlot.SetConfirmUse(confirmUse);

            itemSlotUIs.Add(newSlot);
        }

        // 모든 슬롯을 순회하며 데이터를 설정하거나 비활성화
        for (int i = 0; i < itemSlotUIs.Count; i++)
        {
            if (i < slots.Count)
            {
                itemSlotUIs[i].SetSlot(slots[i]);
                itemSlotUIs[i].gameObject.SetActive(true); // 슬롯 활성화
            }
            else
            {
                itemSlotUIs[i].gameObject.SetActive(false); // 슬롯 비활성화
            }
        }
    }

    public void OnWeaponButton()
    {
        ItemType type = ItemType.Weapon;

        RefreshUI(Inventory.GetItems(type), type);
    }

    public void OnAromrButton()
    {
        ItemType type = ItemType.Armor;

        RefreshUI(Inventory.GetItems(type), type);
    }

    public void OnAccessoryButton()
    {
        ItemType type = ItemType.Accessory;

        RefreshUI(Inventory.GetItems(type), type);
    }

    public void OnRuneButton()
    {
        ItemType type = ItemType.Rune;

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

    public void OnQuestButton()
    {
        ItemType type = ItemType.Quest;

        RefreshUI(Inventory.GetItems(type), type);
    }

    protected override MenuState GetMenuState()
    {
        return MenuState.Inventory;
    }
}