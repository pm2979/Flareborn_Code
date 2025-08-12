using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering.Universal;

public class QuestDataBase_SortByNPC
{
    // 각 NPC별 퀘스트리스트를 쉽게 찾을 수 있도록 딕셔너리로 구성
    // Dictionary<npc이름, Dictionary<퀘스트ID, Quest>>
    private Dictionary<string, Dictionary<string, Quest>> npc_QuestLists;

    // 프로퍼티
    public Dictionary<string, Dictionary<string, Quest>> NPC_QuestLists
    {
        get { return npc_QuestLists; }
        set { npc_QuestLists = value; }
    }


    // 생성자 (Constructor)
    public QuestDataBase_SortByNPC()
    {
        NPC_QuestLists = new Dictionary<string, Dictionary<string, Quest>>();

        // E_NPCList Enum의 모든 값을 순회하여 딕셔너리를 초기화
        foreach (E_NPCList npc in Enum.GetValues(typeof(E_NPCList)))
        {
            // 각 NPC에 대한 퀘스트 리스트를 초기화
            npc_QuestLists[EnumToString(npc)] = new Dictionary<string, Quest>();
        }
    }

    public string EnumToString(E_NPCList npc)
    {
        // Enum을 문자열로 변환하여 딕셔너리 키로 사용
        return npc.ToString();
    }
}

public class QuestDataBase_SortByQuestState
{
    // 퀘스트 상태별 딕셔너리
    private Dictionary<string, Quest> red_QuestList;
    private Dictionary<string, Quest> yellow_QuestList;
    private Dictionary<string, Quest> green_QuestList;

    // 프로퍼티
    public Dictionary<string, Quest> Red_QuestList { get { return red_QuestList; } set { red_QuestList = value; } }
    public Dictionary<string, Quest> Yellow_QuestList { get { return yellow_QuestList; } set { yellow_QuestList = value; } }
    public Dictionary<string, Quest> Green_QuestList { get { return green_QuestList; } set { green_QuestList = value; } }

    // 생성자 (Constructor)
    public QuestDataBase_SortByQuestState()
    {
        red_QuestList = new Dictionary<string, Quest>();
        yellow_QuestList = new Dictionary<string, Quest>();
        green_QuestList = new Dictionary<string, Quest>();
    }

    public void ClearLists()
    {
        // 퀘스트 상태별 리스트를 초기화
        red_QuestList.Clear();
        yellow_QuestList.Clear();
        green_QuestList.Clear();
    }
}

public class QuestDataBase : MonoBehaviour
{
    private QuestDataBase_SortByNPC byNPC;
    private QuestDataBase_SortByQuestState byState;

    // 모든 퀘스트를 저장하는 딕셔너리
    private Dictionary<string, Quest> allQuests;

    // 프로퍼티 
    public Dictionary<string, Quest> AllQuests { get { return allQuests; } set { allQuests = value; } }
    public QuestDataBase_SortByNPC ByNPC { get { return byNPC; } }
    public QuestDataBase_SortByQuestState ByState { get { return byState; } }

    private void Awake()
    {
        byNPC = new QuestDataBase_SortByNPC();
        byState = new QuestDataBase_SortByQuestState();

        Init();
        Debug.Log("[QuestDataBase] Initialized successfully.");
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        // 퀘스트 정보를 Resources폴더에서 로드한다.
        QuestInfoSO[] allQuestInfoSOs = Resources.LoadAll<QuestInfoSO>("Quests");

        // 퀘스트 ID를 키로 하고 Quest 객체를 값으로 하는 딕셔너리를 생성한다.
        Dictionary<string, Quest> idToQuestDic = new Dictionary<string, Quest>();

        // 퀘스트 정보를 루프돌며 딕셔너리에 추가한다.
        foreach (QuestInfoSO questInfoSO in allQuestInfoSOs)
        {
            // 이미 존재하는 ID는 건너뛴다.
            if (idToQuestDic.ContainsKey(questInfoSO.id)) continue;

            // QuestInfoSO의 정보를 베이스로 한 Quest 객체를 생성하고 딕셔너리에 추가한다.
            idToQuestDic.Add(questInfoSO.id, new Quest(questInfoSO));
        }

        return idToQuestDic;
    }

    private void Init()
    {
        // 01) All Quests Initialization
        allQuests = CreateQuestMap();

        // 02) NPC별 퀘스트 각 퀘스트 딕셔너리초기화 (allQuests를 기반으로)
        SortQuestsByNPC();

        // 03) Quest 상태별 각 퀘스트 딕셔너리 초기화 (allQuests를 기반으로)
        SortQuestByQuestState();
    }

    public void ResetQuest(string questID)
    {
        // 특정 퀘스트를 초기화하는 메서드
        if (allQuests.TryGetValue(questID, out Quest quest))
        {
            quest.ResetQuest();

            // 퀘스트 상태별로 다시 분류
            SortQuestByQuestState();
        }
    }

    public bool TryGetQuestByID_NPC(string questID, out Quest quest)
    {
        // 특정 퀘스트 ID로 퀘스트를 찾는 메서드
        if (allQuests.TryGetValue(questID, out quest))
        {
            // 퀘스트가 존재하면 해당 퀘스트를 반환
            return true;
        }
        else
        {
            // 퀘스트가 존재하지 않으면 null 반환
            quest = null;
            return false;
        }
    }

    private void SortQuestsByNPC()
    {
        // 퀘스트를 주는 NPC의 종류별로 퀘스트 분류
        foreach (Quest quest in allQuests.Values)
        {
            switch (quest.QuestNPC)
            {
                case E_NPCList.NPC_Mother:
                    ByNPC.NPC_QuestLists[ByNPC.EnumToString(E_NPCList.NPC_Mother)].Add(quest.QuestInfo.id, quest);
                    break;

                case E_NPCList.NPC_Dian:
                    ByNPC.NPC_QuestLists[ByNPC.EnumToString(E_NPCList.NPC_Dian)].Add(quest.QuestInfo.id, quest);
                    break;

                case E_NPCList.NPC_Nepin:
                    ByNPC.NPC_QuestLists[ByNPC.EnumToString(E_NPCList.NPC_Nepin)].Add(quest.QuestInfo.id, quest);
                    break;

                case E_NPCList.NPC_Rene:
                    ByNPC.NPC_QuestLists[ByNPC.EnumToString(E_NPCList.NPC_Rene)].Add(quest.QuestInfo.id, quest);
                    break;
            }
        }
    }

    public void SortQuestByQuestState()
    {
        // 퀘스트 상태별로 퀘스트를 분류하기 전에 기존 리스트를 초기화 (항상 초기화 후 깔끔한 상태에서 시작)
        ByState.ClearLists();

        // 현재 퀘스트의 상태에 따라 퀘스트 분류
        foreach (Quest quest in allQuests.Values)
        {
            switch (quest.QuestState)
            {
                case E_QuestState.REQUIREMENTS_NOT_MET:
                case E_QuestState.CAN_START:
                    byState.Red_QuestList.Add(quest.QuestInfo.id, quest);
                    break;
                case E_QuestState.IN_PROGRESS:
                case E_QuestState.CAN_FINISH:
                    byState.Yellow_QuestList.Add(quest.QuestInfo.id, quest);
                    break;
                case E_QuestState.FINISHED:
                    byState.Green_QuestList.Add(quest.QuestInfo.id, quest);
                    break;
            }
        }
    }
}