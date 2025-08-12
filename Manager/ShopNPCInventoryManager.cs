using System.Collections.Generic;
using Ink.Parsed;
using UnityEngine;

// 상점 NPC와의 다이얼로그에서 상점을 열게 되면 :
// ShopNPCInventoryManager의 currentShopNPCInventory에 해당 NPC의 인벤토리를 할당하여 Shop_NPCInventoryUI에서 사용할 수 있도록 한다.
public class ShopNPCInventoryManager : MonoSingleton<ShopNPCInventoryManager>
{
    private Dictionary<string, ShopNPCInventory> shopNPCInventories;

    private string currentShopNPCName;
    private ShopNPCInventory currentShopNPCInventory;

    // 프로퍼티
    public string CurrentShopNPCName
    {
        get => currentShopNPCName;
        set => currentShopNPCName = value;
    }
    public ShopNPCInventory CurrentShopNPCInventory
    {
        get => currentShopNPCInventory;
        set => currentShopNPCInventory = value;
    }

    protected override void OnEnable()
    {
        GameEventsManager.Instance.dialogueEvents.onOpenShop += OpenShop;
    }

    protected void Start()
    {
        Init();
    }

    private void OnDisable()
    {
        // #방어로직
        if (GameEventsManager.Instance == null) return;

        GameEventsManager.Instance.dialogueEvents.onOpenShop -= OpenShop;
        DeInit();
    }

    private void Init()
    {
        shopNPCInventories = new Dictionary<string, ShopNPCInventory>();
        ShopNPC[] shopNPCs = FindObjectsByType<ShopNPC>(FindObjectsSortMode.None);
        foreach (ShopNPC shopNPC in shopNPCs)
        {
            string npcName = shopNPC.NPCName;
            if (!shopNPCInventories.ContainsKey(npcName))
            {
                shopNPCInventories.Add(npcName, shopNPC.ShopNPCInventory);
            }

            Debug.Log($"ShopNPCInventoryManager: {npcName} added to playerInventory manager.");
        }
    }

    private void DeInit()
    {
        // 종료시 담겨있는 데이터 깔끔하게 초기화
        currentShopNPCInventory = null;
        shopNPCInventories.Clear();
    }

    public void OpenShop(string npcName)
    {
        // 현재 상점 NPC의 이름을 설정 && 인벤토리 할당
        currentShopNPCName = npcName;
        currentShopNPCInventory = GetShopInventory(npcName);
    }

    public ShopNPCInventory GetShopInventory(string npcName)
    {
        ShopNPCInventory inventory = null;
        if (shopNPCInventories.TryGetValue(npcName, out ShopNPCInventory requestedInventory))
        {
            inventory = requestedInventory;
        }

        return inventory;
    }
}
