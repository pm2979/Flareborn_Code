using System;
using UnityEngine;

// 플레이어의 골드를 관리하는 클래스
[Serializable]
public class CurrencyWallet
{
    // 현재 골드 보유량
    [field: SerializeField] public int Gold { get; private set; }

    // 골드 변화 시 호출되는 이벤트
    public event Action<int> OnGoldChanged;

    public CurrencyWallet(int initialGold = 0)
    {
        Gold = initialGold;
    }

    // 골드 추가
    public void AddGold(int amount)
    {
        Gold += amount;

        OnGoldChanged?.Invoke(Gold);
    }

    // 골드 사용
    public bool TrySpendGold(int amount)
    {
        if (Gold < amount)
        {
            Logger.Log("골드가 부족합니다.");
            return false;
        }

        Gold -= amount;
        OnGoldChanged?.Invoke(Gold);
        return true;
    }
}
