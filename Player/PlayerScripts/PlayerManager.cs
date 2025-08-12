using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    [Header("Player Prefab")]
    [SerializeField] public GameObject playerPrefab;

    [Header("Player Components")]
    [SerializeField] public PlayerMovement playerMovement;

    [SerializeField] private GameObject joinPopup;
    [SerializeField] private TextMeshProUGUI joinPopupText;

    private bool infrontOfPartyMember;
    private GameObject joinableMember;
    private PlayerControls playerControls;
    private List<GameObject> overworldCharacters = new List<GameObject>();

    private const string PARTY_JOINED_MESSAGE = " Joined The Party!";
    private const string NPC_JOINABLE_TAG = "NPCJoinable";

    private PartyManager partyManager;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        partyManager = GameManager.Instance.PartyManager;

        playerMovement = GetComponent<PlayerMovement>();
    }
    void Start()
    {
        playerControls.Player.Interact.performed += _ => Interact();
        SpawnOverworldMembers();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Interact()
    {
        if (infrontOfPartyMember && joinableMember != null)
        {
            var joinableScript = joinableMember.GetComponent<JoinableCharacterScript>();
            var character = joinableScript?.MemberToJoin;
            if (character == null)
            {
                Debug.LogError("MemberToJoin is null on interact!");
                return;
            }

            MemberJoined(character);
            infrontOfPartyMember = false;
            joinableMember = null;
        }
    }

    private void MemberJoined(CharacterInstance character)
    {
        if (partyManager == null)
        {
            Debug.LogError("PartyManager not found!");
            return;
        }

        partyManager.AddMemberByKey(character.ID);

        Debug.Log($"Member joined: {character.Name}, ID: {character.ID}");

        if (joinableMember != null)
        {
            var joinableScript = joinableMember.GetComponent<JoinableCharacterScript>();
            if (joinableScript != null)
            {
                joinableScript.ShowInteractPrompt(false);
                joinableScript.enabled = false; // interaction off
            }
            joinableMember.SetActive(false); // hide
        }

        joinPopup.SetActive(true);
        joinPopupText.text = character.Name + PARTY_JOINED_MESSAGE;

        SpawnOverworldMembers();
    }

    public void SpawnOverworldMembers()
    {
        // 기존 동료 제거
        foreach (var c in overworldCharacters)
        {
            Destroy(c);
        }
        overworldCharacters.Clear();

        if (partyManager == null) return;

        IReadOnlyList<CharacterInstance> currentParty = partyManager.GetCurrentParty();

        // 1번째 캐릭터는 플레이어이므로 스킵 (index == 0)
        for (int i = 1; i < currentParty.Count; i++)
        {
            int index = i;
            Vector3 spawnPos = transform.position;
            spawnPos.x -= index;

            currentParty[index].LoadCharacterOverworldPrefab(this, spawnPos, (characterGO) =>
            {
                if (characterGO == null) return;

                var joinable = characterGO.GetComponent<JoinableCharacterScript>();
                if (joinable != null)
                {
                    joinable.Init(currentParty[index]);
                }

                // 플레이어 외 동료들은 AI로 따라오게 설정
                var ai = characterGO.GetComponent<MemberFollowAI>();
                if (ai != null)
                {
                    ai.SetFollowDistance(index);
                }

                overworldCharacters.Add(characterGO);
            });
        }
    }

    public GameObject CreateCharacter(GameObject obj, Vector3 pos)
    {
        return Instantiate(obj, pos, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(NPC_JOINABLE_TAG))
        {
            //enable our prompt
            infrontOfPartyMember = true;
            joinableMember = other.gameObject;
            var joinable = joinableMember.GetComponent<JoinableCharacterScript>();
            if (joinable != null)
            {
                joinable.ShowInteractPrompt(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(NPC_JOINABLE_TAG))
        {
            //disable our prompt
            infrontOfPartyMember = false;

            if (joinableMember != null)
            {
                var joinable = joinableMember.GetComponent<JoinableCharacterScript>();
                if (joinable != null)
                {
                    joinable.ShowInteractPrompt(false);
                }
            }

            joinableMember = null;
        }
    }
}
