using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int[] allEnemies;
    [SerializeField] private List<EnemyInstance> currentEnemies = new List<EnemyInstance>();

    private EnemyDataLoader enemyDataLoader;

    public void Init()
    {
        enemyDataLoader = GameManager.Instance.DataManager.EnemyDataLoader;
    }

    public void AddEnemyInstance(EnemyInstance enemy)
    {
        if (!currentEnemies.Contains(enemy))
        {
            currentEnemies.Add(enemy);
        }
    }

    public void GenerateEnemiesByEncounter(List<int> enemyIDs)
    {
        currentEnemies.Clear();
        foreach (var id in enemyIDs)
        {
            GenerateEnemyByName(id);
        }
    }

    private void GenerateEnemyByName(int enemyKey)
    {
        for (int i = 0; i < allEnemies.Length; i++)
        {
            if (enemyKey == allEnemies[i])
            {
                EnemyInstance newEnemy = new EnemyInstance(enemyDataLoader.GetByKey(enemyKey));
                currentEnemies.Add(newEnemy);
                Debug.Log($"[EnemyManager] 적 인스턴스 생성 및 추가: 키={enemyKey}, 이름={newEnemy.Name}");
            }
        }
    }

    public List<EnemyInstance> GetCurrentEnemies()
    {
        return currentEnemies;
    }
}
