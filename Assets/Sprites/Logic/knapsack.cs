using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class knapsack
{
    public knapsack(bool[] ise)
    {
        isexploits = ise;
    }
    public bool[] isexploits;
    public Dictionary<Vector2, MagicPart> installParts = new Dictionary<Vector2, MagicPart>();
}