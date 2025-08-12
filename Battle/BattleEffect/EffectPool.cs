using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EffectPool
{
    private readonly Dictionary<string, Queue<Effect>> pool = new Dictionary<string, Queue<Effect>>();
    private Transform root; // 생성된 이펙트가 소속될 부모 트랜스폼

    public void Initialize(Transform parent)
    {
        root = parent;
    }

    // 이펙트를 미리 로드하여 풀에 준비
    public async Task Preload(string key, int count)
    {
        if (root == null)
        {
            return;
        }

        if (!pool.ContainsKey(key))
            pool[key] = new Queue<Effect>();

        for (int i = 0; i < count; i++)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(key, root);
            GameObject effectObj = await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Effect effect = effectObj.GetComponent<Effect>();
                if (effect != null)
                {
                    effect.gameObject.SetActive(false);
                    pool[key].Enqueue(effect);
                }
                else
                {
                    Logger.Log($"'{key}' 프리팹에 Effect 컴포넌트가 없습니다.");
                    Addressables.ReleaseInstance(effectObj); // 컴포넌트가 없으면 즉시 파괴
                }
            }
        }
    }

    // 풀에서 이펙트를 가져오거나, 없으면 새로 생성하여 반환
    public async Task<Effect> GetEffect(string key)
    {
        // 풀에 사용 가능한 이펙트 사용
        if (pool.TryGetValue(key, out var queue) && queue.Count > 0)
        {
            Effect effect = queue.Dequeue();
            effect.gameObject.SetActive(true);
            return effect;
        }

        // 풀에 없다면 새로 생성
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(key, root);
        GameObject newEffectObj = await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Effect newEffect = newEffectObj.GetComponent<Effect>();
            if (newEffect != null)
            {
                return newEffect;
            }
            else
            {
                Logger.Log($"'{key}' 프리팹에 Effect 컴포넌트가 없습니다.");
                Addressables.ReleaseInstance(newEffectObj);
            }
        }

        return null;
    }

    // 사용한 이펙트를 풀에 반환
    public void ReturnEffect(string key, Effect effect)
    {
        if (effect == null) return;

        if (!pool.ContainsKey(key))
        {
            Logger.Log($"'{key}'에 해당하는 풀이 없습니다. 이펙트를 새로 생성하여 풀에 추가합니다.");
            pool[key] = new Queue<Effect>();
        }

        // 반환 시 부모를 다시 root로 지정하여 관리
        effect.transform.SetParent(root);
        effect.gameObject.SetActive(false);
        pool[key].Enqueue(effect);
    }

    public void Clear()
    {
        foreach (Queue<Effect> queue in pool.Values)
        {
            while (queue.Count > 0)
            {
                Effect effect = queue.Dequeue();

                Addressables.ReleaseInstance(effect.gameObject);
            }
        }
        pool.Clear();
    }
}