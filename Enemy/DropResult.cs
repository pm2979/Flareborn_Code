using System;
using System.Collections.Generic;

[Serializable]
public class DropResult
{
    public int Exp;
    public int Gold;
    public List<int> ItemKeys;

    public DropResult()
    {
        ItemKeys = new List<int>();
    }
}
