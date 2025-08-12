using UnityEngine;

public static class EnemyFactory
{
    private static EnemyDataLoader enemyDataLoader => GameManager.Instance.DataManager.EnemyDataLoader;

    public static EnemyInstance CreateEnemy(int enemyKey)
    {
        EnemyData data = enemyDataLoader.GetByKey(enemyKey);
        if (data == null)
        {
            Debug.LogError($"[EnemyFactory] EnemyData not found: key={enemyKey}");
            return null;
        }

        return new EnemyInstance(data);
    }
}