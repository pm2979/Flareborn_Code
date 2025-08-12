using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatPanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI defenseText;

    public List<CharacterInstance> CurrentParty { get; set; }

    private void Start()
    {

        RefreshUI();
    }

    private void OnEquipmentChanged()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {

    }
}
