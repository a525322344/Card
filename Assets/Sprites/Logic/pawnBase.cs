﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class pawnbase
{
    public string name = "默认单位";
    public int healthmax;
    public int healthnow;
    public int armor;
    public int enemynum;
    public List<stateAbstarct> stateList = new List<stateAbstarct>();///展示用链表
    public Dictionary<string, stateAbstarct> nameStatePairs = new Dictionary<string, stateAbstarct>();
    
    public virtual void hurtHealth(int i)
    {
        //if(nameStatePairs.ContainsKey("Burn"))
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
    public virtual void GetArmor(int i) { }
    public virtual void destoryArmor(int i) { }
}

[System.Serializable]
public class enemybase : pawnbase
{
    public override void GetArmor(int i)
    {
        if (i >= 0)
        {
            armor += i;
        }
        gameManager.Instance.battlemanager.realenemy.changeHealthAndArmor(armor,healthnow);
    }
    public override void destoryArmor(int i)
    {

        armor -= i;
        gameManager.Instance.battlemanager.realenemy.changeHealthAndArmor(armor, healthnow);
    }
}
[System.Serializable]
public class playerpawn : pawnbase
{
    public override void GetArmor(int i)
    {
        if (i >= 0)
        {
            armor += i;
        }
        gameManager.Instance.battlemanager.realplayer.changeHealthAndArmor(armor, healthnow);
    }
    public override void destoryArmor(int i)
    {

        armor -= i;
        gameManager.Instance.battlemanager.realplayer.changeHealthAndArmor(armor, healthnow);
    }
    public override void hurtHealth(int i)
    {
        //if(nameStatePairs.ContainsKey("Burn"))
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
        gameManager.Instance.battlemanager.realplayer.changeHealthAndArmor(armor, healthnow);
    }
}