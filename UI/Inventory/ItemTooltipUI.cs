using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipUI : MonoBehaviour
{
    // 아이템 설명 텍스트
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private RectTransform rectTransform;

    // 툴팁을 표시하는 메서드
    public void ShowTooltip(string description)
    {
        descriptionText.text = description;
        gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform); // 툴팁 크기 조정
    }

    // 툴팁을 숨기는 메서드
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}

