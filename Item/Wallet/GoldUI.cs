using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI goldText;

    private CurrencyWallet wallet;
    public CurrencyWallet Wallet => wallet;

    private void OnEnable()
    {
        if (wallet != null)
        {
            wallet.OnGoldChanged += UpdateGoldText;
        }
    }


    // wallet 설정 메서드
    public void SetWallet(CurrencyWallet newWallet)
    {
        if (wallet != null)
        {
            wallet.OnGoldChanged -= UpdateGoldText;
        }

        wallet = newWallet;

        if (wallet != null)
        {
            wallet.OnGoldChanged += UpdateGoldText;
            UpdateGoldText(wallet.Gold);
        }
    }

    private void OnDisable()
    {
        if (wallet != null)
        {
            wallet.OnGoldChanged -= UpdateGoldText;
        }
    }

    private void UpdateGoldText(int currentGold)
    {
        goldText.text = currentGold.ToString("N0");
    }
}
