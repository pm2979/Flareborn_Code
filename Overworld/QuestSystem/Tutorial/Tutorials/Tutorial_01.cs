using UnityEngine;

public class Tutorial_01 : Tutorial
{
    private TutorialEvents tutorialEvents;
    [SerializeField] private GameObject arrowPrefab;
    public GameObject npcDian;

    protected override void Start()
    {
        base.Start();

        tutorialEvents = GameEventsManager.Instance.tutorialEvents;

        // 디안 NPC를 찾음
        FindDian();

        // 디안 NPC에 화살표를 생성
        Vector3 arrowSpawnPos = npcDian.transform.position + new Vector3(0, 3f, 0);
        Instantiate (arrowPrefab, arrowSpawnPos, Quaternion.identity, npcDian.transform);

        // 튜토리얼 텍스트 설정
        if (tutorialUI != null)
        {
            tutorialUI.SetTutorialText(SetTutorialText_Dian());
        }
    }

    private void FindDian()
    {
        NPC[] npcs = NPCManager.Instance.GetComponentsInChildren<NPC>();

        foreach (NPC npc in npcs)
        {
            if (npc.NPCData.NPCName == "NPC_Dian")
            {
                npcDian = npc.gameObject;
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
        return "- 디안 찾아가기 (동료영입)";
    }
}
