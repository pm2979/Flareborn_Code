using System.Collections.Generic;
using UnityEngine;

public class PartyMemberSaver : IDataPersistence
{
    private PartyManager partyManager;

    public PartyMemberSaver(PartyManager partyManager)
    {
        this.partyManager = partyManager;

        if (partyManager == null || partyManager.Party == null)
        {
            Debug.LogWarning("[PartyMemberSaver] 생성자에서 PartyManager 또는 Party가 null입니다.");
        }
    }

    public void SaveData(ref GameData data)
    {
        if (partyManager == null || partyManager.Party == null)
        {
            Debug.LogWarning("[PartyMemberSaver] SaveData: PartyManager 또는 Party가 null입니다. 저장 생략.");
            return;
        }

        data.savedPartyMembers.Clear();

        foreach (CharacterInstance member in partyManager.Party.Members)
        {
            var savedChar = new SavedCharacterData(
                member.ID,
                member.Name,
                member.CurrentHp,
                member.CurrentExp,
                member.Level,
                member.CurrentStress,
                member.CurrentHG,
                member.IsAwakened == 1,
                member.IsCollapsed
            );
            data.savedPartyMembers.Add(savedChar);

            Debug.Log($"[PartyMemberSaver] 멤버 저장: {member.Name} (ID: {member.ID}, HP: {member.CurrentHp})");
        }

        Debug.Log($"[PartyMemberSaver] SaveData: {data.savedPartyMembers.Count}명의 파티원 저장 완료.");
    }

    public void LoadData(GameData data)
    {
        // 불러오기 기능 필요 시 구현
    }
}
