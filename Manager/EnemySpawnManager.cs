using System.Collections;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public Transform spawnCenter;
    public float spawnRadius = 3f;
    public int totalSpawnCount = 1;
    public bool autoSpawnOnStart = true;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => DungeonManager.Instance.CurrentDungeon != null);

        var dungeon = DungeonManager.Instance.CurrentDungeon;

        if (dungeon.Data.IsEntrance)
        {
            Debug.Log("입구층이므로 몬스터를 스폰하지 않습니다.");
            yield break;
        }

        if (dungeon.Data.IsBoss)
        {
            Debug.Log("보스층입니다. 보스만 스폰합니다.");
            int bossID = dungeon.SpawnableEnemies.Count > 0 ? dungeon.SpawnableEnemies[0] : -1;
            if (bossID >= 0)
            {
                var instance = EnemyFactory.CreateEnemy(bossID);
                if (instance != null)
                {
                    instance.LoadOverworldPrefab(Vector3.zero, prefab =>
                    {
                        if (prefab == null) return;
                        EnemySpawner.SpawnEnemy(instance.Clone(), prefab, spawnCenter.position, 0f, null);
                    });
                }
            }
            yield break;
        }

        if (autoSpawnOnStart)
            SpawnRandomEnemies();
    }

    public void SpawnRandomEnemies()
    {
        var dungeon = DungeonManager.Instance.CurrentDungeon;
        var spawnableIDs = dungeon.SpawnableEnemies;

        for (int i = 0; i < totalSpawnCount; i++)
        {
            int rand = Random.Range(0, spawnableIDs.Count);
            var enemyID = spawnableIDs[rand];
            var instance = EnemyFactory.CreateEnemy(enemyID);
            if (instance == null) continue;

            instance.LoadOverworldPrefab(Vector3.zero, prefab =>
            {
                if (prefab == null) return;
                EnemySpawner.SpawnEnemy(instance.Clone(), prefab, spawnCenter.position, spawnRadius, null);
            });
        }
    }
}