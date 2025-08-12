using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DesignEnums;

public class EquipmentSlotUI : MonoBehaviour
{
    [Header("슬롯 타입")]
    [SerializeField] public ItemType slotType; // 인스펙터에서 슬롯 타입 설정

    [Header("UI 컴포넌트")]
    [SerializeField] private Image itemIcon; // 룬 아이콘을 표시할 이미지
    [SerializeField] private Button slotButton; // 슬롯 자체를 선택하는 버튼
    [SerializeField] private TextMeshProUGUI nameText;

    // 슬롯 클릭 이벤트를 외부에 알림
    public event Action<EquipmentSlotUI> OnSlotClicked;

    private void Awake()
    {
        // 버튼 클릭 시 이벤트가 발생하도록 리스너 연결
        slotButton.onClick.AddListener(() => OnSlotClicked?.Invoke(this));
    }

    // 아이템 정보 업데이트
    public void UpdateSlot(ItemInstance equippedItem, Sprite icon)
    {
        if (equippedItem != null)
        {
            itemIcon.sprite = icon;
            nameText.text = equippedItem.ItemName();
            itemIcon.gameObject.SetActive(true);
        }
        else
        {
            itemIcon.sprite = null;
            nameText.text = "";
            itemIcon.gameObject.SetActive(false);
        }
    }
}
