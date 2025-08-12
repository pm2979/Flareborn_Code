// 커스텀 에디터 
#if UNITY_EDITOR
using System.Security;
using UnityEditor;
#endif
using UnityEngine;

public enum E_QuestType
{
    Main,
    Side,
}

public enum E_QuestChpater
{
    Chapter0,
    Chapter1,
}

public enum E_NPCList
{
    // Ink에서 사용하는 NPC 이름과 동일하게 설정
    NPC_Mother,
    NPC_Dian,
    NPC_Nepin,
    NPC_Tom,
    NPC_Anna,
    NPC_Bramdur,
    NPC_Amila,
    NPC_Lutina,
    NPC_Unknown,
    NPC_Marfin,
    NPC_Cleric,
    NPC_Serian,
    NPC_Rene,
}

public enum  E_ExtraRewardsType
{
    GetMember,
    GetItem,
}

[CreateAssetMenu(fileName = "QuestInfoSO", menuName = "ScriptableObjects/QuestInfoSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    // 각 퀘스트와 연결되어있는 Dialogue의 Knot 이름은 퀘스트의 ID와 동일하게 설정.
    [field: SerializeField] public string id { get; private set; }

    [Header("Quest")]
    public string displayName;
    public E_QuestType questType;
    public E_NPCList questNPC;
    public E_QuestChpater questChapter;

    [Header("Requirements")]
    public int levelRequirement;
    public QuestInfoSO[] questPrerequisites;

    [Header("Steps")]
    public GameObject[] questStepPrefabs;

    [Header("Rewards")]
    public int goldReward;
    public int expReward;
    public bool isThereExtraRewards;
    public E_ExtraRewardsType extraRewardsType;
    public string extraRewards;
    public int memberKey; // 멤버 획득 보상
    public E_NPCList npc; // 멤버로 획득하는 NPC 
    public int itemKey; // 아이템 획득 보상

    // ID가 언제나 스크립터블 오브젝트의 이름이 될 수 있도록 한다.
    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);  
#endif
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(QuestInfoSO))]
public class QuestInfoEditor : Editor
{
    SerializedProperty isThereExtraRewardsProp;
    SerializedProperty extraRewardsTypeProp;
    SerializedProperty extraRewardsProp;
    SerializedProperty rewardsGetMemberProp;
    SerializedProperty npcProp;
    SerializedProperty rewardsGetItemProp;

    private void OnEnable()
    {
        isThereExtraRewardsProp = serializedObject.FindProperty("isThereExtraRewards");
        extraRewardsTypeProp = serializedObject.FindProperty("extraRewardsType");
        extraRewardsProp = serializedObject.FindProperty("extraRewards");
        npcProp = serializedObject.FindProperty("npc");
        rewardsGetMemberProp = serializedObject.FindProperty("memberKey");
        rewardsGetItemProp = serializedObject.FindProperty("itemKey");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        DrawPropertiesExcluding(serializedObject, "isThereExtraRewards", "extraRewardsType", "extraRewards", "memberKey", "npc", "itemKey");

        EditorGUILayout.PropertyField(isThereExtraRewardsProp);

        if (isThereExtraRewardsProp.boolValue)
        {
            EditorGUILayout.PropertyField(extraRewardsTypeProp);
            EditorGUILayout.PropertyField(extraRewardsProp);

            var type = (E_ExtraRewardsType)extraRewardsTypeProp.enumValueIndex;

            if (type == E_ExtraRewardsType.GetMember)
            {
                EditorGUILayout.PropertyField(rewardsGetMemberProp);
                EditorGUILayout.PropertyField(npcProp);
            }
            else if (type == E_ExtraRewardsType.GetItem)
            {
                EditorGUILayout.PropertyField(rewardsGetItemProp);
            }
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif