using UnityEngine;
using UnityEngine.EventSystems;
using static DesignEnums;

// 인벤토리 슬롯 UI 하나를 관리하는 스크립트
public class InventoryItemSlotUI : ItemSlotUI, IPointerClickHandler
{
    private ConfirmUse confirmUse; // 인스펙터에는 표시 안 함, 코드로만 설정

    public void SetConfirmUse(ConfirmUse confirmUseUI)
    {
        confirmUse = confirmUseUI;
    }

    public override void SetSlot(ItemInstance data)
    {
        itemData = data;
        if (data == null)
        {
            ClearSlot();
            return;
        }

        SetItemInfo(data);

        nameText.text = data.ItemName();

        if(quantityText != null)
        {
            quantityText.text = data.CurrentStack.ToString();
        }

        string iconAddress = data.BaseData.Icon;
        if (!string.IsNullOrEmpty(iconAddress))
        {
            Sprite icon = IconCacheManager.GetIcon(iconAddress);
            if (icon != null)
            {
                iconImage.sprite = icon;
                iconImage.gameObject.SetActive(true);
            }
            else
            {
                iconImage.gameObject.SetActive(false);
            }
        }
        else
        {
            iconImage.gameObject.SetActive(false);
        }

        if(equipText != null)
        {
            if (data.IsEquipped)
            {
                equipText.gameObject.SetActive(true);
            }
            else
            {
                equipText.gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEmpty() || eventData.button != PointerEventData.InputButton.Right)
            return;

        if (ItemData.BaseData.ItemType != ItemType.Consumable)
            return;

        if (confirmUse != null)
        {
            confirmUse.HideAll();
            confirmUse.Open(this);
        }
        else
        {
            Debug.LogWarning("[InventoryItemSlotUI] ConfirmUse 참조가 설정되지 않았습니다.");
        }
    }
}