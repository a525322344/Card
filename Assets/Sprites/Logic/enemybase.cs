using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class pawnbase
{
    public int healthmax;
    public int healthnow;
    public int armor;
}

[System.Serializable]
public class enemybase : pawnbase
{
    public int index;
    public void hurtHealth(int i)
    {
        healthnow -= i;
    }
    public void GetArmor(int i)
    {
        if (i >= 0)
        {
            armor += i;
        }
    }
}
[System.Serializable]
public class playerpawn : enemybase
{

}