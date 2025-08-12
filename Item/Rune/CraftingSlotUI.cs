using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlotUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button slotButton;

    private ItemInstance currentItem;

    public IconCacheManager IconCacheManager { get; protected set; }

    private void Awake()
    {
        ClearSlot();
    }

    public void InitSlot(IconCacheManager iconCacheManager)
    {
        IconCacheManager = iconCacheManager;
    }

    public void SetItem(ItemInstance item)
    {
        currentItem = item;

        nameText.text = item.ItemName();

        string iconAddress = item.BaseData.Icon;
        if (!string.IsNullOrEmpty(iconAddress))
        {
            Sprite icon = IconCacheManager.GetIcon(iconAddress);
            if (icon != null)
            {
                itemIcon.sprite = icon;
                itemIcon.gameObject.SetActive(true);
            }
            else
            {
                itemIcon.gameObject.SetActive(false);
            }
        }
        else
        {
            itemIcon.gameObject.SetActive(false);
        }

        itemIcon.color = Color.white; // 아이콘 표시
    }

    public void ClearSlot()
    {
        nameText.text = "";
        currentItem = null;
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0); // 아이콘 숨김
    }

    public bool HasItem() => currentItem != null;
    public ItemInstance GetItem() => currentItem;
    public Button GetSlotButton() => slotButton;
}
