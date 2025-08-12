using System;
using static DesignEnums;

[Serializable]
public class RuneStat
{
    public AbilityType AbilityType { get; set; }
    public RuneValue RuneType { get; set; }
    public int Value { get; set; }

    public float HP { get; set; }
    public float ATK { get; set; }
    public float SATK { get; set; }
    public float DEF { get; set; }
    public float SDEF { get; set; }
    public float SPD { get; set; }
    public float Critical { get; set; }
    public float EVA { get; set; }
    public float FS { get; set; }

    public void ApplyValueToStat()
    {
        switch (AbilityType)
        {
            case AbilityType.ATK:
                ATK = Value;
                break;
            case AbilityType.SATK:
                SATK = Value;
                break;
            case AbilityType.DEF:
                DEF = Value;
                break;
            case AbilityType.SDEF:
                SDEF = Value;
                break;
            case AbilityType.SPD:
                SPD = Value;
                break;
            case AbilityType.Critical:
                Critical = Value;
                break;
            case AbilityType.EVA:
                EVA = Value;
                break;
            case AbilityType.FS:
                FS = Value;
                break;
            default:
                break;
        }
    }
}
