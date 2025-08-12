using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBattleVisuals : BattleVisuals
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image staggerBar;

    // 체력
    public int CurrentHealth { get; protected set; }
    public int MaxHealth { get; protected set; }

    // 무력화 게이지 (Stagger Gauge)
    public int CurrentSG { get; protected set; }
    public int MaxSG { get; protected set; }

    public override void ChangeStat(int currHealth, int currBG)
    {
        CurrentHealth = currHealth;
        CurrentSG = currBG;

        // 만약 체력이 0일경우 -> death애니메이션 플레이 -> 배틀 오브젝트 제거
        if (currHealth <= 0)
        {
            if (animationHandler != null && animationHandler.enabled)
                animationHandler.PlayDeathAnimation();
        }

        UpdateBar();
    }

    public override void SetStartingValues(int currHealth, int maxHealth, int curentSG, int maxSG, int level)
    {
        CurrentHealth = currHealth;
        MaxHealth = maxHealth;
        CurrentSG = curentSG;
        MaxSG = maxSG;
        Level = level;

        UpdateBar();
    }

    public override void UpdateBar()
    {
        if (healthBar == null)
        {
            return;
        }

        healthBar.fillAmount = (float) CurrentHealth / MaxHealth;

        staggerBar.fillAmount = (float) CurrentSG / MaxSG;
    }
}
