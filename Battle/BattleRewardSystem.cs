using System.Collections.Generic;
using UnityEngine;

public class BattleRewardSystem
{
    public void ProcessBattleRewards(List<EnemyInstance> defeatedEnemies, Party party)
    {
        int totalExp = 0;
        int totalGold = 0;
        List<int> totalItemKeys = new();

        foreach (var enemy in defeatedEnemies)
        {
            var drop = enemy.ProcessDrops();
            totalExp += drop.Exp;
            totalGold += drop.Gold;
            totalItemKeys.AddRange(drop.ItemKeys);
        }

        // 경험치 지급 (생존한 파티원)
        var aliveMembers = party.GetAliveMembers();
        foreach (var member in aliveMembers)
        {
            member.GainExp(totalExp); // 각 멤버에게 총 경험치 지급
        }

        // 골드 지급
        party.CurrencyWallet.AddGold(totalGold);

        // 아이템 지급
        foreach (int itemKey in totalItemKeys)
        {
            var item = ItemFactory.CreateItem(itemKey);
            if (item != null)
            {
                party.Inventory.AddItem(item);
            }
            else
            {
                Debug.LogWarning($"[BattleRewardSystem] 아이템 생성 실패: key = {itemKey}");
            }
        }

        Debug.Log($"전투 보상 지급 완료 - Exp: {totalExp}, Gold: {totalGold}, Items: {totalItemKeys.Count}");
    }
}