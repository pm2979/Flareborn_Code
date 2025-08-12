using System.Collections.Generic;

public class VictoryState : IBattleState // 승리 상태
{
    public void EnterState(BattleSystem system)
    {
        system.SyncCharacterDataFromBattle();

        GiveRewards(system);
    }

    public void ExitState(BattleSystem system) { }


    private void GiveRewards(BattleSystem system)
    {
        int totalExp = 0;
        int totalGold = 0;
        List<ItemInstance> totalItems = new();

        Party party = system.partyManager.Party;
        List<EnemyInstance> defeatedEnemies = system.enemyManager.GetCurrentEnemies();
        List<CharacterInstance> aliveMembers = party.GetAliveMembers();

        foreach (EnemyInstance enemy in defeatedEnemies)
        {
            DropResult drop = enemy.ProcessDrops();
            totalExp += drop.Exp;
            totalGold += drop.Gold;
            foreach (int itemKey in drop.ItemKeys)
            {
                ItemInstance item = ItemFactory.CreateItem(itemKey);
                if (item != null) totalItems.Add(item);
            }
        }

        // 골드와 아이템은 즉시 인벤토리에 지급
        party.CurrencyWallet.AddGold(totalGold);
        foreach (ItemInstance item in totalItems)
        {
            party.Inventory.AddItem(item);
        }

        Logger.Log($"전투 보상 지급 - Exp: {totalExp}, Gold: {totalGold}, Items: {totalItems.Count}");


        system.OnBattleRewarded(aliveMembers, totalExp, totalGold, totalItems);
    }
}
