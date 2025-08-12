using UnityEngine;

public static class ExpTable
{
    public static int GetRequiredExp(int level)
    {
        return 50 + (level * level * 10);
    }
}
