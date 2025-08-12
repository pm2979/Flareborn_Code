using UnityEngine;

public class CharacterInitializer : MonoBehaviour
{
    [SerializeField] private int characterID;

    void Start()
    {
        if (!CharacterFactory.IsInitialized)
        {
            Debug.LogError("CharacterFactory가 아직 초기화되지 않았습니다.");
            return;
        }


        var instance = CharacterFactory.CreateCharacter(characterID);
        if (instance == null) return;

        var joinable = GetComponent<JoinableCharacterScript>();
        joinable?.Init(instance);
    }
}
