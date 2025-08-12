using UnityEngine;

public class DungeonSceneMaker : MonoBehaviour
{
    public int dungeonKey;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("NextDungeonKey"))
        {
            dungeonKey = PlayerPrefs.GetInt("NextDungeonKey");
            PlayerPrefs.DeleteKey("NextDungeonKey");
        }
    }
}