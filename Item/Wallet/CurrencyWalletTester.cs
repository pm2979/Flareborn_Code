using UnityEngine;

public class CurrencyWalletTester : MonoBehaviour
{
    public CurrencyWallet wallet;

    [SerializeField] private GoldUI goldUI;

    private void Start()
    {
        wallet = new CurrencyWallet(10000);

        if (goldUI != null)
        {
            goldUI.SetWallet(wallet);
        }

        Debug.Log($"[테스트] 초기 골드: {wallet.Gold}");
    }

    public void AddGold(int gold)
    {
        wallet.AddGold(gold);
    }

    public void SpendGold(int gold)
    {
        bool success = wallet.TrySpendGold(gold);
        if (success)
        {
            Debug.Log($"[테스트] 골드 -{gold} 성공");
        }
        else
        {
            Debug.Log($"[테스트] 골드 -{gold} 실패 (부족)");
        }
    }
}
