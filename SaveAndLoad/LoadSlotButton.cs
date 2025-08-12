using UnityEngine;

public class LoadSlotButton : MonoBehaviour
{
    [SerializeField] private string slotId;

    public void Load()
    {
        Debug.Log($"{slotId}로드됨");
        // SaveAndLoadManager.Instance.LoadFromSlot(slotId);
    }
}
