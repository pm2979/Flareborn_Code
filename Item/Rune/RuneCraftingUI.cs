using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class RuneCraftingUI : MonoBehaviour
{
    public RuneCrafting RuneCrafter { get; private set; }


    [Header("실시간 확률 표시")]
    [SerializeField] private TextMeshProUGUI epicChanceText;
    [SerializeField] private TextMeshProUGUI rareChanceText;
    [SerializeField] private TextMeshProUGUI normalChanceText;

    [Header("연결 필요")]
    [SerializeField] private RuneCrafting runeCrafter; // 테스트 용
    [SerializeField] private MaterialSelectionPopup materialPopup; // 재료 선택 팝업 UI

    [Header("재료 슬롯")]
    [SerializeField] private CraftingSlotUI[] materialSlots;

    [Header("제작 버튼")]
    [SerializeField] private Button craftButton;

    [Header("완성 UI")]
    [SerializeField] private Image finishIconImg;
    [SerializeField] private TextMeshProUGUI finishNameText;

    [Header("기타 등등")]
    [SerializeField] private GameObject contentParent;

    // 현재 재료를 추가할 대상 슬롯의 인덱스
    private int currentTargetSlotIndex = -1;

    // UI상 제작 슬롯에 올라간 아이템들을 관리
    private List<ItemInstance> materialsForCrafting = new List<ItemInstance>();
    public IconCacheManager IconCacheManager { get; protected set; }

    private void OnEnable()
    {
        GameEventsManager.Instance.miscEvents.onEnableRunePanel += OnEnableRunePanel;
    }

    private void Start()
    {
        IconCacheManager = GameManager.Instance.IconCacheManager;

        // 시작할때는 컨텐츠 패널을 비활성화 
        contentParent.SetActive(false);

        // 각 슬롯 버튼에 클릭 이벤트 연결
        for (int i = 0; i < materialSlots.Length; i++)
        {
            int index = i; // 클로저 문제 방지
            materialSlots[i].GetSlotButton().onClick.AddListener(() => OnMaterialSlotClicked(index));
            materialSlots[i].InitSlot(IconCacheManager);
        }

        // 제작 버튼 클릭 이벤트 연결
        craftButton.onClick.AddListener(OnExecuteCraft);

        // 팝업이 닫힐 때 실행될 콜백 함수 등록
        materialPopup.OnMaterialSelected += AddMaterialToSlot;

        UpdateCraftButtonState();

        finishIconImg.gameObject.SetActive(false);
        finishNameText.text = "";
        UpdatePreview();
    }

    private void OnDisable()
    {
        // # 방어로직 
        if (GameEventsManager.Instance != null)
        {
            GameEventsManager.Instance.miscEvents.onEnableRunePanel -= OnEnableRunePanel;
        }
    }

    // 재료 슬롯을 클릭했을 때 호출
    private void OnMaterialSlotClicked(int slotIndex)
    {
        if (materialSlots[slotIndex].HasItem())
        {
            materialSlots[slotIndex].ClearSlot();
            UpdateCraftButtonState();
            UpdatePreview();
        }
        else
        {
            currentTargetSlotIndex = slotIndex;

            List<ItemInstance> usedItems = new List<ItemInstance>();
            foreach (CraftingSlotUI slot in materialSlots)
            {
                if (slot.HasItem()) usedItems.Add(slot.GetItem());
            }
            materialPopup.Show(usedItems);
        }

        finishIconImg.gameObject.SetActive(false);
        finishNameText.text = "";
    }

    // 재료가 선택되었을 때 호출될 콜백 메서드
    private void AddMaterialToSlot(ItemInstance selectedItem)
    {
        if (currentTargetSlotIndex != -1)
        {
            materialSlots[currentTargetSlotIndex].SetItem(selectedItem);
            currentTargetSlotIndex = -1; // 타겟 슬롯 초기화
            UpdateCraftButtonState();
            UpdatePreview();
        }
    }

    // 제작 버튼 상태 업데이트
    private void UpdateCraftButtonState()
    {
        int itemCount = 0;
        foreach (CraftingSlotUI slot in materialSlots)
        {
            if (slot.HasItem()) itemCount++;
        }
        craftButton.interactable = (itemCount == 3);
    }

    // 제작 버튼 최종 실행
    private void OnExecuteCraft()
    {
        List<ItemInstance> materials = new List<ItemInstance>();

        foreach (CraftingSlotUI slot in materialSlots)
        {
            if (slot.HasItem()) materials.Add(slot.GetItem());
        }

        if (materials.Count == 3)
        {
            ItemInstance result = runeCrafter.CraftRune(materials);
            if (result != null)
            {
                // 제작 성공 시 모든 슬롯 비우기
                foreach (CraftingSlotUI slot in materialSlots) slot.ClearSlot();
                UpdateCraftButtonState();
                UpdatePreview();

                string iconAddress = result.BaseData.Icon;
                if (!string.IsNullOrEmpty(iconAddress))
                {
                    Sprite icon = IconCacheManager.GetIcon(iconAddress);
                    if (icon != null)
                    {
                        finishIconImg.sprite = icon;
                        finishIconImg.gameObject.SetActive(true);
                    }
                    else
                    {
                        finishIconImg.gameObject.SetActive(false);
                    }
                }
                else
                {
                    finishIconImg.gameObject.SetActive(false);
                }

                finishNameText.text = result.ItemName();
                finishIconImg.gameObject.SetActive(true);
            }
        }
    }

    private void UpdatePreview()
    {
        materialsForCrafting.Clear();
        foreach (CraftingSlotUI slot in materialSlots)
        {
            if (slot.HasItem())
            {
                materialsForCrafting.Add(slot.GetItem());
            }
        }

        if (materialsForCrafting.Count == 3)
        {
            (int epicChance, int rareChance) = runeCrafter.PreviewProbabilities(materialsForCrafting);
            UpdateProbabilityUI(epicChance, rareChance);
        }
        else
        {
            UpdateProbabilityUI(0, 0);
        }
    }

    private void UpdateProbabilityUI(int epic, int rare)
    {
        if (epicChanceText != null) epicChanceText.text = $"에픽: {epic}%";
        if (rareChanceText != null) rareChanceText.text = $"레어: {rare}%";
        if (normalChanceText != null)
        {
            int normal = Mathf.Max(0, 100 - epic - rare);
            normalChanceText.text = $"노멀: {normal}%";
        }

        if(epic == 0 && rare == 0)
        {
            normalChanceText.text = $"노멀: 0%";
        }
    }


    public void OnEnableRunePanel()
    {
        // 룬 제작 UI 활성화
        contentParent.SetActive(true);
    }

    public void OnExitButton()
    {
        // 룬 제작 UI 비활성화
        contentParent.SetActive(false);
    }
}
