using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [Header("슬롯 UI 요소들")]
    [SerializeField] protected Image iconImage;
    [SerializeField] protected TextMeshProUGUI quantityText;
    [SerializeField] protected TextMeshProUGUI nameText;
    [SerializeField] protected TextMeshProUGUI equipText;


    [Header("기타 필요 컴포넌트")]
    public ItemInfoUI itemInfoUI; // 아이템 정보를 표시해주는 UI

    protected ItemInstance itemData; // 슬롯에 저장된 아이템 참조용
    protected string itemNameValue;
    protected int itemQuantityValue;
    protected string itemDescriptionValue;

    // 프로퍼티
    public ItemInstance ItemData => itemData;
    public string ItemNameValue => itemNameValue;
    public int ItemQuantityValue => itemQuantityValue;
    public string ItemDescriptionValue => itemDescriptionValue;
    public IconCacheManager IconCacheManager { get; protected set; }


    public virtual void SetSlot(ItemInstance data)
    {
        if (data == null || data.BaseData == null)
        {
            ClearSlot();
            return;
        }

        itemData = data;
        SetItemInfo(data);

        // 아이콘 설정
        if (iconImage != null)
        {
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
        }

        // 이름 설정
        if (nameText != null)
        {
            nameText.text = itemNameValue;
        }

        // 수량 설정 (1개 초과일 때만 보이도록)
        if (quantityText != null)
        {
            quantityText.text = itemQuantityValue.ToString();
        }

        // 장착 텍스트는 보상창에선 불필요하므로 비활성화
        if (equipText != null)
        {
            equipText.gameObject.SetActive(false);
        }
    }

    public void InitSlot(IconCacheManager cacheManager, ItemInfoUI infoUI = null)
    {
        IconCacheManager = cacheManager;
        itemInfoUI = infoUI;
    }

    public virtual void SetItemInfo(ItemInstance data)
    {
        itemNameValue = data.ItemName();
        itemQuantityValue = data.CurrentStack;
        itemDescriptionValue = data.BaseData.Description;
    }

    public virtual void OnClickSlot()
    {
        if (IsEmpty()) return;
        if(itemInfoUI == null) return;

        itemInfoUI.ShowItemInfo(itemNameValue, itemDescriptionValue, ItemData.BaseData.Icon);
    }

    public virtual void ClearSlot()
    {
        itemData = null;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.gameObject.SetActive(false);
        }

        if (nameText != null)
        {
            nameText.text = "";
        }

        if (quantityText != null)
        {
            quantityText.text = "";
            quantityText.gameObject.SetActive(false);
        }
    }

    public virtual bool IsEmpty()
    {
        return ItemData == null;
    }
}