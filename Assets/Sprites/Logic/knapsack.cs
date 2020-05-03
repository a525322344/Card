using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class knapsack
{
    //4
    //3
    //2
    //1
    //0
    //  0  1  2  3  4
    public knapsack(bool[] ise)
    {
        isexploits = ise;
    }
    public bool[] isexploits;
    public Dictionary<Vector2, MagicPart> installParts = new Dictionary<Vector2, MagicPart>();


}

