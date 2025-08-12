using System.Collections;
using UnityEngine;

public class NPC_Wander : MonoBehaviour
{
    [Header("Wander Area")]
    public float wanderWidth = 5f;
    public float wanderHeight = 5f;
    public Vector3 startingPosition;

    public float pauseDuration = 1.5f;
    public float speed = 2f;
    public Vector3 target;

    private Rigidbody rb;
    private Animator anim;
    private WaitForSeconds waitForPause;
    private bool isPaused = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        waitForPause = new WaitForSeconds(pauseDuration);
    }

    private void OnEnable()
    {
        target = GetRandomTarget();
    }

    private void Update()
    {
        if (isPaused)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        } 
        
        var pos = new Vector3(transform.position.x, 0, transform.position.z);

        //if (Vector3.Distance(pos, target) < 0.1f)
        if ((pos - target).sqrMagnitude < 0.1f * 0.1f)
        {
            StartCoroutine(PauseAndPickNewDestination());
        }

        Vector3 direction = (target - pos).normalized;
        rb.linearVelocity = direction * speed;

        SetWalkAnim(direction); // 이동 애니메이션 설정
    }

    IEnumerator PauseAndPickNewDestination()
    {
        isPaused = true;
        anim.Play("Idle");
        yield return waitForPause;

        target = GetRandomTarget();
        isPaused = false;
    }

    private void SetWalkAnim(Vector3 direction)
    {
        float x = Mathf.Abs(direction.x);
        float z = Mathf.Abs(direction.z);

        if (direction.x < 0)
        {
            anim.Play("Walk_Left");
        }
        else if (direction.x > 0)
        {
            anim.Play("Walk_Right");
        }
    }

    private Vector3 GetRandomTarget()
    {
        float halfWidth = wanderWidth / 2f;
        float halfHeight = wanderHeight / 2f;
        int edge = Random.Range(0, 4);

        return edge switch
        {
            0 => new Vector3(startingPosition.x - halfWidth, 0, Random.Range(startingPosition.z - halfHeight, startingPosition.z + halfHeight)),  // 왼쪽
            1 => new Vector3(startingPosition.x + halfWidth, 0, Random.Range(startingPosition.z - halfHeight, startingPosition.z + halfHeight)),  // 오른쪽
            2 => new Vector3(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), 0, startingPosition.z - halfHeight),    // 아래쪽
            _ => new Vector3(Random.Range(startingPosition.x - halfWidth, startingPosition.x + halfWidth), 0, startingPosition.z + halfHeight),    // 위쪽
        };
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(startingPosition, new Vector3(wanderWidth, 0.1f, wanderHeight));
    }
}
