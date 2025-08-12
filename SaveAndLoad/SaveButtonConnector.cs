using UnityEngine;
using UnityEngine.UI;

public class SaveButtonConnector : MonoBehaviour
{
    public Button saveButton1;
    public Button saveButton2;
    public Button saveButton3;

    private void Start()
    {
        // SaveAndLoadManager에 접근
        var manager = SaveAndLoadManager.Instance;

        // 버튼마다 슬롯 ID를 다르게 연결
        saveButton1.onClick.AddListener(() => manager.SaveToSlot("save_slot_1"));
        saveButton2.onClick.AddListener(() => manager.SaveToSlot("save_slot_2"));
        saveButton3.onClick.AddListener(() => manager.SaveToSlot("save_slot_3"));
    }
}
