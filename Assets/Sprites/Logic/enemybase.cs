using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class pawnbase
{
    public int healthmax;
    public int healthnow;
    public int armor;
    public int enemynum;
    public List<stateAbstarct> stateList = new List<stateAbstarct>();///展示用链表
    public Dictionary<string, stateAbstarct> nameStatePairs = new Dictionary<string, stateAbstarct>();
}

[System.Serializable]
public class enemybase : pawnbase
{
    public void hurtHealth(int i)
    {
        if (i > 0)
        {
            if (armor > i)
            {
                destoryArmor(i);
            }
            else
            {
                healthnow -= (i - armor);
                destoryArmor(armor);
            }
        }
    }
    public void GetArmor(int i)
    {
        if (i >= 0)
        {
            armor += i;
        }
    }
    public void destoryArmor(int i)
    {
        armor -= i;
        if (armor < 0)
        {
            armor = 0;
        }
    }
}
[System.Serializable]
public class playerpawn : enemybase
{

}