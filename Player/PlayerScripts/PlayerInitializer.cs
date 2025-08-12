using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private int characterID;  // 캐릭터 ID만 저장

    private void Start()
    {
        Init(characterID);
    }

    public void Init(int id)
    {
        characterID = id;

        // 캐릭터 데이터를 팩토리에서 생성해서 가져옴
        var characterInstance = CharacterFactory.CreateCharacter(id);
        if (characterInstance == null)
        {
            Debug.LogError($"PlayerInitializer - 캐릭터 데이터를 찾을 수 없음 (ID: {id})");
            return;
        }

        // GameManager의 PartyManager에 추가
        var partyManager = GameManager.Instance.PartyManager;
        if (partyManager != null)
        {
            partyManager.AddMemberByKey(id);
        }
        else
        {
            Debug.LogError("PlayerInitializer - PartyManager를 찾을 수 없음");
        }
    }
}