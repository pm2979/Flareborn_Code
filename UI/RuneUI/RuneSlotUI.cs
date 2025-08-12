using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DesignEnums;

public class RuneSlotUI : MonoBehaviour
{
    [Header("슬롯 정보")]
    [Tooltip("능력치 타입")]
    public AbilityType abilityType; // 인스펙터에서 타입 지정
    public int slotIndex;

    [Header("UI 컴포넌트")]
    [SerializeField] private Image itemIcon; // 룬 아이콘을 표시할 이미지
    [SerializeField] private TextMeshProUGUI runeName;
    [SerializeField] private TextMeshProUGUI slotName;
    [SerializeField] private Button slotButton; // 슬롯 버튼

    public string ItemName { get; private set; }

    public event Action<RuneSlotUI> OnSlotClicked;

    private void Awake()
    {
        slotButton?.onClick.AddListener(() => OnSlotClicked?.Invoke(this));
    }

    // 슬롯의 UI를 업데이트
    public void UpdateSlot(ItemInstance runeItem, Sprite icon)
    {
        if (runeItem != null && runeItem.BaseData != null)
        {
            itemIcon.sprite = icon;
            runeName.text = runeItem.ItemName();
            itemIcon.gameObject.SetActive(true);
        }
        else
        {
            itemIcon.sprite = null;
            runeName.text = "";
            itemIcon.gameObject.SetActive(false);
        }
    }

    public void SetSlot(int slotIndex, AbilityType abilityType)
    {
        this.abilityType = abilityType;
        this.slotIndex = slotIndex;
        slotName.text = abilityType.ToString();
    }
}
