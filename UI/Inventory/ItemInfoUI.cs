using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour
{
    [Header("UI 컴포넌트")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI itemNameTxt;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public IconCacheManager IconCacheManager { get; protected set; }

    public void Init(IconCacheManager cacheManager)
    {
        IconCacheManager = cacheManager;
    }

    private void OnEnable()
    {
        // 아이템 정보 UI가 활성화될 때 초기화 작업
        ClearItemInfo();
    }

    // 아이템 정보를 UI에 표시하는 메서드
    public void ShowItemInfo(string itemName, string description, string iconAddress)
    {
        itemNameTxt.text = itemName;
        descriptionText.text = description;

        if (!string.IsNullOrEmpty(iconAddress))
        {
            Sprite icon = IconCacheManager.GetIcon(iconAddress);
            if (icon != null)
            {
                iconImage.sprite = icon;
                iconImage.gameObject.SetActive(true);
            }
            else
            {
                iconImage.gameObject.SetActive(false);
            }
        }
        else
        {
            iconImage.gameObject.SetActive(false);
        }
    }

    // 아이콘 로딩이 완료되었을 때 호출될 콜백 함수
    private void OnIconLoaded(AsyncOperationHandle<Sprite> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            iconImage.sprite = handle.Result;
        }
        else
        {
            Logger.LogError($"[ItemInfoUI] 아이콘 로드 실패: {handle.DebugName}");
            iconImage.gameObject.SetActive(false);
        }
    }

    // 아이템 정보 UI를 초기화
    public void ClearItemInfo()
    {
        itemNameTxt.text = string.Empty;
        descriptionText.text = string.Empty;

        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.gameObject.SetActive(false);
        }
    }

    // 메모리 누수 방지
    private void OnDisable()
    {
        ClearItemInfo();
    }
}
