using System;

public class StaggerController 
{
    // 무력화 게이지 (Stagger Gauge)
    public int CurrentSG { get; private set; }
    public int MaxSG { get; private set; }

    public EnemyEntities EnemyEntities { get; private set; }

    public StaggerController(int maxStagger, EnemyEntities enemyEntities)
    {
        StaggerReset();
        MaxSG = maxStagger;
        EnemyEntities = enemyEntities;
    }

    public void AddStagger(int amount) // 무력화 게이지 증가
    {
        CurrentSG = Math.Min(CurrentSG + amount, MaxSG);

        if(CurrentSG >= MaxSG) 
        {
            EnemyEntities.ApplyStun(1);
        }
    }

    public void ReduceStagger(int amount) // 무력화 게이지 감소
    {
        CurrentSG = Math.Max(0, CurrentSG - amount);
    }

    public void StaggerReset()
    {
        CurrentSG = 0;
    }
}
