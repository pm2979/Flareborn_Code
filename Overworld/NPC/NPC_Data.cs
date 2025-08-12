using UnityEngine;

public enum NPCType
{
    Default,
    Merchant,
    Tutorial,
}

[System.Serializable]
public class NPC_Data
{
    [SerializeField] private string npcName;
    [SerializeField] private E_NPCList npc;
    [SerializeField] private NPCType npcType;

    // 프로퍼티
    public string NPCName { get { return npcName; } set { npcName = value; } }
    public NPCType NPCType { get { return npcType; } set { npcType = value; } }
}
