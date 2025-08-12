using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DialogueChoiceButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [Header("Componenet")]
    [SerializeField] private GameObject pressE;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI choiceText;
    [SerializeField] private Color baseColor;

    private int choiceIndex = -1;

    private void OnEnable()
    {
        baseColor = new Color(41f / 255f, 18f / 255f, 18f / 255f);

        // 버튼이 활성화되면 E키를 누르라는 이미지를 숨김
        pressE.SetActive(false);
    }
    private void OnDisable()
    {
        choiceText.color = baseColor;
    }

    public void SetChoiceText(string choiceTextString)
    {
        choiceText.text = choiceTextString;
    }

    public void SetChoiceIndex(int index)
    {
        this.choiceIndex = index;
    }

    public void SelectButton()
    {
        button.Select();
    }

    // 버튼 클릭 등록용
    public void OnClick()
    {
        GameEventsManager.Instance.dialogueEvents.UpdateChoiceIndexEvent(choiceIndex);
        ChangeVisualState();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameEventsManager.Instance.dialogueEvents.UpdateChoiceIndexEvent(choiceIndex);
        ChangeVisualState();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ResetVisualState();
    }

    public void ChangeVisualState()
    {
        // 선택된 상태로 변경
        choiceText.color = Color.white;
        // E키를 누르라는 이미지를 표시
        pressE.SetActive(true);
    }

    public void ResetVisualState()
    {
        // 선택 해제 상태로 변경
        choiceText.color = baseColor;

        // E키를 누르라는 이미지를 숨김
        pressE.SetActive(false);
    }


}
