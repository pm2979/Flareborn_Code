using UnityEngine;

public class EnemyInitializer : MonoBehaviour
{
    [SerializeField] private int enemyID;

    private void Start()
    {
        var instance = EnemyFactory.CreateEnemy(enemyID);
        if (instance == null) return;

        var controller = GetComponent<OverWorldEnemy>();
        controller?.Init(instance);

        var enemyManager = GameManager.Instance.EnemyManager;
        if (enemyManager != null)
        {
            enemyManager.AddEnemyInstance(instance);
        }
        else
        {
            Debug.LogError("EnemyInitializer - EnemyManager를 찾을 수 없음");
        }
    }
}
