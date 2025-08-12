using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TargetSelectUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;   // 버튼들이 들어갈 곳
    [SerializeField] private GameObject buttonPrefab;   // TextMeshProUGUI 가 붙은 버튼 프리팹
    [SerializeField] private Button cancelButton;       // 취소 버튼

    // members: 파티원 리스트
    // onSelected: 특정 멤버 클릭 시 호출
    // onCancelled: 취소 클릭 시 호출
    public void Setup(
        IReadOnlyList<CharacterInstance> members,
        System.Action<CharacterInstance> onSelected,
        System.Action onCancelled
    )
    {
        // 멤버별 버튼 생성
        foreach (var mem in members)
        {
            var go = Instantiate(buttonPrefab, contentParent);
            var txt = go.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = mem.Name;
            var btn = go.GetComponent<Button>();
            btn.onClick.AddListener(() => {
                onSelected?.Invoke(mem);
                Destroy(gameObject);
            });
        }

        // 취소 버튼
        cancelButton.onClick.AddListener(() => {
            onCancelled?.Invoke();
            Destroy(gameObject);
        });
    }
}
