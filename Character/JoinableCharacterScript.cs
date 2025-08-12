using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinableCharacterScript : MonoBehaviour
{
    public CharacterInstance MemberToJoin { get; private set; }
    public PartyManager PartyManager { get; private set; }

    [SerializeField] private GameObject interactPrompt;

    private bool initialized = false;

    public void Init(CharacterInstance character)
    {
        PartyManager = GameManager.Instance.PartyManager;

        if (character == null || character.CharacterData == null)
        {
            Debug.LogWarning($"{gameObject.name} - LoadAll 호출 시 null character 또는 CharacterData");
            return;
        }

        MemberToJoin = character;
        initialized = true;

        CheckIfJoined();
    }

    public void ShowInteractPrompt(bool showPrompt)
    {
        interactPrompt?.SetActive(showPrompt);
    }

    public void CheckIfJoined()
    {
        if (!initialized || MemberToJoin == null || MemberToJoin.CharacterData == null)
            return;

        if (PartyManager == null) return;

        var currentParty = PartyManager.GetCurrentParty();
        foreach (var member in currentParty)
        {
            if (member.ID == MemberToJoin.ID)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        // 파티에 없으면 AI 비활성화
        var followAI = GetComponent<MemberFollowAI>();
        if (followAI != null)
        {
            followAI.enabled = false;
        }
    }

    public void Interact()
    {
        if (MemberToJoin == null)
        {
            Debug.LogError("Interact 호출 시 MemberToJoin이 null 입니다!");
            return;
        }

        PartyManager.AddMemberByKey(MemberToJoin.ID);
        gameObject.SetActive(false);
    }
}