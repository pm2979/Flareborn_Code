using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DesignEnums;

public class EquipmentUI : BaseMenuUI
{
    [Header("슬롯 UI")]
    [SerializeField] private List<EquipmentSlotUI> equipmentSlots;

    [Header("장착 가능 장비 UI")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform inventoryItemContainer;
    [SerializeField] private GameObject itemSlotPrefab; // InventoryUI와 동일한 슬롯 프리팹 사용

    [Header("아이템 정보 UI")]
    [SerializeField] private ItemInfoUI itemInfoUI; // 아이템 정보창 참조
    [SerializeField] private Button equipButton;
    [SerializeField] private Button unEquipButton;

    [Header("캐릭터 정보")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI jobText;

    public CharacterInstance CurrentCharacter { get; private set; }
    public IconCacheManager IconCacheManager { get; private set; }
    public Inventory PartyInventory { get; private set; }
    public Party Party { get; private set; }

    public ItemInstance SelectedInventoryItem { get; private set; }
    public EquipmentSlotUI SelectedEquipmentSlot { get; private set; }

    private void Awake()
    {
        // 각 버튼에 대한 리스너를 한 번만 연결
        equipButton.onClick.AddListener(EquipSelectedItem);
        unEquipButton.onClick.AddListener(UnequipSelectedSlot);
    }

    public void Init(CharacterStatUI[] characterStatUIs, Party party)
    {
        IconCacheManager = GameManager.Instance.IconCacheManager;
        itemInfoUI.Init(IconCacheManager);

        for (int i = 0; i < characterStatUIs.Length; i++)
        {
            characterStatUIs[i].OnCharacterSelected += OnClickCharacterChange;
        }

        Party = party;
        PartyInventory = party.Inventory;

        foreach (EquipmentSlotUI slot in equipmentSlots)
        {
            slot.OnSlotClicked -= OnEquipmentSlotClicked;
            slot.OnSlotClicked += OnEquipmentSlotClicked;
        }

        inventoryPanel.SetActive(false);
        itemInfoUI.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (Party != null && Party.Members.Count > 0)
        {
            OnClickCharacterChange(Party.Members[0]);
            itemInfoUI.gameObject.SetActive(false);
        }
    }

    // 슬롯 업데이트
    private void OnCharacterEquipmentChanged(ItemType type)
    {
        // 변경된 타입의 슬롯을 찾습니다.
        EquipmentSlotUI slotToUpdate = equipmentSlots.FirstOrDefault(s => s.slotType == type);
        if (slotToUpdate != null)
        {
            ItemInstance item = CurrentCharacter.EquipmentController.GetEquippedItem(type);
            Sprite icon = null;
            if (item != null && !string.IsNullOrEmpty(item.BaseData.Icon))
            {
                icon = IconCacheManager.GetIcon(item.BaseData.Icon);
            }
            // 해당 슬롯만 UI를 갱신합니다.
            slotToUpdate.UpdateSlot(item, icon);
        }

        if (SelectedEquipmentSlot != null && SelectedEquipmentSlot.slotType == type)
        {
            OnEquipmentSlotClicked(slotToUpdate);
        }
        else
        {
            inventoryPanel.SetActive(false);
        }
    }

    private void OnClickCharacterChange(CharacterInstance currentCharacter)
    {
        if (!gameObject.activeInHierarchy) return;
        if (CurrentCharacter == currentCharacter) return;

        if (CurrentCharacter != null)
        {
            CurrentCharacter.EquipmentController.OnEquipmentChanged -= OnCharacterEquipmentChanged;
        }

        CurrentCharacter = currentCharacter;
        CurrentCharacter.EquipmentController.OnEquipmentChanged += OnCharacterEquipmentChanged;

        nameText.text = currentCharacter.Name;
        jobText.text = $"직업 : {currentCharacter.Job.JobType}";

        UpdateAllSlotsUI(currentCharacter);

        inventoryPanel.SetActive(false);
        itemInfoUI.gameObject.SetActive(false);
    }

    // UI 업데이트
    private void UpdateAllSlotsUI(CharacterInstance currentCharacter)
    {
        if (currentCharacter == null) return;

        foreach (EquipmentSlotUI slotUI in equipmentSlots)
        {
            ItemInstance item = currentCharacter.EquipmentController.GetEquippedItem(slotUI.slotType);
            Sprite icon = null;
            if (item != null && !string.IsNullOrEmpty(item.BaseData.Icon))
            {
                icon = IconCacheManager.GetIcon(item.BaseData.Icon);
            }
            slotUI.UpdateSlot(item, icon);
        }
    }

    private void OnEquipmentSlotClicked(EquipmentSlotUI selectedSlot)
    {
        SelectedEquipmentSlot = selectedSlot;
        SelectedInventoryItem = null;

        PopulateInventoryPanel(selectedSlot.slotType, CurrentCharacter.Job.JobType);
        inventoryPanel.SetActive(true);

        ItemInstance equippedItem = CurrentCharacter.EquipmentController.GetEquippedItem(selectedSlot.slotType);
        if (equippedItem != null)
        {
            itemInfoUI.gameObject.SetActive(true);
            itemInfoUI.ShowItemInfo(equippedItem.ItemName(), equippedItem.BaseData.Description, equippedItem.BaseData.Icon);
            unEquipButton.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
        }
        else
        {
            itemInfoUI.ClearItemInfo();
            unEquipButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
        }
    }

    private void PopulateInventoryPanel(ItemType filterType, JobType characterJobType)
    {
        foreach (Transform child in inventoryItemContainer)
        {
            Destroy(child.gameObject);
        }

        List<ItemInstance> itemsToDisplay = PartyInventory.GetItems(filterType);

        IEnumerable<ItemInstance> itemsForSlot = itemsToDisplay.Where(item =>
            !item.IsEquipped &&
            item.EquipStats != null &&
            (item.EquipStats.JobType == characterJobType || item.EquipStats.JobType == JobType.None)
        );

        foreach (ItemInstance item in itemsForSlot)
        {
            GameObject slotObj = Instantiate(itemSlotPrefab, inventoryItemContainer);
            InventoryItemSlotUI itemSlot = slotObj.GetComponent<InventoryItemSlotUI>();
            if (itemSlot != null)
            {
                itemSlot.InitSlot(IconCacheManager, itemInfoUI);
                itemSlot.SetSlot(item);
                itemSlot.GetComponent<Button>().onClick.AddListener(() => OnInventoryItemSelected(item));
            }
        }
    }

    private void OnInventoryItemSelected(ItemInstance selectedItem)
    {
        SelectedInventoryItem = selectedItem;
        itemInfoUI.gameObject.SetActive(true);
        itemInfoUI.ShowItemInfo(selectedItem.ItemName(), selectedItem.BaseData.Description, selectedItem.BaseData.Icon);
        equipButton.gameObject.SetActive(true);
        unEquipButton.gameObject.SetActive(false);
    }

    private void EquipSelectedItem()
    {
        if (SelectedInventoryItem != null && CurrentCharacter != null)
        {
            CurrentCharacter.EquipmentController.Equip(SelectedInventoryItem, CurrentCharacter.Job.JobType);
        }
    }

    private void UnequipSelectedSlot()
    {
        if (SelectedEquipmentSlot != null && CurrentCharacter != null)
        {
            CurrentCharacter.EquipmentController.Unequip(SelectedEquipmentSlot.slotType);
        }
    }

    private void OnDestroy()
    {
        if (CurrentCharacter != null)
        {
            CurrentCharacter.EquipmentController.OnEquipmentChanged -= OnCharacterEquipmentChanged;
        }
    }

    protected override MenuState GetMenuState()
    {
        return MenuState.Equipment;
    }
}
