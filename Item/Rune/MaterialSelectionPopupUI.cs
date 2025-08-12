using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static DesignEnums;

public class MaterialSelectionPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Transform contentParent; // 아이템 버튼이 생성될 부모 Transform
    [SerializeField] private GameObject itemButtonPrefab; // 아이템 선택 버튼 프리팹

    public Action<ItemInstance> OnMaterialSelected;

    private List<GameObject> spawnedButtons = new List<GameObject>();

    public Inventory Inventory { get; protected set; }
    public IconCacheManager IconCacheManager { get; protected set; }

    private void Start()
    {
        popupPanel.SetActive(false);
        Inventory = GameManager.Instance.PartyManager.Party.Inventory;
        IconCacheManager = GameManager.Instance.IconCacheManager;
    }

    // 팝업을 띄우고 인벤토리의 코어 아이템 목록을 표시
    public void Show(List<ItemInstance> excludeItems)
    {
        popupPanel.SetActive(true);

        // 이전 버튼들 삭제
        foreach (GameObject button in spawnedButtons)
        {
            Destroy(button);
        }

        spawnedButtons.Clear();

        // 이미 슬롯에 추가된 코어 아이템들의 키와 개수를 계산
        Dictionary<int, int> excludedCounts = excludeItems.Where(IsCoreItem).GroupBy(item => item.ItemKey).ToDictionary(g => g.Key, g => g.Count());

        // 인벤토리에서 중복되지 않는 코어 아이템 목록
        IEnumerable<ItemInstance> coreItemStacks = Inventory.GetItems(ItemType.General)
            .Where(item => IsCoreItem(item))
            .GroupBy(item => item.ItemKey)
            .Select(g => g.First());

        // 각 코어 아이템에 대해 버튼을 생성할지 결정
        foreach (ItemInstance itemStack in coreItemStacks)
        {
            excludedCounts.TryGetValue(itemStack.ItemKey, out int selectedCount);

            // 보유 수량이 이미 선택된 수량보다 많을 경우에만 선택 목록에 표시
            if (itemStack.CurrentStack > selectedCount)
            {
                GameObject buttonObj = Instantiate(itemButtonPrefab, contentParent);
                spawnedButtons.Add(buttonObj);

                InventoryItemSlotUI slot = buttonObj.GetComponent<InventoryItemSlotUI>();
                slot.InitSlot(IconCacheManager);
                slot.SetSlot(itemStack);

                Button button = buttonObj.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    // 버튼 클릭 시 선택된 아이템 정보 전달 후 팝업 닫기
                    OnMaterialSelected?.Invoke(itemStack);
                    popupPanel.SetActive(false);
                });
            }
        }
    }

    // 코어인지 판별
    private bool IsCoreItem(ItemInstance item)
    {
        return item.ItemKey >= 4001 && item.ItemKey <= 4003;
    }
}
