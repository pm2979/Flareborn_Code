using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static DesignEnums;

public class EffectManager : MonoBehaviour
{
    private EffectPool pool;
    private Transform effectRoot;

    [Header("Root Transforms")]
    [SerializeField] private Transform damageTextRoot;
    [SerializeField] private Camera _camera;

    [Header("Pool & Preload Settings")]
    [SerializeField] private List<string> preloadEffectKeys;
    [SerializeField] private int preloadCount = 3;

    [Header("Range Skill Spawn Transforms")]
    [SerializeField] private Transform enemyTargetSkillSpawnPoint;
    [SerializeField] private Transform partyTargetSkillSpawnPoint;

    public async void Init()
    {
        pool = new EffectPool();

        GameObject root = new GameObject("EffectRoot");
        root.transform.SetParent(transform);
        effectRoot = root.transform;

        // 풀을 초기화
        pool.Initialize(effectRoot);

        // 이펙트 미리 로딩
        foreach (string key in preloadEffectKeys)
        {
            await pool.Preload(key, preloadCount);
        }
    }

    // 이펙트 재생
    public async Task PlayEffectAsync(string key, BattleEntities caster, EffectSpawn spawn, IEnumerable<BattleEntities> targets = null, Action onImpact = null)
    {
        // 풀에서 가져오기
        Effect effect = await pool.GetEffect(key);
        if (effect == null)
        {
            Debug.LogError($"{key}에 해당하는 이펙트 프리팹 또는 Effect 컴포넌트가 없습니다.");
            return;
        }

        effect.transform.SetParent(effectRoot);

        // 이펙트 타입에 따라 위치와 회전 설정
        Vector3 spawnPosition = GetSpawnPosition(caster, spawn, targets);
        effect.transform.position = spawnPosition;
        effect.transform.rotation = caster.IsPlayer ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

        // Effect 타입에 따라 분기 처리 및 재생
        Task effectTask;

        effectTask = effect.PlayEffect(onImpact);

        // 이펙트 재생이 완료될 때까지 대기
        await effectTask;

        // 이펙트 사용이 끝나면 풀에 반환
        pool.ReturnEffect(key, effect);
    }

    private Vector3 GetSpawnPosition(BattleEntities caster, EffectSpawn spawn, IEnumerable<BattleEntities> targets)
    {
        // 위치를 반환
        switch (spawn)
        {
            case EffectSpawn.Target:
                using (IEnumerator<BattleEntities> enumerator = targets.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        return enumerator.Current.BattleVisuals.GetPoint(PointType.Hit);
                    }
                }
                return caster.BattleVisuals.GetPoint(PointType.Attack);
            case EffectSpawn.Caster:
                return caster.BattleVisuals.GetPoint(PointType.Attack);
            case EffectSpawn.Range:
                return caster.IsPlayer ? enemyTargetSkillSpawnPoint.position : partyTargetSkillSpawnPoint.position;
            case EffectSpawn.Ground:
                using (IEnumerator<BattleEntities> enumerator = targets.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        return enumerator.Current.BattleVisuals.GetPoint(PointType.Ground);
                    }
                }
                return caster.BattleVisuals.GetPoint(PointType.Attack);
            case EffectSpawn.Center:
                return caster.BattleVisuals.GetPoint(PointType.Center);
            case EffectSpawn.Hit:
                return caster.BattleVisuals.GetPoint(PointType.Hit);
            default:
                return caster.BattleVisuals.GetPoint(PointType.Attack);
        }
    }

    // 데미지 텍스용 코드
    public async Task PlayDamageText(string key, Vector3 target, TextType type, int amount = 0, bool isCritical = false)
    {
        // 풀에서 데미지 텍스트 오브젝트 가져오기
        Effect damageTextObj = await pool.GetEffect(key);
        damageTextObj.transform.SetParent(damageTextRoot, false);

        TextMeshProUGUI tmp = damageTextObj.GetComponent<TextMeshProUGUI>();
        if (tmp != null)
        {
            switch(type)
            {
                case TextType.Damage:
                    tmp.text = amount.ToString();
                    if (isCritical)
                    {
                        tmp.fontSize = 30;
                        tmp.color = Color.red;
                    }
                    else
                    {
                        tmp.fontSize = 27;
                        tmp.color = Color.orange;
                    }
                    break;
                case TextType.Eva:
                    tmp.text = "Miss";
                    tmp.fontSize = 25;
                    tmp.color = Color.lightSkyBlue;
                    break;
                case TextType.Critical:
                    tmp.text = "Critical";
                    tmp.fontSize = 20;
                    tmp.color = Color.yellow;
                    break;
                case TextType.Heal:
                    tmp.text = amount.ToString();
                    tmp.fontSize = 27;
                    tmp.color = Color.darkGreen;
                    break;
            }
        }

        // 월드 좌표를 UI 좌표로 변환하여 배치
        Vector3 screenPos = _camera.WorldToScreenPoint(target);
        damageTextObj.transform.position = screenPos;

        damageTextObj.gameObject.SetActive(true);

        // 애니메이션 재생 및 풀에 반환 대기
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        StartCoroutine(WaitForDamageTextCompletion(key, damageTextObj, tcs));
        await tcs.Task;
    }

    private IEnumerator WaitForDamageTextCompletion(string key, Effect damageTextObj, TaskCompletionSource<bool> tcs)
    {
        Animator animator = damageTextObj.GetComponent<Animator>();
        if (animator != null)
        {
            // 애니메이션 클립의 길이를 가져와서 그 시간만큼 대기
            float animLength = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animLength);
        }
        else
        {
            // 애니메이터가 없다면 1초 후 반환
            yield return new WaitForSeconds(1f);
        }

        // 풀에 반환
        pool.ReturnEffect(key, damageTextObj);
        tcs.SetResult(true);
    }

    // 패시브 이펙트 생성
    public async Task<Effect> GetContinuousEffect(string key, Vector3 target)
    {
        Effect effect = await pool.GetEffect(key);
        if (effect != null && target != null)
        {
            effect.gameObject.SetActive(true);
            effect.transform.SetParent(effectRoot);
            effect.transform.position = target;
        }
        return effect;
    }


    // 패시브 이펙트를 풀에 반환
    public void ReturnContinuousEffect(string key, Effect effect)
    {
        if (effect != null)
        {
            pool.ReturnEffect(key, effect);
        }
    }
}
