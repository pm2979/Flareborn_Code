using UnityEngine;

public class Shop_Buttons : MonoBehaviour
{
    [Header("필요 컴포넌트")]
    protected QuestStepEvents questStepEvents; // 퀘스트 이벤트 참조용
    protected Inventory shopNPCInventory; // 상점 NPC의 인벤토리 참조용
    protected Inventory playerInventory; // 인벤토리 참조용
    protected CurrencyWallet currencyWallet; // 화폐 지갑 참조용
    protected bool isInitialized = false; // 초기화 여부

    [Header("아이템 데이터")]
    protected ItemInstance itemData; // 구매할 아이템 데이터
    protected int priceValue; // 아이템 가격

    protected virtual void OnEnable() 
    {
        questStepEvents = GameEventsManager.Instance.questStepEvents;
    }

    // 자식 클래스에서 이벤트 구독을 위해 사용할 수 있도록 virtual로 선언
    protected virtual void OnDisable() { }

    public void HideButton()
    {
        // 버튼을 숨김
        this.gameObject.SetActive(false);
    }

    public void Init(Inventory playerInventoryObject, Inventory shopNPCInventoryObject, CurrencyWallet currencyWalletObject)
    {
        // 방어로직 : 이미 초기화가 되어 있다면 다시 초기화하지 않음
        if (isInitialized) return;

        shopNPCInventory = shopNPCInventoryObject;
        playerInventory = playerInventoryObject;
        currencyWallet = currencyWalletObject;
        isInitialized = true;
    }

    public void DeInit()
    {
        isInitialized = false;
        shopNPCInventory = null;
        playerInventory = null;
        currencyWallet = null;
    }

    public void SetSelectedItemData(ItemInstance newitemData, int newPriceValue)
    {
        itemData = newitemData;
        priceValue = newPriceValue;
    }
}
