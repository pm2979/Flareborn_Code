using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!gameObject.activeSelf) return;

        OverWorldEnemy enemy = other.GetComponentInParent<OverWorldEnemy>();
        if (enemy != null && enemy.enemyInstance != null)
        {
            EncounterSystem.Instance?.TriggerEncounter(enemy.enemyInstance, enemy.gameObject);
        }
    }
}
