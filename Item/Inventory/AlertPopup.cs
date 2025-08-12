using System.Collections;
using TMPro;
using UnityEngine;

public class AlertPopup : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI messageText;

    private void Awake()
    {
        Hide(); // 초기 비활성화
    }

    public void Show(string message)
    {
        messageText.text = message;
        panel.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(AutoHideRoutine());
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    public void OnConfirmClick()
    {
        Hide();
    }
    private IEnumerator AutoHideRoutine()
    {
        yield return new WaitForSeconds(5f);
        Hide();
    }

}


//using UnityEngine;
//using TMPro;

//public class AlertPopup : MonoBehaviour
//{
//    [SerializeField] private GameObject panel;
//    [SerializeField] private TextMeshProUGUI messageText;

//    public static AlertPopup Instance;

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
//        Instance = this;
//        Hide();
//    }

//    public void Show(string message)
//    {
//        messageText.text = message;
//        panel.SetActive(true);
//    }

//    public void Hide()
//    {
//        panel.SetActive(false);
//    }

//    public void OnConfirmClick()
//    {
//        Hide();
//    }
//}
