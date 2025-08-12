using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Trait
{
    public int ID { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    public string Description;

    public TraitData TraitData { get; private set; }

    public bool IsActive { get; private set; } // 활성화 상태

    private List<ITraitCondition> conditions;
    private List<ITraitEffect> effects;

    public Trait(TraitData data, List<ITraitCondition> conds, List<ITraitEffect> effs)
    {
        TraitData = data;
        ID = data.key;
        Name = data.name;
        Description = data.Description;
        conditions = conds;
        effects = effs;
        IsActive = false;
    }

    // 모든 발동 조건 확인
    public bool CheckConditions(PartyEntities owner)
    {
        return conditions.All(condition => condition.Evaluate(owner));
    }

    // 특성 활성화
    public void Activate(PartyEntities owner)
    {
        foreach (ITraitEffect effect in effects)
        {
            effect.Apply(owner);
        }
        IsActive = true;
    }

    // 특성 비활성화
    public void Deactivate(PartyEntities owner)
    {
        foreach (ITraitEffect effect in effects)
        {
            effect.UnApply(owner);
        }
        IsActive = false;
    }
}
