using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleRewardUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainPanel; // 전체 패널
    [SerializeField] private GameObject characterExpPanel; // 경험치 패널
    [SerializeField] private GameObject itemAndGoldPanel; // 아이템/골드 패널

    [Header("EXP Panel Components")]
    [SerializeField] private CharacterExpEntryUI[] characterExpEntrys;

    [Header("Item/Gold Panel Components")]
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private Button confirmButton; // 나가기 버튼

    [Header("Prefabs")]
    [SerializeField] private ItemSlotUI itemSlotPrefab;

    // 아이템 슬롯 UI를 관리하기 위한 오브젝트 풀
    private List<ItemSlotUI> itemSlotPool = new List<ItemSlotUI>();
    private IconCacheManager iconCacheManager;

    public BattleSystem BattleSystem { get; private set; }

    public void Init(BattleSystem system)
    {
        BattleSystem = system;
        iconCacheManager = GameManager.Instance.IconCacheManager;
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
        mainPanel.SetActive(false);

        foreach (CharacterExpEntryUI slot in characterExpEntrys)
        {
            slot.gameObject.SetActive(false);
        }
    }

    // 보상 시퀀스 전체를 시작하는 메서드
    public void StartRewardSequence(List<CharacterInstance> aliveCharacters, int totalExp, int totalGold, List<ItemInstance> items)
    {
        StartCoroutine(RewardSequenceCoroutine(aliveCharacters, totalExp, totalGold, items));
    }

    private IEnumerator RewardSequenceCoroutine(List<CharacterInstance> aliveCharacters, int totalExp, int totalGold, List<ItemInstance> items)
    {
        mainPanel.SetActive(true);
        characterExpPanel.SetActive(true);
        itemAndGoldPanel.SetActive(false);
        confirmButton.gameObject.SetActive(false);

        // 경험치 획득 애니메이션
        List<Coroutine> runningAnimations = new List<Coroutine>();

        for (int i = 0; i < aliveCharacters.Count; i++)
        {
            if (i >= characterExpEntrys.Length) continue;

            CharacterInstance character = aliveCharacters[i];
            CharacterExpEntryUI entry = characterExpEntrys[i];

            // 애니메이션 시작 전 초기 상태 설정 및 UI 활성화
            entry.SetData(character);
            entry.gameObject.SetActive(true);

            // 실제 데이터 업데이트
            character.GainExp(totalExp);

            Coroutine animCoroutine = StartCoroutine(entry.AnimateExpGain(character, totalExp));
            runningAnimations.Add(animCoroutine);
        }

        // 리스트에 있는 모든 애니메이션 코루틴이 끝날 때까지 대기
        foreach (Coroutine coroutine in runningAnimations)
        {
            yield return coroutine;
        }

        yield return new WaitForSeconds(1f);

        // 아이템 및 골드 보상 표시
        itemAndGoldPanel.SetActive(true);

        expText.text = $"총 경험치: {totalExp}";
        goldText.text = $"골드: {totalGold}";

        foreach (ItemSlotUI slot in itemSlotPool)
        {
            slot.gameObject.SetActive(false);
        }

        // 보상 아이템 리스트를 순회하며 슬롯 설정
        for (int i = 0; i < items.Count; i++)
        {
            ItemSlotUI slot;
            if (i < itemSlotPool.Count)
            {
                // 풀에 비활성 슬롯이 있으면 재사용
                slot = itemSlotPool[i];
            }
            else
            {
                // 슬롯이 부족하면 새로 생성하고 풀에 추가
                slot = Instantiate(itemSlotPrefab, itemContainer);
                slot.InitSlot(iconCacheManager, null); // 새로 생성된 슬롯 초기화
                itemSlotPool.Add(slot);
            }

            slot.SetSlot(items[i]); // 슬롯에 아이템 정보 설정
            slot.gameObject.SetActive(true); // 슬롯 활성화
        }

        confirmButton.gameObject.SetActive(true);
    }

    private void OnConfirmButtonClicked()
    {
        mainPanel.SetActive(false);
        BattleSystem.ReturnToOverworld();
    }

    public void Disabled()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveListener(OnConfirmButtonClicked);
        }
    }
}
