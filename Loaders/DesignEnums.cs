using System;

public static class DesignEnums
{
    public enum SkillType
    {
        None = 0,
        Attack = 1,
        Heal = 2,
        Buff = 3,
        Debuff = 4,
        Stress = 5,
    }
    public enum TargetType
    {
        None = 0,
        Self = 1,
        AllySingle = 2,
        AllyAll = 3,
        OpponentSingle = 4,
        OpponentAll = 5,
    }
    public enum AttackType
    {
        None = 0,
        Physical = 1,
        Special = 2,
        Flare = 3,
    }
    public enum CostType
    {
        None = 0,
        HP = 1,
        HG = 2,
        ST = 3,
    }
    public enum JobType
    {
        None = 0,
        Swordsman = 1,
        Spearman = 2,
        Axeman = 3,
        Archer = 4,
    }
    public enum BuffType
    {
        None = 0,
        IncreaseATK = 1,
        DecreaseATK = 2,
        IncreaseDEF = 3,
        DecreaseDEF = 4,
        IncreaseSATK = 5,
        DecreaseSATK = 6,
        IncreaseSDEF = 7,
        DecreaseSDEF = 8,
        IncreaseSPD = 9,
        DecreaseSPD = 10,
        IncreaseCritical = 11,
        DecreaseCritical = 12,
        IncreaseEVA = 13,
        DecreaseEVA = 14,
        BleedBuff = 15,
        HealBuff = 16,
    }
    public enum EnemyType
    {
        None = 0,
        Normal = 1,
        Elite = 2,
        Boss = 3,
    }
    public enum TraitCondition
    {
        None = 0,
        StressAbove = 1,
        TurnCountAbove = 2,
        HealthBelowPercentage = 3,
    }
    public enum TraitEffect
    {
        None = 0,
        UnlockEmotion = 1,
    }
    public enum ItemType
    {
        None = 0,
        Consumable = 1,
        Weapon = 2,
        Armor = 3,
        Accessory = 4,
        Rune = 5,
        General = 6,
        Quest = 7,
    }
    public enum EquipType
    {
        None = 0,
        Weapon = 1,
        Armor = 2,
        Accessory = 3,
        Rune = 4,
    }
    public enum AbilityType
    {
        None = 0,
        ATK = 1,
        SATK = 2,
        DEF = 3,
        SDEF = 4,
        SPD = 5,
        Critical = 6,
        EVA = 7,
        FS = 8,
    }
    public enum RuneValue
    {
        None = 0,
        Normal = 1,
        Rare = 2,
        Epic = 3,
        Legendary = 4,
    }
    public enum EmotionType
    {
        None = 0,
        Joy = 1,
        Anger = 2,
        Sorrow = 3,
        Pleasure = 4,
        Affection = 5,
        Disgust = 6,
        Desire = 7,
    }
    public enum BuffName
    {
        None = 0,
        Pleasure = 1,
        Awaken = 2,
        Collapse = 3,
        Transcendent = 4,
    }
    public enum PointType
    {
        None = 0,
        Hit = 1,
        Ground = 2,
        Attack = 3,
        Center = 4,
        Head = 5,
        Overhead = 6,
        Floating = 7,
        Damage = 8,
    }
    public enum ConsumableType
    {
        None = 0,
        HealHP = 1,
    }
    public enum EffectSpawn
    {
        None = 0,
        Target = 1,
        Caster = 2,
        Range = 3,
        Ground = 4,
        Center = 5,
        Hit = 6,
    }
}
