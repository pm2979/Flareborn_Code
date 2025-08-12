using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner
{
    // NavMesh 위 랜덤 위치 찾기 (반경 내)
    private static bool GetRandomNavMeshPosition(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPos = center + new Vector3(
                UnityEngine.Random.Range(-range, range),
                0,
                UnityEngine.Random.Range(-range, range));

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 2f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    // EnemyInstance와 프리팹 받아서 씬에 Instantiate, 위치 셋팅, LoadAll 호출
    public static void SpawnEnemy(EnemyInstance instance, GameObject prefab, Vector3 centerPos, float spawnRadius, Action<GameObject> onSpawned)
    {
        if (instance == null || prefab == null)
        {
            Debug.LogError("[EnemySpawner] Invalid parameters for SpawnEnemy");
            onSpawned?.Invoke(null);
            return;
        }

        Vector3 spawnPos;
        if (!GetRandomNavMeshPosition(centerPos, spawnRadius, out spawnPos))
        {
            Debug.LogWarning("[EnemySpawner] Failed to find NavMesh position, spawning at center");
            spawnPos = centerPos;
        }

        GameObject enemyObj = UnityEngine.Object.Instantiate(prefab, spawnPos, Quaternion.identity);

        OverWorldEnemy overworldEnemy = enemyObj.GetComponent<OverWorldEnemy>();
        if (overworldEnemy != null)
        {
            overworldEnemy.Init(instance);
        }
        else
        {
            Debug.LogWarning("[EnemySpawner] OverWorldEnemy component missing");
        }

        onSpawned?.Invoke(enemyObj);
    }
}