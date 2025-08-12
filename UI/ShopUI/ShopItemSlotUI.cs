using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

// 인벤토리 슬롯 UI 하나를 관리하는 스크립트
public class ShopItemSlotUI : ItemSlotUI
{
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Shop_BuyButton buyButton; // 구매 버튼
    [SerializeField] private Shop_SellButton sellButton; // 판매 버튼

    [Header("슬롯 타입")]
    [SerializeField] private ShopSlotType slotType; // 슬롯 타입 (구매용, 판매용)
    private enum ShopSlotType { Buy, Sell } // 슬롯 타입 (구매용, 판매용)

    private int itemPriceValue;

    // 프로퍼티 
    public int ItemPriceValue => itemPriceValue; // 아이템 가격 프로퍼티

    public void ShopItemSlotInit(Shop_BuyButton buyButton, Shop_SellButton sellButton)
    {
        if (slotType == ShopSlotType.Buy)
        {
            this.buyButton = buyButton;
        }
        else if (slotType == ShopSlotType.Sell)
        {
            this.sellButton = sellButton;
        }
    }

    public override void SetSlot(ItemInstance data)
    {
        itemData = data;

        if (data == null)
        {
            ClearSlot();
            return;
        }

        nameText.text = data.BaseData.Name;
        quantityText.text = data.CurrentStack.ToString();
        priceText.text = (slotType == ShopSlotType.Buy ? data.BaseData.CostPrice : data.BaseData.SalePrice).ToString("N0");
        SetItemInfo(data);

        string iconAddress = data.BaseData.Icon;
        if (!string.IsNullOrEmpty(iconAddress))
        {
            iconImage.gameObject.SetActive(true);

            Sprite icon = IconCacheManager.GetIcon(iconAddress);
            if (icon != null)
            {
                iconImage.sprite = icon;
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
    }

    public override void SetItemInfo(ItemInstance data)
    {
        base.SetItemInfo(data);
        itemPriceValue = GetItemPrice(data);
    }


    public override void OnClickSlot()
    {
        if (IsEmpty()) return;

        base.OnClickSlot();
        BuyOrSellItem(itemData, itemPriceValue);
        ActivateButton();
    }

    public void ActivateButton()
    {
        if (slotType == ShopSlotType.Buy)
        {
            buyButton.gameObject.SetActive(true);
        }
        else if (slotType == ShopSlotType.Sell)
        {
            sellButton.gameObject.SetActive(true);
        }
    }

    public void DeActivateButton()
    {
        if (slotType == ShopSlotType.Buy)
        {
            buyButton.gameObject.SetActive(false);
        }
        else if (slotType == ShopSlotType.Sell)
        {
            sellButton.gameObject.SetActive(false);
        }
    }

    private void BuyOrSellItem(ItemInstance newItemData, int newPriceValue)
    {
        if (slotType == ShopSlotType.Buy)
        {
            buyButton.SetSelectedItemData(newItemData, newPriceValue);
        }
        else if (slotType == ShopSlotType.Sell)
        {
            sellButton.SetSelectedItemData(newItemData, newPriceValue);
        }
    }

    private int GetItemPrice(ItemInstance data)
    {
        return slotType == ShopSlotType.Buy ? data.BaseData.CostPrice : data.BaseData.SalePrice;
    }

    public override void ClearSlot()
    {
        base.ClearSlot();
        priceText.text = "";
    }
}