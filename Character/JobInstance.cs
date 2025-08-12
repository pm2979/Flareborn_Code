using System;

[Serializable]
public class JobInstance
{
    public JobData JobData { get; private set; }

    public DesignEnums.JobType JobType => JobData.JobType;

    public int MaxHp => JobData.MaxHp;
    public int Atk => JobData.ATK;
    public int SATK => JobData.SATK;
    public int Def => JobData.DEF;
    public int SDEF => JobData.SDEF;
    public int Spd => JobData.SPD;
    public int Critical => JobData.Critical;
    public int Eva => JobData.EVA;
    public int SP => JobData.SP;

    public int BasicAttakIndex => JobData.BasicAttack;
    public SkillInstance BasicAttack { get; private set; }

    public JobInstance(JobData data)
    {
        JobData = data;

        EquippedSetSkill(BasicAttakIndex);
    }

    public void EquippedSetSkill(int skillKey) // 스킬 장착
    {
        SkillInstance instance = SkillFactory.Create(skillKey);
        EquippedBasicAttack(instance);
    }

    public void EquippedBasicAttack(SkillInstance skill)
    {
        BasicAttack = skill;
    }
}
