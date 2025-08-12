using System.Collections.Generic;
using UnityEngine;

public class PartyManager: MonoBehaviour
{
    [field:SerializeField]public Party Party { get; private set; }

    [SerializeField] private int[] defaultPartyKeys; // 테스트용 시작 멤버 생성

    // 플레이어 파티의 월드 내 위치
    private Vector3 partyPosition;

    public void Init(Party party = null) // 초기화
    {
        DataManager dataManager = GameManager.Instance.DataManager;

        Party = party ?? new Party();

        InitializeDefaultParty();
    }

    private void InitializeDefaultParty()
    {
        if (Party.Members.Count != 0) return;

        Party.ClearMembers();

        foreach (int key in defaultPartyKeys)
        {
            AddMemberByKey(key);
        }
    }

    public void AddMemberByKey(int key)
    {
        // 팩토리를 통해 캐릭터 인스턴스 생성
        CharacterInstance newMember = CharacterFactory.CreateCharacter(key);

        if (newMember == null)
        {
            // 캐릭터 생성 실패 시 로그는 팩토리 내부에서 처리
            return;
        }

        // Party 객체에 멤버 추가 시도
        if (Party.AddMember(newMember))
        {
            Logger.Log($"[PartyManager] 멤버 추가 성공: {newMember.Name}");
        }
        else
        {
            Logger.Log($"[PartyManager] 멤버가 이미 파티에 존재합니다: key={key}");
        }
    }

    public IReadOnlyList<CharacterInstance> GetCurrentParty() // 현재 파티원 반환
    {
        // 여기서 파티 멤버 상태를 출력해보기
        foreach (var member in Party.Members)
        {
            Debug.Log($"Party member ID: {member.ID}, Name: {member.Name}");
        }

        return Party.Members;
    }

    public IReadOnlyList<CharacterInstance> GetAliveParty()
    {
        return Party.GetAliveMembers();
    }

    public void SaveHealth(int index, int hp)
    {
        if (index >= 0 && index < Party.Members.Count)
        {
            Party.Members[index].SetCurrentHp(hp);
        }
        else
        {
            Debug.LogWarning($"[PartyManager] SaveHealth 호출 시 인덱스 범위 초과: index={index}");
        }
    }

    public void SetPosition(Vector3 position)
    {
        partyPosition = position;
    }

    public Vector3 GetPosition()
    {
        return partyPosition;
    }
}