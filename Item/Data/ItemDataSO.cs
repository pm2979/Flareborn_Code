using UnityEngine;
using static DesignEnums;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemDataSO : ScriptableObject
{
    [Header("Item Info")]
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public ItemType itemType;

    [Header("Item Configuration")]
    public int MaxStack;
    public int SellPrice;

    private void OnValidate()
    {
#if UNITY_EDITOR
        itemName = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

}
