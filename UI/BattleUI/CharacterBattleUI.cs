using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBattleUI : MonoBehaviour
{
    public PartyEntities Entities { get; private set; } // 캐릭터 스탯 연결

    [Header("UI Elements")]
    [field:SerializeField] public TextMeshProUGUI NameText {  get; private set; }
    [field:SerializeField] public Image HPBar {  get; private set; }
    [field: SerializeField] public TextMeshProUGUI HPText { get; private set; }
    [field: SerializeField] public Image STBar { get; private set; }
    [field: SerializeField] public TextMeshProUGUI STText { get; private set; }
    [field: SerializeField] public Image HGBar { get; private set; }
    [field: SerializeField] public GameObject HGObj { get; private set; }

    public void Init(PartyEntities entities)
    {
        Entities = entities;
        UpdateUI();

        if(Entities == null)
        {
            gameObject.SetActive(false);
        }
    }

    // UI 업데이트 함수
    public void UpdateUI()
    {
        if (Entities == null)
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            gameObject.SetActive(true);
        }

        // 이름
        NameText.text = Entities.Name;

        // HP 업데이트
        float hpRatio = (float)Entities.CurrHP / Entities.MaxHP;
        HPBar.fillAmount = hpRatio;
        HPText.text = $"{Entities.CurrHP} / {Entities.MaxHP}";

        // ST 업데이트
        float stRatio = (float)Entities.Stress.CurrentStress / Entities.Stress.MaxStress;
        STBar.fillAmount = stRatio;
        STText.text = $"{Entities.Stress.CurrentStress} / {Entities.Stress.MaxStress}";

        // HG 업데이트
        if(Entities.Humanity != null)
        {
            float hgRatio = (float)Entities.Humanity.CurrentHumanity / Entities.Humanity.MaxHumanity;
            HGBar.fillAmount = hgRatio;
        }
        else
        {
            HGObj.SetActive(false);
        }
    }

    void LateUpdate()
    {
        // 매 프레임 UI를 업데이트하여 실시간 변경사항을 반영
        UpdateUI();
    }
}
