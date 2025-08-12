using UnityEngine;

public class Tutorial_03 : Tutorial
{
    private TutorialEvents tutorialEvents;
    private PlayerInteractionHandler playerInteractionHandler;
    [SerializeField] private GameObject arrowPrefab;
    private GameObject instantiatedArrowPrefab;
    public GameObject toForestPortal;

    protected override void Start()
    {
        base.Start();

        tutorialEvents = GameEventsManager.Instance.tutorialEvents;
        playerInteractionHandler = FindFirstObjectByType<PlayerInteractionHandler>();

        // 포탈을 찾음
        FindForestPortal();

        // 포탈에 화살표 생성
        Vector3 arrowSpawnPos = toForestPortal.transform.position + new Vector3(0, 3f, 0);
        Vector3 arrowScale = new Vector3 (0.37f, 0.37f, 0.37f);
        instantiatedArrowPrefab = Instantiate (arrowPrefab, arrowSpawnPos, Quaternion.identity, toForestPortal.transform);

        // 화살표 크기 설정
        instantiatedArrowPrefab.transform.localScale = arrowScale;

        // 튜토리얼 텍스트 설정
        if (tutorialUI != null)
        {
            tutorialUI.SetTutorialText(SetTutorialText_ToForest());
        }
    }

    private void Update()
    {
        if (playerInteractionHandler == null) return;

        IInteractable portal = toForestPortal.GetComponent<Portal_OnPress>();
        if (playerInteractionHandler.CurrentInteractable == portal && Input.GetKeyDown(KeyCode.E))
        {
            FinishTutorial();
        }
    }

    private void FindForestPortal()
    {
        PortalManager portalManager = FindFirstObjectByType<PortalManager>();

        toForestPortal = portalManager.GetPortalById("ToForest").gameObject;
    }

    public override void FinishTutorial()
    {
        Destroy(instantiatedArrowPrefab);
        Destroy(gameObject);
    }

    private string SetTutorialText_ToForest()
    {
        return "- 숲 외곽으로 이동하기";
    }
}
