using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleStatusUI : MonoBehaviour
{
    // 대상이 될 PartyEntities
    private PartyEntities targetEntity;

    [Header("기본 정보")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI stressText;
    [SerializeField] private TextMeshProUGUI humanityText;

    [Header("능력치 텍스트")]
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI defText;
    [SerializeField] private TextMeshProUGUI satkText;
    [SerializeField] private TextMeshProUGUI sdefText;
    [SerializeField] private TextMeshProUGUI spdText;
    [SerializeField] private TextMeshProUGUI criticalText;
    [SerializeField] private TextMeshProUGUI evaText;
    [SerializeField] private TextMeshProUGUI spText;
    [SerializeField] private TextMeshProUGUI fsText;

    [Header("특성")]
    [SerializeField] private TextMeshProUGUI traitsText;

    [SerializeField] private Button exitButton; // 나가기 버튼

    public void Init(Action onExitCallback)
    {
        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(() => onExitCallback?.Invoke());
    }

    public void SetTarget(PartyEntities entity) // 플레이어 연결
    {
        targetEntity = entity;
        UpdateUI();
    }

    public void UpdateUI() // UI 업데이트
    {
        if (targetEntity == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        // 기본 정보 업데이트
        characterNameText.text = targetEntity.Name;
        hpText.text = $"체력 : {targetEntity.CurrHP} / {targetEntity.MaxHP}";
        if (targetEntity.Stress != null)
        {
            stressText.text = $"스트레스 : {targetEntity.Stress.CurrentStress} / {targetEntity.Stress.MaxStress}";
        }
        if (targetEntity.Humanity != null)
        {
            humanityText.gameObject.SetActive(true);
            humanityText.text = $"인간성 : {targetEntity.Humanity.CurrentHumanity} / {targetEntity.Humanity.MaxHumanity}";
        }
        else
        {
            humanityText.gameObject.SetActive(false);
        }

        // 능력치 업데이트
        UpdateStatText(atkText, "공격력", targetEntity.BaseATK, targetEntity.ATK);
        UpdateStatText(defText, "방어력", targetEntity.BaseDEF, targetEntity.DEF);
        UpdateStatText(satkText, "특수공격력", targetEntity.BaseSATK, targetEntity.SATK);
        UpdateStatText(sdefText, "특수방어력", targetEntity.BaseSDEF, targetEntity.SDEF);
        UpdateStatText(spdText, "속도", targetEntity.BaseSPD, targetEntity.SPD);
        UpdateStatText(criticalText, "치명타", targetEntity.BaseCritical, targetEntity.Critical);
        UpdateStatText(evaText, "회피율", targetEntity.BaseEVA, targetEntity.EVA);
        UpdateStatText(spText, "무력화", targetEntity.SP, targetEntity.SP);
        UpdateStatText(fsText, "플레어 스탯", targetEntity.FS, targetEntity.FS, targetEntity.IsFlare);

        // 특성 정보 업데이트
        if (targetEntity.Trait != null && traitsText != null)
        {
            List<Trait> traits = targetEntity.Trait.Traits;

            if (traits.Any())
            {
                IEnumerable<string> traitInfos = traits.Select(trait => $"<b>{trait.Name}</b>: {trait.Description}");
                traitsText.text = "<b>특성</b>\n" + string.Join("\n", traitInfos);
            }
            else
            {
                traitsText.text = "특성: 없음";
            }
        }
    }

    private void UpdateStatText(TextMeshProUGUI textElement, string statName, int baseValue, int currentValue, bool isFlare = true)
    {
        if (textElement == null) return;

        if (isFlare == false) 
        {
            textElement.text = "";
            return;
        }

        int change = currentValue - baseValue;
        string color = "white";

        if (change > 0)
        {
            color = "green";
            textElement.text = $"{statName}: {currentValue} <color={color}>(+{change})</color>";
        }
        else if (change < 0)
        {
            color = "red";
            textElement.text = $"{statName}: {currentValue} <color={color}>({change})</color>";
        }
        else
        {
            textElement.text = $"{statName}: {currentValue}";
        }
    }
}
