// EquippedRuneSaver.cs
using UnityEngine;
using System.Collections.Generic;

public class EquippedRuneSaver : IDataPersistence
{
    private PartyManager partyManager;

    public EquippedRuneSaver(PartyManager partyManager)
    {
        this.partyManager = partyManager;
    }
    public void SaveData(ref GameData data)
    {
        if (partyManager == null || partyManager.Party == null)
        {
            Debug.LogWarning("[RuneSaver] PartyManager 또는 Party가 null입니다.");
            return;
        }

        data.savedRunes.Clear();

        foreach (var character in partyManager.Party.Members)
        {
            var runeController = character.RuneController;

            var runeKeys = new List<int>();
            foreach (var slot in runeController.RuneSlots)
            {
                if (slot?.RuneItem?.RuneData != null)
                    runeKeys.Add(slot.RuneItem.RuneData.key);
                else
                    runeKeys.Add(-1);
            }

            var saved = new SavedEquippedRuneData
            {
                characterID = character.ID,
                equippedRuneKeys = runeKeys
            };

            data.savedRunes.Add(saved);

            Debug.Log($"[RuneSaver] {character.Name}의 룬 저장 완료.");
        }
    }

    public void LoadData(GameData data)
    {
        // 로드 기능은 현재 구현하지 않습니다.
        // Debug.LogWarning("[EquippedRuneSaver] LoadData 메서드는 현재 구현되지 않았습니다. 로딩이 생략됩니다.");
    }
}