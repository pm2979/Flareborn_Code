using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField] ShopNPCInventoryManager shopNPCInventoryManager;

    private void Awake()
    {
        // 이 오브젝트가 파괴되지 않도록 설정
        DontDestroyOnLoad(gameObject);

        shopNPCInventoryManager = GetComponentInChildren<ShopNPCInventoryManager>();
    }
}
