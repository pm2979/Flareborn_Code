using UnityEngine;

public class NPC_StateMachine : MonoBehaviour
{
    public enum NPCState { Default, Idle, Patrol, Wander, Talk }
    public NPCState currentState = NPCState.Patrol;
    public NPCState defaultState;

    public NPC_Patrol patrol;
    public NPC_Wander wander;
    public NPC_Talk talk;

    private void Start()
    {
        defaultState = currentState;
        SwitchState(currentState);
    }

    public void SwitchState(NPCState newState)
    {
        currentState = newState;

        patrol.enabled = (newState == NPCState.Patrol);
        wander.enabled = (newState == NPCState.Wander);
        talk.enabled = (newState == NPCState.Talk);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchState(NPCState.Talk);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchState(defaultState);
        }
    }
}
