using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Party
{
    [field: SerializeField] public Inventory Inventory { get; private set; }
    [field: SerializeField] public CurrencyWallet CurrencyWallet { get; private set; }


    [SerializeField] private List<CharacterInstance> pendingMembers; // 대기 파티원

    [SerializeField] private List<CharacterInstance> members; // 현재 파티원
    public IReadOnlyList<CharacterInstance> Members => members.AsReadOnly();
    public IReadOnlyList<CharacterInstance> PendingMembers => pendingMembers.AsReadOnly();

    public CharacterInstance DefaulMember { get; private set; } // 기본 주인공

    public int MaxPending { get; private set; }
    public int MaxMembers { get; private set; }

    public Party(Inventory inventory = null)
    {
        Inventory = inventory ?? new Inventory();
        CurrencyWallet = new CurrencyWallet(1000);

        MaxPending = 3;
        MaxMembers = 4;

        members = new List<CharacterInstance>();
        pendingMembers = new List<CharacterInstance>();

        if (DefaulMember != null)
        members.Add(DefaulMember);
    }

    public bool AddMember(CharacterInstance newMember) // 파티원 추가
    {
        if (pendingMembers.Exists(m => m.ID == newMember.ID) || members.Exists(m => m.ID == newMember.ID))
        {
            return false;
        }
        
        if(members.Count < MaxMembers)
        {
            members.Add(newMember);
            return true;
        }
        else if(pendingMembers.Count < MaxPending)
        {
            pendingMembers.Add(newMember);
            return true;
        }
        return false;
    }

    public void ClearMembers()
    {
        members.Clear();
    }

    public List<CharacterInstance> GetAliveMembers() // 현재 생존해 있는 파티원 목록을 반환합니다.
    {
        return members.Where(member => member.CurrentHp > 0).ToList();
    }

    public void UpdateMemberHealth(int index, int hp) // 지정된 인덱스의 파티원 체력을 업데이트합니다.
    {
        if (index >= 0 && index < members.Count)
        {
            members[index].SetCurrentHp(hp);
        }
        else
        {
            Logger.LogError($"[Party] UpdateMemberHealth 호출 시 인덱스 범위 초과: index={index}");
        }
    }
}
