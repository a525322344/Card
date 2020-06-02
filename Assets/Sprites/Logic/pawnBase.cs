using System.Collections;
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

    public virtual void hurtHealth(int i,out int real)
    {
        if (i > 0)
        {
            if (armor > i)
            {
                destoryArmor(i);
                real = 0;
            }
            else
            {
                healthnow -= (i - armor);
                destoryArmor(armor);
                real = i - armor;
            }
        }
        else
        {
            real = 0;
        }
    }
    public virtual void realHurtHealth(int i)
    {
        healthnow -= i;
    }
    public virtual void GetArmor(int i)
    {
        if (i >= 0)
        {
            armor += i;
        }
    }
    public virtual void destoryArmor(int i)
    {
        armor -= i;
    }
}

[System.Serializable]
public class enemybase : pawnbase
{
    public override void GetArmor(int i)
    {
        base.GetArmor(i);
        gameManager.Instance.battlemanager.realenemy.changeHealthAndArmor(armor,healthnow);
    }
    public override void destoryArmor(int i)
    {
        base.destoryArmor(i);
        gameManager.Instance.battlemanager.realenemy.changeHealthAndArmor(armor, healthnow);
    }
    public override void hurtHealth(int i,out int real)
    {
        base.hurtHealth(i,out real);
        gameManager.Instance.battlemanager.realenemy.showGetHurt(real);
        gameManager.Instance.battlemanager.realenemy.changeHealthAndArmor(armor, healthnow);
    }
    public override void realHurtHealth(int i)
    {
        base.realHurtHealth(i);
        gameManager.Instance.battlemanager.realenemy.changeHealthAndArmor(armor, healthnow);
        gameManager.Instance.battlemanager.realenemy.showGetHurt(i);
    }
}
[System.Serializable]
public class playerpawn : pawnbase
{
    public override void GetArmor(int i)
    {
        base.GetArmor(i);
        gameManager.Instance.battlemanager.realplayer.changeHealthAndArmor(armor, healthnow);
    }
    public override void destoryArmor(int i)
    {
        base.destoryArmor(i);
        gameManager.Instance.battlemanager.realplayer.changeHealthAndArmor(armor, healthnow);
    }
    public override void hurtHealth(int i,out int real)
    {
        base.hurtHealth(i,out real);
        gameManager.Instance.battlemanager.realplayer.changeHealthAndArmor(armor, healthnow);
        gameManager.Instance.battlemanager.realplayer.showGetHurt(real);
    }
    public override void realHurtHealth(int i)
    {
        base.realHurtHealth(i);
        gameManager.Instance.battlemanager.realplayer.changeHealthAndArmor(armor, healthnow);
        gameManager.Instance.battlemanager.realplayer.showGetHurt(i);
    }
}