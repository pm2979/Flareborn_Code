using UnityEngine;

[System.Serializable]
public class TutorialManager : MonoSingleton<TutorialManager>
{
    [SerializeField] private Tutorial[] tutorials_GetMembers;
    [SerializeField] private Tutorial[] tutorials_Battles;
    [SerializeField] private Tutorial currentTutorial;
    [SerializeField] private PortalManager portalManager;
    [SerializeField] private bool isTutorial_GetTeamMembers_Finished;
    private PlayerInteractionHandler playerInteractionHandler;
    private NPC npc_Rene;
    private bool isReneHidden;
    private bool isFirstLoad = true;


    protected override void OnEnable()
    {
        if (isFirstLoad)
        {
            playerInteractionHandler = FindFirstObjectByType<PlayerInteractionHandler>();
            portalManager = FindFirstObjectByType<PortalManager>();

            // 포탈을 비활성화 한다
            TurnOffPortals();

            // 첫번째 튜토리얼을 시작한다
            currentTutorial = Instantiate(tutorials_GetMembers[0], transform);
        }
    }

    protected void Start()
    {
        // 르네를 찾는다
        FindRene();
        HideRene();
        isReneHidden = true;
    }

    public void Tutorial_GetTeamMembers_Finished()
    {
        // 현재 튜토리얼을 끝낸다
        currentTutorial.FinishTutorial();
        playerInteractionHandler.EmptyCurrentInteractable();

        if (isReneHidden)
        {
            // 르네 NPC를 활성화 한다
            ShowRene();
            isReneHidden = false;
        }


        // 다음 튜토리얼을 시작한다
        if (currentTutorial.TutorialId < tutorials_GetMembers.Length)
        {
            currentTutorial = Instantiate(tutorials_GetMembers[1], transform);
        }
        // 다음 튜토리얼이 없다면 모든 튜토리얼이 끝났다고 표시한다 && 배틀 튜토리얼을 시작한다
        else
        {
            isTutorial_GetTeamMembers_Finished = true;
            AllTutorialFinished();
            TurnOnPortals();

            currentTutorial = Instantiate(tutorials_Battles[0], transform);
        }
    }

    public void Tutorial_Battles_Finished()
    {

    }

    private void AllTutorialFinished()
    {
        // 모든 튜토리얼이 끝났을 때 포탈을 활성화 한다
    }

    private void TurnOffPortals()
    {
        portalManager.gameObject.SetActive(false);
    }

    private void TurnOnPortals()
    {
        portalManager.gameObject.SetActive(true);
    }

    private void FindRene()
    {
        NPC[] npcs = NPCManager.Instance.GetComponentsInChildren<NPC>();
        foreach (NPC npc in npcs)
        {
            if (npc.NPCData.NPCName == "NPC_Rene")
            {
                npc_Rene = npc;
                break;
            }
        }
    }

    private void HideRene()
    {
        npc_Rene.gameObject.SetActive(false);
    }

    private void ShowRene()
    {
        npc_Rene.gameObject.SetActive(true);
    }
}
