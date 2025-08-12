using UnityEngine;

public class ShopNPC : MonoBehaviour
{
    [SerializeField] protected string npcName;
    [SerializeField] protected ShopNPCInventory shopNPCInventory;

    // 프로퍼티
    public string NPCName { get => npcName; set => npcName = value; }
    public ShopNPCInventory ShopNPCInventory { get => shopNPCInventory; set => shopNPCInventory = value; }

    protected void Awake()
    {
        shopNPCInventory = new ShopNPCInventory();
    }

    protected void Start()
    {
        ShopInit();
    }

    protected virtual void ShopInit()
    {
        // 상점에서 판매하고싶은 아이템들을 추가
    }

    protected void OnValidate()
    {
#if UNITY_EDITOR
        NPCName = gameObject.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
