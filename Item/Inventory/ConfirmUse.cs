using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmUse : MonoBehaviour
{
    [Header("사용/취소 버튼 패널")]
    [SerializeField] private GameObject confirmButtons;
    [SerializeField] private Button useButton;
    [SerializeField] private Button cancelConfirmButton;

    [Header("멤버 선택 패널")]
    [SerializeField] private GameObject memberSelectPanel;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private Button memberButtonPrefab;
    [SerializeField] private Button cancelMemberButtonPrefab;
    [SerializeField] private Button useAllButtonPrefab;

    [SerializeField] private AlertPopup alertPopup;

    private ItemInstance pendingItem;
    private ItemSlotUI sourceSlot;

    public void Initialize()
    {
        useButton.onClick.AddListener(OnUseClicked);
        cancelConfirmButton.onClick.AddListener(HideAll);
        HideAll();
    }

    public void Open(ItemSlotUI slot)
    {
        HideAll();
        if (slot == null || slot.IsEmpty()) return;

        pendingItem = slot.ItemData;
        sourceSlot = slot;

        PositionUsePanelNearSlot(slot.transform);
        confirmButtons.SetActive(true);
    }

    private void OnUseClicked()
    {
        Debug.Log("사용 버튼 클릭됨"); // 로그 찍히는지 확인

        confirmButtons.SetActive(false);

        var effect = new ConsumableEffectsLoader().GetByKey(pendingItem.ItemKey);
        if (effect == null)
        {
            Debug.LogWarning($"[ConfirmUse] 효과를 찾을 수 없음: ID = {pendingItem.ItemKey}");
            return;
        }

        switch (effect.TargetType)
        {
            case DesignEnums.TargetType.AllyAll:
                ApplyEffectToAll(effect);
                break;

            case DesignEnums.TargetType.AllySingle:
                ShowMemberSelection(effect);
                break;

            default:
                Debug.LogWarning($"[ConfirmUse] 지원되지 않는 TargetType: {effect.TargetType}");
                break;
        }
    }

    private void ShowMemberSelection(ConsumableEffects effect)
    {
        memberSelectPanel.SetActive(true);
        memberSelectPanel.transform.position = confirmButtons.transform.position;

        foreach (Transform child in buttonContainer)
            Destroy(child.gameObject);

        var members = GameManager.Instance.PartyManager.GetCurrentParty();

        foreach (var member in members)
        {
            var captured = member;
            var btn = Instantiate(memberButtonPrefab, buttonContainer);
            var label = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (label != null)
                label.text = captured.Name;

            btn.onClick.AddListener(() =>
            {
                ApplyEffectToMember(captured, effect);
                GameManager.Instance.PartyManager.Party.Inventory.RemoveItem(pendingItem, 1);
                HideAll();
            });

            if (captured.CurrentHp >= captured.MaxHp)
                btn.interactable = false;
        }

        var useAllBtn = Instantiate(useAllButtonPrefab, buttonContainer);
        var useAllLabel = useAllBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (useAllLabel != null)
            useAllLabel.text = "모두";

        useAllBtn.onClick.AddListener(() =>
        {
            var validMembers = members.Where(m => m.CurrentHp < m.MaxHp).ToList();

            if (validMembers.Count == 0)
            {
                alertPopup.Show("모든 파티원의 체력이 가득합니다.");
                HideAll();
                return;
            }

            if (pendingItem.CurrentStack < validMembers.Count)
            {
                alertPopup.Show("아이템 수량이 부족합니다.");
                HideAll();
                return;
            }

            foreach (var member in validMembers)
                ApplyEffectToMember(member, effect);

            GameManager.Instance.PartyManager.Party.Inventory.RemoveItem(pendingItem, validMembers.Count);
            HideAll();
        });

        var cancelBtn = Instantiate(cancelMemberButtonPrefab, buttonContainer);
        var cancelLabel = cancelBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (cancelLabel != null)
            cancelLabel.text = "취소";

        cancelBtn.onClick.AddListener(HideAll);
    }

    private void ApplyEffectToMember(CharacterInstance member, ConsumableEffects effect)
    {
        int recovered = effect.Amount;
        int newHp = Mathf.Min(member.CurrentHp + recovered, member.MaxHp);
        member.SetCurrentHp(newHp);

        foreach (var ui in FindObjectsByType<CharacterStatUI>(FindObjectsSortMode.None))
        {
            if (ui.Character == member)
            {
                ui.UpdateUI();
                break;
            }
        }

        Debug.Log($"[ConfirmUse] {member.Name} 회복됨 +{recovered} → {member.CurrentHp}/{member.MaxHp}");
    }

    private void ApplyEffectToAll(ConsumableEffects effect)
    {
        var members = GameManager.Instance.PartyManager.GetCurrentParty();

        foreach (var member in members)
            ApplyEffectToMember(member, effect);

        GameManager.Instance.PartyManager.Party.Inventory.RemoveItem(pendingItem, members.Count);
        HideAll();
    }

    private void PositionUsePanelNearSlot(Transform slotTransform)
    {
        RectTransform slotRect = slotTransform as RectTransform;
        RectTransform panelRect = confirmButtons.GetComponent<RectTransform>();

        if (slotRect == null || panelRect == null)
        {
            Debug.LogWarning("[ConfirmUse] RectTransform 변환 실패");
            return;
        }

        panelRect.anchoredPosition = slotRect.anchoredPosition + new Vector2(-60f, 230f);
    }

    public void HideAll()
    {
        Debug.Log("취소 또는 완료 → HideAll 호출");
        confirmButtons.SetActive(false);
        memberSelectPanel.SetActive(false);
        pendingItem = null;
        sourceSlot = null;
    }
}
