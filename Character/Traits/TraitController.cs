using System.Collections.Generic;
using System.Linq;

public class TraitController
{
    public List<Trait> Traits { get; private set; }
    private PartyEntities owner;

    public TraitController(PartyEntities owner, List<Trait> traits = null)
    {
        this.owner = owner;
        this.Traits = traits ?? new List<Trait>();

        foreach(Trait trait in traits)
        {
            trait.Deactivate(owner);
        }
    }

    public void OnTurnStart() // 턴 시작 특성 활성화 확인
    {
        Logger.Log("특성");

        foreach (Trait trait in Traits)
        {
            bool conditionMet = trait.CheckConditions(owner);

            if (conditionMet && !trait.IsActive) // 활성화
            {
                trait.Activate(owner);
                Logger.Log("특성 활성화");
                Logger.Log(trait.Name);
            }
            else if (!conditionMet && trait.IsActive) // 비활성화
            {
                trait.Deactivate(owner);
                Logger.Log("특성 비활성화");
                Logger.Log(trait.Name);
            }
        }
    }

    public bool HasTrait(int traitID)
    {
        return Traits.Exists(trait => trait.ID == traitID);
    }

    public bool AddTrait(int key)
    {
        if(Traits.Exists(t => t.ID == key))
        {
            return false;
        }

        Traits.Add(TraitFactory.CreatTrait(key));

        return true;
    }

    public bool AddTrait(Trait trait)
    {
        if (Traits.Exists(t => t.ID == trait.ID))
        {
            return false;
        }

        Traits.Add(trait);

        return true;
    }

    public bool RemoveTrait(int traitID) // 특성 삭제
    {
        var traitToRemove = Traits.FirstOrDefault(t => t.ID == traitID);
        if (traitToRemove != null)
        {
            Traits.Remove(traitToRemove);
            return true;
        }
        return false;
    }

}
