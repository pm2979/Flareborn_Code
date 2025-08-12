using UnityEngine;

public class Tutorial_02 : Tutorial
{
    private TutorialEvents tutorialEvents;
    [SerializeField] private GameObject arrowPrefab;
    public GameObject npcRene;

    protected override void Start()
    {
        base.Start();

        tutorialEvents = GameEventsManager.Instance.tutorialEvents;

        // 르네 NPC를 찾음
        FindRene();

        // 르네 NPC에 화살표를 생성
        Vector3 arrowSpawnPos = npcRene.transform.position + new Vector3(0, 3f, 0);
        Instantiate (arrowPrefab, arrowSpawnPos, Quaternion.identity, npcRene.transform);

        // 튜토리얼 텍스트 설정
        if (tutorialUI != null)
        {
            tutorialUI.SetTutorialText(SetTutorialText_Dian());
        }
    }

    private void FindRene()
    {
        NPC[] npcs = NPCManager.Instance.GetComponentsInChildren<NPC>();

        foreach (NPC npc in npcs)
        {
            if (npc.NPCData.NPCName == "NPC_Rene")
            {
                npcRene = npc.gameObject;
                break;
            }
        }
    }

    public override void FinishTutorial()
    {
        Destroy(gameObject);
    }

    private string SetTutorialText_Dian()
    {
        return "- 르네 찾아가기 (동료영입)";
    }
}
