using UnityEngine;
using UnityEngine.UI;

public class ContextMenuUI : MonoBehaviour
{
    [SerializeField] public Button useButton;
    [SerializeField] public Button cancelButton;

    System.Action onUse;
    System.Action onCancel;

    // 버튼 콜백 연결
    public void Setup(System.Action onUse, System.Action onCancel)
    {
        this.onUse = onUse;
        this.onCancel = onCancel;

        useButton.onClick.AddListener(() => {
            onUse?.Invoke();
            Destroy(gameObject);
        });
        cancelButton.onClick.AddListener(() => {
            onCancel?.Invoke();
            Destroy(gameObject);
        });
    }
}
