using UnityEngine;
using System;
using DG.Tweening;
using System.Collections.Generic;
using static DesignEnums;

public class BattleVisuals : MonoBehaviour
{
    [Serializable]
    public class NamedPoint
    {
        public PointType pointType;
        public Transform pointTransform;
    }

    [SerializeField] private List<NamedPoint> hitPoints = new();

    private Dictionary<PointType, Transform> pointMap;

    public AnimationHandler animationHandler;

    [Header("상태 이펙트")]
    [SerializeField] private ParticleSystem awakenEffect; // 각성
    [SerializeField] private ParticleSystem collapseEffect; // 붕괴
    [SerializeField] private ParticleSystem transcendentEffect; // 초월
    [SerializeField] private ParticleSystem enrageEffect; // 격노

    // 레벨
    public int Level { get; protected set; }

    protected const string LEVEL_ABB = "Lvl: ";

    private void Awake()
    {
        animationHandler = GetComponentInChildren<AnimationHandler>();

        pointMap = new Dictionary<PointType, Transform>();

        foreach (var point in hitPoints)
        {
            if (point.pointTransform != null && !pointMap.ContainsKey(point.pointType))
            {
                pointMap[point.pointType] = point.pointTransform;
            }
        }

        AwakenEffect(false);
        CollapseEffect(false);
        TranscendentEffect(false);
        EnrageEffect(false);
    }

    public virtual void SetStartingValues(int currHealth, int maxHealth, int curentSG, int maxSG, int level) { } // 시작 설정
    public virtual void ChangeStat(int currHealth, int currSG) { } // 체력 변경
    public virtual void UpdateBar() { } // UI 업데이트

    public void BattlePosition(Transform transform, Action action) // 캐릭터 턴 위치
    {
        animationHandler.PlayWalkAnimation(true);

        this.transform.DOMove(transform.position, 0.5f).OnComplete(() =>
        {
            if (this == null || animationHandler == null || gameObject == null) return;
            if (!this.gameObject.activeInHierarchy) return;

            action?.Invoke();
            animationHandler.PlayWalkAnimation(false);
        });
    }

    public Vector3 GetPoint(PointType type) // 해당 타입의 포인트 반환
    {
        if (pointMap != null && pointMap.TryGetValue(type, out var transform))
            return transform.position;

        return this.transform.position; // 기본 위치 fallback
    }

    public void AwakenEffect(bool isAwaken)
    {
        if(awakenEffect != null)
        {
            awakenEffect.gameObject.SetActive(isAwaken);

            if(isAwaken)
            {
                awakenEffect.Play();
            }
            else
            {
                awakenEffect.Pause();
            }
        }
    }

    public void CollapseEffect(bool isCollapse)
    {
        if (collapseEffect != null)
        {
            collapseEffect.gameObject.SetActive(isCollapse);

            if (isCollapse)
            {
                collapseEffect.Play();
            }
            else
            {
                collapseEffect.Pause();
            }
        }
    }

    public void TranscendentEffect(bool isTranscendent)
    {
        if (transcendentEffect != null)
        {
            transcendentEffect.gameObject.SetActive(isTranscendent);

            if (isTranscendent)
            {
                transcendentEffect.Play();
            }
            else
            {
                transcendentEffect.Pause();
            }
        }
    }

    public void EnrageEffect(bool isEnrage)
    {
        if (enrageEffect != null)
        {
            enrageEffect.gameObject.SetActive(isEnrage);

            if (isEnrage)
            {
                enrageEffect.Play();
            }
            else
            {
                enrageEffect.Pause();
            }
        }
    }
}
