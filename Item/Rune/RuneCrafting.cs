using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using static DesignEnums;

// 각 재료의 정보 정의
[System.Serializable]
public class CraftingMaterialInfo
{
    public string materialName; // 인스펙터 식별용 이름
    public int materialId;
    [Tooltip("이 재료가 제작 확률에 기여하는 가치 점수")]
    public int pointValue;
}

// 점수 구간별 확률 정의
[System.Serializable]
public class ProbabilityTier
{
    [Tooltip("이 확률이 적용되기 위한 최소 점수")]
    public int minPoints;
    [Range(0, 100)] public int epicChance;
    [Range(0, 100)] public int rareChance;
}

public class RuneCrafting : MonoBehaviour
{
    [Header("재료 정보 설정")]
    [SerializeField] private List<CraftingMaterialInfo> materialDatabase;

    [Header("점수별 확률 설정 (점수 낮은 순으로 정렬)")]
    [SerializeField] private List<ProbabilityTier> probabilityTiers;

    private bool CanCraft(List<ItemInstance> selectedMaterials)
    {
        // 각 아이템 ID별로 몇 개가 필요한지 계산
        Dictionary<int, int> requiredItems = selectedMaterials.GroupBy(item => item.ItemKey).ToDictionary(group => group.Key, group => group.Count());

        Inventory inventory = GameManager.Instance.PartyManager.Party.Inventory;

        foreach (KeyValuePair<int, int> req in requiredItems)
        {
            // 인벤토리에서 해당 아이템 타입의 모든 아이템을 가져옴
            IEnumerable<ItemInstance> itemsInInventory = inventory.GetItems(ItemType.General).Where(item => item.ItemKey == req.Key);

            int totalAmount = itemsInInventory.Sum(item => item.CurrentStack);

            if (totalAmount < req.Value)
            {
                return false;
            }
        }
        return true;
    }

    public ItemInstance CraftRune(List<ItemInstance> selectedMaterials)
    {
        // 제작 가능 여부 확인
        if (!CanCraft(selectedMaterials))
        {
            Debug.LogError("제작 실패: 재료가 부족합니다.");
            return null;
        }

        var inventory = GameManager.Instance.PartyManager.Party.Inventory;

        // 재료 소모
        foreach (ItemInstance material in selectedMaterials)
        {
            inventory.RemoveItem(material, 1);
        }

        // 총 가치 점수 계산
        int totalPoints = CalculateTotalPoints(selectedMaterials);

        // 총 점수에 따른 확률 결정 및 룬 생성
        ProbabilityTier currentTier = GetProbabilityTier(totalPoints);
        ItemInstance newRune = GenerateRandomRune(currentTier);

        // 결과물 인벤토리에 추가
        inventory.AddItem(newRune);
        Debug.Log($"룬 제작 성공! (총점: {totalPoints}) - {newRune.ItemName()} 획득!");

        return newRune;
    }

    public (int epicChance, int rareChance) PreviewProbabilities(List<ItemInstance> selectedMaterials)
    {
        int totalPoints = CalculateTotalPoints(selectedMaterials);
        ProbabilityTier tier = GetProbabilityTier(totalPoints);
        return (tier.epicChance, tier.rareChance);
    }

    private int CalculateTotalPoints(List<ItemInstance> materials)
    {
        int totalPoints = 0;
        foreach (var material in materials)
        {
            CraftingMaterialInfo info = materialDatabase.Find(db => db.materialId == material.ItemKey);
            if (info != null)
            {
                totalPoints += info.pointValue;
            }
        }
        return totalPoints;
    }

    private ProbabilityTier GetProbabilityTier(int totalPoints)
    {
        // 점수가 낮은 순으로 정렬
        // 가장 높은 등급을 찾음
        for (int i = probabilityTiers.Count - 1; i >= 0; i--)
        {
            if (totalPoints >= probabilityTiers[i].minPoints)
            {
                return probabilityTiers[i];
            }
        }
        // 어떤 구간에도 해당되지 않으면 기본값 반환
        return probabilityTiers.FirstOrDefault() ?? new ProbabilityTier();
    }

    private ItemInstance GenerateRandomRune(ProbabilityTier tier) // 능력치 부여
    {
        int randomRuneId = Random.Range(3001, 3009);
        ItemInstance item = ItemFactory.CreateItem(randomRuneId);

        RuneData data = item.RuneData;
        RuneStat runeStone = item.RuneStat;
        runeStone.AbilityType = data.AbilityType;

        int chance = Random.Range(0, 100);
        if (chance < tier.epicChance) runeStone.RuneType = RuneValue.Epic;
        else if (chance < tier.epicChance + tier.rareChance) runeStone.RuneType = RuneValue.Rare;
        else runeStone.RuneType = RuneValue.Normal;

        // 등급에 따른 능력치 부여
        switch (runeStone.RuneType)
        {
            case RuneValue.Normal:
                runeStone.Value = Random.Range((int)(data.NormalValue * (1f - data.MedianModifier)), (int)(data.NormalValue * (1f + data.MedianModifier)));
                break;
            case RuneValue.Rare:
                runeStone.Value = Random.Range((int)(data.RareValue * (1f - data.MedianModifier)), (int)(data.RareValue * (1f + data.MedianModifier)));
                break;
            case RuneValue.Epic:
                runeStone.Value = Random.Range((int)(data.EpicValue * (1f - data.MedianModifier)), (int)(data.EpicValue * (1f + data.MedianModifier)));
                break;
        }

        return item;
    }
}