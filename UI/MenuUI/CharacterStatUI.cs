using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatUI : MonoBehaviour
{
    public CharacterInstance Character { get; private set; } // 캐릭터 스탯 연결

    [Header("UI Elements")]
    [field: SerializeField] public TextMeshProUGUI NameText { get; private set; }
    [field: SerializeField] public Image HPBar { get; private set; }
    [field: SerializeField] public TextMeshProUGUI HPText { get; private set; }
    [field: SerializeField] public Image STBar { get; private set; }
    [field: SerializeField] public TextMeshProUGUI STText { get; private set; }
    [field: SerializeField] public Image HGBar { get; private set; }
    [field: SerializeField] public GameObject HGObj { get; private set; }

    public event Action<CharacterInstance> OnCharacterSelected;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(HandleClick);
    }
    
    public void Set(CharacterInstance character)
    {
        Character = character;

        if (Character == null)
        {
            gameObject.SetActive(false);
        }

        UpdateUI();
    }

    // UI 업데이트 함수
    public void UpdateUI()
    {
        if (Character == null)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        // 이름
        NameText.text = Character.Name;

        // HP 업데이트
        float hpRatio = (float)Character.CurrentHp / Character.MaxHp;
        HPBar.fillAmount = hpRatio;
        HPText.text = $"{Character.CurrentHp} / {Character.MaxHp}";

        // ST 업데이트
        float stRatio = (float)Character.CurrentStress / 100f;
        STBar.fillAmount = stRatio;
        STText.text = $"{Character.CurrentStress} / {100}";

        // HG 업데이트
        if (Character.IsFlare == true)
        {
            HGObj.SetActive(true);
            float hgRatio = (float)Character.CurrentHG / 100f;
            HGBar.fillAmount = hgRatio;
        }
        else
        {
            HGObj.SetActive(false);
        }
    }

    // 버튼이 클릭되었을 때 호출
    private void HandleClick()
    {
        OnCharacterSelected?.Invoke(Character);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(HandleClick);
    }

}
