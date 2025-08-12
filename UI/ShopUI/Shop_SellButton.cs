using UnityEngine;

public class Shop_SellButton : Shop_Buttons
{
    public void OnClick()
    {
        // 상점 내에서 판매버튼 클릭시 작동하는 로직 구현

        // 방어로직 : 아이템 데이터가 null이거나 가격이 0 이하인 경우
        if (itemData == null || priceValue <= 0) return;

        // 화폐 지갑에 아이템 가격만큼 더함
        currencyWallet.AddGold(priceValue);

        // 플레이어 아이템을 인벤토리에서 1개 제거 && 상점 NPC 인벤토리에 아이템 추가
        playerInventory.RemoveItem(itemData, 1);
        shopNPCInventory.AddItem(itemData, 1);

        // 아이템 인스턴스의 스택 수를 체크
        if (!playerInventory.DoesItemExist(itemData))
        {
            itemData = null;

            // 아이템이 더 이상 없으면 버튼 비활성화
            this.gameObject.SetActive(false);
            Debug.Log($"아이템 {itemData}을 모두 구매하였습니다. 버튼을 비활성화합니다.");
        }
    }
}
