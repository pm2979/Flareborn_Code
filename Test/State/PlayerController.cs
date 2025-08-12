using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private int stepsInGrass;
    [SerializeField] private int minStepsToEncounter;
    [SerializeField] private int maxStepsToEncounter;

    private PlayerControls playerControls;
    private Rigidbody rb;
    private Vector3 movement;
    private bool movingInGrass;
    private float stepTimer;
    private int stepsToEncounter;
    private PartyManager partyManager;
    private Vector3 scale;

    /// ------------------------------------------ 스테이트머신 추가 ----------------- ///
    /// 각 항목은 인스펙터에서 숨겼습니다.
    private PlayerStateMachine stateMachine;

    // 스테이트 머신에서 쓰일 퍼블릭 플레이어 콘트롤
    [HideInInspector]
    public PlayerControls _playerControls { get { return playerControls; } private set { _playerControls = playerControls; } }

    // 스테이트 머신에서 쓰일 퍼블릭 리지드바디
    [HideInInspector]
    public Rigidbody _rb { get { return rb; } private set { _rb = rb; } }

    // 스테이트 머신에서 쓰일 퍼블릭 애니메이션
    [HideInInspector]
    public Animator _anim { get { return anim; } private set { _anim = anim; } }

    // 스테이트 머신에서 쓰일 퍼블릭 스프라이트
    [HideInInspector]
    public SpriteRenderer _playerSprite { get { return playerSprite; } private set { _playerSprite = playerSprite; } }

    // 스테이트 머신에서 쓰일 퍼블릭 스케일
    [HideInInspector]
    public Vector3 _scale { get { return scale; } private set { _scale = scale; } }
    /// --------------------------------------------------------------------------- ///

    [HideInInspector]
    public TerrainDetector terrainDetector; // 땅 착지 확인

    //private const string IS_WALK_PARAM = "IsWalk";
    [Header("Animations")]
    [SerializeField]
    public PlayerAnimationData animationData; // 애니메이션데이터 스크립트 추가
    private const string BATTLE_SCENE = "BattleScene";
    private const float TIME_PER_STEP = 0.5f;

    private void Awake()
    {
        playerControls = new PlayerControls();

        stateMachine = new PlayerStateMachine(this);
        stateMachine.ChangeState(stateMachine.idleState);
        animationData.Initialize();

        CalculateStepsToNextEncounter();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        partyManager = GameManager.Instance.PartyManager;
        if (partyManager.GetPosition() != Vector3.zero)// if we have a position saved
        {
            transform.position = partyManager.GetPosition();// move the player
        }
        if (terrainDetector == null)
        {
            terrainDetector = GetComponent<TerrainDetector>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.HandleInput(); // 스테이트 머신 핸들인풋
        stateMachine.Update(); // 스테이트 머신 업데이트
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate(); // 스테이트 머신 피직스 업데이트

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1, grassLayer);
        movingInGrass = colliders.Length != 0 && movement != Vector3.zero;

        if (movingInGrass == true)
        {
            stepTimer += Time.fixedDeltaTime;
            if (stepTimer > TIME_PER_STEP)
            {
                stepsInGrass++;
                stepTimer = 0;

                if (stepsInGrass >= stepsToEncounter)
                {
                    partyManager.SetPosition(transform.position);
                    SceneManager.LoadScene(BATTLE_SCENE);
                }

                // check to see if we have reached an encounter 
                // ->change the scene
            }
        }
    }

    private void CalculateStepsToNextEncounter()
    {
        stepsToEncounter = Random.Range(minStepsToEncounter, maxStepsToEncounter);
    }

    public void SetOverworldVisuals(Animator animator, SpriteRenderer spriteRenderer, Vector3 playerScale)
    {
        anim = animator;
        playerSprite = spriteRenderer;
        scale = playerScale;
    }

}
