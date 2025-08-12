using UnityEngine;

public class QuestRewardSystem : MonoBehaviour
{
    private QuestManager questManager;
    private DialogueEvents dialogueEvents;
    private Party party;

    private void Start()
    {
        // 컴포넌트 그랩
        questManager = GetComponent<QuestManager>();

        // 싱글톤 캐싱
        party = GameManager.Instance.PartyManager.Party;
        dialogueEvents = GameEventsManager.Instance.dialogueEvents;
    }

    public void ClaimRewards(Quest quest, int expRewards, int goldRewards)
    {
        GiveExpRewards(expRewards);
        GiveGoldRewards(goldRewards);

        // 퀘스트가 추가 보상을 가지고 있다면 해당 보상을 지급한다.
        if (quest.QuestInfo.isThereExtraRewards)
        {
            GiveExtraRewards(quest, quest.QuestInfo.extraRewardsType);
        }
    }

    private void GiveExpRewards(int expRewards)
    {
        // 파티원들에게 경험치 보상을 지급
        foreach (var member in party.Members)
        {
            member.GainExp(expRewards);
        }
    }

    private void GiveGoldRewards(int goldRewards)
    {
        // 파티인벤토리에 골드 보상을 지급
        party.CurrencyWallet.AddGold(goldRewards);
    }

    private void GiveExtraRewards(Quest quest, E_ExtraRewardsType extraRewardsType)
    {
        switch(extraRewardsType)
        {
            case E_ExtraRewardsType.GetMember:
                // CharacterFactory를 통해 새로운 멤버를 생성하고 파티에 추가
                CharacterInstance newMember = CharacterFactory.CreateCharacter(quest.QuestInfo.memberKey);
                party.AddMember(newMember);

                DeleteNPCGameObject(quest.QuestInfo.npc);

                dialogueEvents.DialogueFinishedEvent();
                break;

            case E_ExtraRewardsType.GetItem:
                // 아이템을 생성하고 파티 인벤토리에 추가
                ItemInstance newItem = ItemFactory.CreateItem(quest.QuestInfo.itemKey);
                party.Inventory.AddItem(newItem);
                break;
        }
    }

    private void DeleteNPCGameObject(E_NPCList npc)
    {
        // 퀘스트 완료 후 멤버로 추가된 NPC GameObject를 씬에서 제거

        // 씬에서 NPCManager를 찾아 자식 오브젝트로 생성된 모든 NPC GameObject를 가져온다.
        NPC[] npcs = FindFirstObjectByType<NPCManager>().gameObject.GetComponentsInChildren<NPC>();

        foreach (NPC NPC in npcs)
        {
            if (NPC.NPCData.NPCName == npc.ToString())
            {
                // NPC의 이름이 일치하는 경우 해당 NPC GameObject를 제거
                NPCMemoryTracker.MarkNPCAsDestroyed(NPC.NPCData.NPCName, true);
                Destroy(NPC.gameObject);
                return;
            }
        }
    }
}

