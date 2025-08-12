using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class JobData
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 이름 
    /// </summary>
    public string Name;

    /// <summary>
    /// 직업 타입
    /// </summary>
    public DesignEnums.JobType JobType;

    /// <summary>
    /// 최대 체력
    /// </summary>
    public int MaxHp;

    /// <summary>
    /// 공격력
    /// </summary>
    public int ATK;

    /// <summary>
    /// 특수 공격력
    /// </summary>
    public int SATK;

    /// <summary>
    /// 방어력
    /// </summary>
    public int DEF;

    /// <summary>
    /// 특수 방어력
    /// </summary>
    public int SDEF;

    /// <summary>
    /// 속도
    /// </summary>
    public int SPD;

    /// <summary>
    /// 크리티컬
    /// </summary>
    public int Critical;

    /// <summary>
    /// 회피력
    /// </summary>
    public int EVA;

    /// <summary>
    /// 무력화력
    /// </summary>
    public int SP;

    /// <summary>
    /// 기본 공격 스킬
    /// </summary>
    public int BasicAttack;

}
public class JobDataLoader
{
    public List<JobData> ItemsList { get; private set; }
    public Dictionary<int, JobData> ItemsDict { get; private set; }

    public JobDataLoader(string path = "JSON/JobData")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, JobData>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<JobData> Items;
    }

    public JobData GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public JobData GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
