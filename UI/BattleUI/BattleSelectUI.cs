using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleSelectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("버튼 UI")]
    public Button button;
    public TextMeshProUGUI nameTxt;

    [Header("턴 UI")]
    public GameObject turnUI;
    public TextMeshProUGUI remainingTurns;

    [Header("설명 UI")]
    public RectTransform descriptionUI;
    public TextMeshProUGUI descriptionTxt;

    public void Set(string _skillName = null, int _remainingTurns = 0, string _description = null)
    {
        if(_skillName != null)
        nameTxt.text = _skillName;
        
        if(_remainingTurns <= 0)
        {
            turnUI.SetActive(false);
        }
        else if (_remainingTurns > 0)
        {
            remainingTurns.text = _remainingTurns.ToString();
            turnUI.SetActive(true);
        }

        if(!string.IsNullOrWhiteSpace(_description)) 
        {
            descriptionTxt.text = _description;
            descriptionUI.gameObject.SetActive(false);
        }
        else
        {
            descriptionUI.gameObject.SetActive(false);
        }
    }

    // 마우스를 슬롯 위에 올렸을 때 호출되는 메서드
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!string.IsNullOrWhiteSpace(descriptionTxt.text))
        {
            descriptionUI.gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(descriptionUI);
        }
    }

    // 마우스가 슬롯에서 벗어났을 때 호출되는 메서드
    public void OnPointerExit(PointerEventData eventData)
    {
        if(descriptionUI.gameObject.activeSelf)
        {
            descriptionUI.gameObject.SetActive(false);
        }
    }

}
