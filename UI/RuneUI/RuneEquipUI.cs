using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DesignEnums;

public class RuneEquipUI : BaseMenuUI
{
    [Header("슬롯 UI")]
    [SerializeField] private Transform runeSlotContainer;
    [SerializeField] private GameObject runeSlotPrefab;
    private readonly List<RuneSlotUI> _runeSlotUIs = new List<RuneSlotUI>(); // UI 슬롯 풀

    [Header("장착 가능 룬 UI")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform inventoryItemContainer;
    [SerializeField] private GameObject itemSlotPrefab;

    [Header("아이템 정보 UI")]
    [SerializeField] private ItemInfoUI itemInfoUI;
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
    public RuneSlotUI SelectedEquipmentSlot { get; private set; }

    private void Awake()
    {
        equipButton.onClick.AddListener(EquipSelectedItem);
        unEquipButton.onClick.AddListener(UnequipSelectedSlot);

        CreateSlotPool();
    }

    // UI 슬롯을 생성
    private void CreateSlotPool()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject slotObj = Instantiate(runeSlotPrefab, runeSlotContainer);
            RuneSlotUI slotUI = slotObj.GetComponent<RuneSlotUI>();
            if (slotUI != null)
            {
                slotUI.OnSlotClicked += OnEquipmentSlotClicked;
                _runeSlotUIs.Add(slotUI);
                slotObj.SetActive(false);
            }
        }
    }

    public void Init(CharacterStatUI[] characterStatUIs, Party party)
    {
        IconCacheManager = GameManager.Instance.IconCacheManager;
        itemInfoUI.Init(IconCacheManager);

        foreach (CharacterStatUI statUI in characterStatUIs)
        {
            statUI.OnCharacterSelected += OnClickCharacterChange;
        }

        Party = party;
        PartyInventory = party.Inventory;
        inventoryPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (Party != null && Party.Members.Count > 0)
        {
            OnClickCharacterChange(Party.Members[0]);
            itemInfoUI.gameObject.SetActive(false);
        }
    }

    // 해당 슬롯의 UI만 업데이트
    private void OnCharacterEquipmentChanged(int slotIndex)
    {
        if (CurrentCharacter == null || slotIndex < 0 || slotIndex >= _runeSlotUIs.Count) return;

        RuneSlotUI slotUI = _runeSlotUIs[slotIndex];
        ItemInstance equippedRune = CurrentCharacter.RuneController.GetEquippedRune(slotIndex);
        Sprite icon = (equippedRune != null && !string.IsNullOrEmpty(equippedRune.BaseData.Icon))
            ? IconCacheManager.GetIcon(equippedRune.BaseData.Icon)
            : null;
        slotUI.UpdateSlot(equippedRune, icon);

        if (SelectedEquipmentSlot != null && SelectedEquipmentSlot.slotIndex == slotIndex)
        {
            OnEquipmentSlotClicked(slotUI);
        }
        else
        {
            inventoryPanel.SetActive(false);
        }
    }

    // 캐릭터 변경
    private void OnClickCharacterChange(CharacterInstance newCharacter)
    {
        if (!gameObject.activeInHierarchy) return;

        if (CurrentCharacter == newCharacter && _runeSlotUIs.All(s => s.gameObject.activeSelf)) return;

        if (CurrentCharacter != null)
        {
            CurrentCharacter.RuneController.OnRuneChanged -= OnCharacterEquipmentChanged;
        }

        CurrentCharacter = newCharacter;
        CurrentCharacter.RuneController.OnRuneChanged += OnCharacterEquipmentChanged;

        nameText.text = CurrentCharacter.Name;
        jobText.text = $"직업 : {CurrentCharacter.Job.JobType}";

        RefreshRuneSlots(); // 슬롯 정보 갱신

        inventoryPanel.SetActive(false);
        itemInfoUI.gameObject.SetActive(false);
    }

    private void RefreshRuneSlots()
    {
        if (CurrentCharacter == null) return;

        IReadOnlyList<Rune> characterRuneSlots = CurrentCharacter.RuneController.RuneSlots;

        for (int i = 0; i < _runeSlotUIs.Count; i++)
        {
            if (i < characterRuneSlots.Count)
            {
                RuneSlotUI slotUI = _runeSlotUIs[i];

                // 슬롯 갱신
                slotUI.SetSlot(i, characterRuneSlots[i].AbilityType);

                // UI 갱신
                ItemInstance equippedRune = CurrentCharacter.RuneController.GetEquippedRune(i);
                Sprite icon = (equippedRune != null && !string.IsNullOrEmpty(equippedRune.BaseData.Icon))
                    ? IconCacheManager.GetIcon(equippedRune.BaseData.Icon)
                    : null;
                slotUI.UpdateSlot(equippedRune, icon);

                // 슬롯 활성화
                slotUI.gameObject.SetActive(true);
            }
            else
            {
                _runeSlotUIs[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnEquipmentSlotClicked(RuneSlotUI selectedSlot)
    {
        SelectedEquipmentSlot = selectedSlot;
        SelectedInventoryItem = null;

        PopulateInventoryPanel(selectedSlot.abilityType);
        inventoryPanel.SetActive(true);

        ItemInstance equippedRune = CurrentCharacter.RuneController.GetEquippedRune(selectedSlot.slotIndex);
        if (equippedRune != null)
        {
            itemInfoUI.gameObject.SetActive(true);
            itemInfoUI.ShowItemInfo(equippedRune.ItemName(), equippedRune.BaseData.Description, equippedRune.BaseData.Icon);
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

    private void PopulateInventoryPanel(AbilityType abilityType)
    {
        foreach (Transform child in inventoryItemContainer)
        {
            Destroy(child.gameObject);
        }

        List<ItemInstance> itemsToDisplay = PartyInventory.GetItems(ItemType.Rune)
            .Where(item =>
                !item.IsEquipped &&
                item.RuneStat?.AbilityType == abilityType)
            .ToList();

        foreach (ItemInstance item in itemsToDisplay)
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

    private void OnInventoryItemSelected(ItemInstance selectedRune)
    {
        SelectedInventoryItem = selectedRune;
        itemInfoUI.gameObject.SetActive(true);
        itemInfoUI.ShowItemInfo(selectedRune.ItemName(), selectedRune.BaseData.Description, selectedRune.BaseData.Icon);
        equipButton.gameObject.SetActive(true);
        unEquipButton.gameObject.SetActive(false);
    }

    private void EquipSelectedItem()
    {
        if (SelectedInventoryItem != null && SelectedEquipmentSlot != null && CurrentCharacter != null)
        {
            CurrentCharacter.RuneController.Equip(SelectedEquipmentSlot.slotIndex, SelectedInventoryItem);
        }
    }

    private void UnequipSelectedSlot()
    {
        if (SelectedEquipmentSlot != null && CurrentCharacter != null)
        {
            CurrentCharacter.RuneController.Unequip(SelectedEquipmentSlot.slotIndex);
        }
    }

    private void OnDestroy()
    {
        if (CurrentCharacter != null)
        {
            CurrentCharacter.RuneController.OnRuneChanged -= OnCharacterEquipmentChanged;
        }
    }

    protected override MenuState GetMenuState()
    {
        return MenuState.Rune;
    }
}
