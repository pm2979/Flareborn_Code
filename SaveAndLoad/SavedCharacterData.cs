using System;
using System.IO;

[System.Serializable]
public class SavedCharacterData
{
    public int characterID;
    public string characterName;
    public int currentHp;
    public int currentExp;
    public int level;
    public int currentStress;
    public int currentHG;
    public bool isAwakened;
    public bool isCollapsed;


    // 생성자: 이 데이터를 이용하여 SavedCharacterData 객체를 초기화
    public SavedCharacterData(int id, string name, int hp, int exp, int lvl, int stress, int hg, bool awakened, bool collapsed)
    {
        characterID = id;
        characterName = name;
        currentHp = hp;
        currentExp = exp;
        level = lvl;
        currentStress = stress;
        currentHG = hg;
        isAwakened = awakened;
        isCollapsed = collapsed;
    }
}