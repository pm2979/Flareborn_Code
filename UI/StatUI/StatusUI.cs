using UnityEngine;

public class StatusUI : BaseMenuUI
{
    [Header("UI 구성 요소")]
    [SerializeField] private CharacterStatusUIPanel characterStatusPanel; // 상세 정보를 표시할 단 하나의 패널

    private PartyManager partyManager;

    public void Init(CharacterStatUI[] characterStatUIs)
    {
        for(int i = 0; i < characterStatUIs.Length; i++)
        {
            characterStatUIs[i].OnCharacterSelected += DisplayCharacterDetails;
        }
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            partyManager = GameManager.Instance.PartyManager;
            characterStatusPanel.DisplayCharacterInfo(partyManager.Party.Members[0]);
        }
    }

    // 캐릭터 상세 정보 표시
    private void DisplayCharacterDetails(CharacterInstance character)
    {
        if (characterStatusPanel != null && gameObject.activeInHierarchy)
        {
            characterStatusPanel.DisplayCharacterInfo(character);
        }
    }


    protected override MenuState GetMenuState()
    {
        return MenuState.Status;
    }
}
