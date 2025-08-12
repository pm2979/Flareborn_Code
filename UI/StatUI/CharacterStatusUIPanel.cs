using UnityEngine;
using TMPro;
using System.Text;
using System.Collections.Generic;

public class CharacterStatusUIPanel : MonoBehaviour
{
    [Header("기본 정보")]
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI jobText;

    [Header("능력치")]
    [SerializeField] private TextMeshProUGUI statsText; // 모든 스탯을 한 번에 표시할 텍스트

    [Header("스킬 정보")]
    [SerializeField] private List<InfoTextUI> skillTexts;
    [SerializeField] private InfoTextUI basicAttackText;
    [SerializeField] private InfoTextUI FlareSkillText;
    [SerializeField] private Transform FlareSkillContainer;

    [Header("특성 정보")]
    [SerializeField] private List<InfoTextUI> traitTexts;

    [Header("리스트 아이템 프리팹")]
    [SerializeField] private GameObject listItemPrefab; // 스킬/특성 이름 하나를 표시할 간단한 Text 프리팹

    public void DisplayCharacterInfo(CharacterInstance character)
    {
        if (character == null) return;

        // 기본 정보 설정
        nameText.text = character.Name;
        levelText.text = $"Lv. {character.Level}";
        hpText.text = $"HP: {character.CurrentHp} / {character.MaxHp}";
        jobText.text = $"직업 : {character.Job.JobType}";

        // 능력치 정보 설정
        StringBuilder statsBuilder = new StringBuilder();

        statsBuilder.AppendLine($"공격(ATK): {character.ATK}");
        statsBuilder.AppendLine();
        statsBuilder.AppendLine($"특수공격(SATK): {character.SATK}");
        statsBuilder.AppendLine();
        statsBuilder.AppendLine($"방어(DEF): {character.DEF}");
        statsBuilder.AppendLine();
        statsBuilder.AppendLine($"특수방어(SDEF): {character.SDEF}");
        statsBuilder.AppendLine();
        statsBuilder.AppendLine($"속도(SPD): {character.SPD}");
        statsBuilder.AppendLine();
        statsBuilder.AppendLine($"치명타(Critical): {character.Critical}%");
        statsBuilder.AppendLine();
        statsBuilder.AppendLine($"회피(EVA): {character.EVA}%");
        statsBuilder.AppendLine();
        statsBuilder.AppendLine($"무력화 피해(SP): {character.SP}");

        if (character.IsFlare)
        {
            statsBuilder.AppendLine();
            statsBuilder.AppendLine($"플레어 스탯(PS): {character.FS}");
        }

        statsText.text = statsBuilder.ToString();

        // 스킬 리스트 채우기
        for (int i = 0; i < skillTexts.Count; i++)
        {
            if (i > character.EquippedSkills.Count - 1)
            {
                skillTexts[i].gameObject.SetActive(false);
            }
            else
            {
                skillTexts[i].gameObject.SetActive(true);
                skillTexts[i].Set($"{character.EquippedSkills[i].Name} : {character.EquippedSkills[i].Description}");
            }
        }

        basicAttackText.Set($"{character.BasicAttack.Name} : {character.BasicAttack.Description}");

        if(character.IsFlare)
        {
            FlareSkillContainer.gameObject.SetActive(true);
            FlareSkillText.Set($"{character.FlareSkill.Name} : {character.FlareSkill.Description}");
        }
        else
        {
            FlareSkillContainer.gameObject.SetActive(false);
        }

        // 특성 리스트 채우기
        for (int i = 0; i < traitTexts.Count; i++)
        {
            if (i > character.Traits.Count - 1)
            {
                traitTexts[i].gameObject.SetActive(false);
            }
            else
            {
                traitTexts[i].gameObject.SetActive(true);
                traitTexts[i].Set($"{character.Traits[i].Name} : {character.Traits[i].Description}");
            }
        }

    }

    private void AddItemToList(Transform container, string itemName)
    {
        GameObject itemGO = Instantiate(listItemPrefab, container);
        itemGO.GetComponentInChildren<TextMeshProUGUI>().text = itemName;
    }
}
