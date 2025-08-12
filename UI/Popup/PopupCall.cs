using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum PopupType
{
    OkOnly,
    OkNo,
}

public class PopupCall : MonoSingleton<PopupCall>
{
    [Tooltip("팝업 전체를 감싸는 CanvasGroup (페이드 인/아웃용)")]
    public CanvasGroup popupPanel;
    [Tooltip("메시지 텍스트")]
    public TextMeshProUGUI messageText;
    [Tooltip("OK 버튼")]
    public Button okButton;
    [Tooltip("NO 버튼")]
    public Button noButton;
    
    private Action _onOkCallback; // OK 버튼 클릭 시 실행될 콜백
    private Action _onNoCallback; // NO 버튼 클릭 시 실행될 콜백

    private List<Button> _navigableButtons; // 현재 탐색 가능한 버튼 목록
    private int _selectedButtonIndex = 0; // 현재 선택된 버튼의 인덱스

    protected override void Awake()
    {
        base.Awake(); 
        okButton.onClick.AddListener(OnOkButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }
    
    protected override void OnEnable()
    {
        SelectButton(0); 
    }
    
    public void SetupPopup(string message, PopupType type, Action onOk, Action onNo)
    {
        popupPanel.gameObject.SetActive(true);
        messageText.text = message;
        _onOkCallback = onOk;
        _onNoCallback = onNo;

        _navigableButtons = new List<Button>();

        switch (type)
        {
            case PopupType.OkOnly:
                okButton.gameObject.SetActive(true);
                noButton.gameObject.SetActive(false);
                _navigableButtons.Add(okButton);
                break;
            case PopupType.OkNo:
                okButton.gameObject.SetActive(true);
                noButton.gameObject.SetActive(true);
                _navigableButtons.Add(okButton); 
                _navigableButtons.Add(noButton); 
                break;
        }
    }
    
    void Update()
    {
        HandleCustomNavigation();
    }
    
    private void HandleCustomNavigation()
    {
        bool moved = false;
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _selectedButtonIndex = (_selectedButtonIndex + 1) % _navigableButtons.Count;
            moved = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _selectedButtonIndex = (_selectedButtonIndex - 1 + _navigableButtons.Count) % _navigableButtons.Count;
            moved = true;
        }

        if (moved)
        {
            SelectButton(_selectedButtonIndex);
        }
        
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
        {
            if (_navigableButtons.Count > 0)
            {
                _navigableButtons[_selectedButtonIndex].onClick.Invoke();
            }
        }
    }
    
    private void SelectButton(int index)
    {
        if (index < 0 || index >= _navigableButtons.Count) return;

        _navigableButtons[0].GetComponent<OnOffButton>()?.Off();
        _navigableButtons[1].GetComponent<OnOffButton>()?.Off();

        OnOffButton selectedOnOffButton = _navigableButtons[index].GetComponent<OnOffButton>();
        if (selectedOnOffButton != null)
        {
            selectedOnOffButton.On();
        }

        EventSystem.current.SetSelectedGameObject(_navigableButtons[index].gameObject);
    }

    private void OnOkButtonClicked()
    {
        _onOkCallback?.Invoke();
        DestroyPopup();
    }

    private void OnNoButtonClicked()
    {
        _onNoCallback?.Invoke();
        DestroyPopup();
    }

    private void DestroyPopup()
    {
        popupPanel.gameObject.SetActive(false);
    }

     protected override void OnDestroy()
    {
        base.OnDestroy();
        okButton.onClick.RemoveListener(OnOkButtonClicked);
        noButton.onClick.RemoveListener(OnNoButtonClicked);
    }

}
