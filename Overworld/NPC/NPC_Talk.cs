using System.Collections;
using UnityEngine;

public class NPC_Talk : MonoBehaviour
{
    private Rigidbody rb;
    private Animator playerAnim;
    public Animator interactAnim;
    public QuestIcon questIcon;

    private Coroutine speechBubbleCloseCoroutine;
    private WaitForSeconds waitForSpeechBubbleClose;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponentInChildren<Animator>();
        questIcon = GetComponentInChildren<QuestIcon>();
    }

    private void Start()
    {
        waitForSpeechBubbleClose = new WaitForSeconds(interactAnim.GetCurrentAnimatorStateInfo(0).length);
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector2.zero;
        rb.mass = Mathf.Infinity;
        questIcon.gameObject.SetActive(false);
        playerAnim.Play("Idle");
        interactAnim.Play("Open");
    }

    private void OnDisable()
    {
        if (speechBubbleCloseCoroutine != null)
            StopCoroutine(speechBubbleCloseCoroutine);

        speechBubbleCloseCoroutine = StartCoroutine(SpeechBubbleClose());
        rb.mass = 1f;
    }

    private IEnumerator SpeechBubbleClose()
    {
        interactAnim.Play("Close");
        yield return waitForSpeechBubbleClose;
        questIcon.gameObject.SetActive(true);
    }
}
