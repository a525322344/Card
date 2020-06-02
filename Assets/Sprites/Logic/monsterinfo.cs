using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate actionAbstract selectWay(int index);
[System.Serializable]
public abstract class monsterInfo
{
    public string name = "默认怪物";
    public int Id;
    public int monsterLevel;
    public int health;
    public List<actionAbstract> actionList = new List<actionAbstract>();
    public selectWay selectAction;
    public virtual void Init() { }
}
[System.Serializable]
public class monInfo_Slima : monsterInfo
{
    private actionAbstract lastAction;
    public monInfo_Slima()
    {
        name = "月光史莱姆";
        health = 60;
        Id = 0;
        monsterLevel = 1;

        actionList.Add(new actionHurt(6));
        actionList.Add(new actionHurt(9));
        actionList.Add(new actionAdmix(new actionArmor(5), new actionHurt(5)));
        //随机选取，但大概率不会连续两次一样
        selectAction = new selectWay((int x) =>
          {
              
              actionAbstract result = ListOperation.RandomValue<actionAbstract>(actionList);
              //如果和上一次相同，则再随机取一次，但就取这两次了，保证大概率不连着相同
              if (result == lastAction)
              {
                  result = ListOperation.RandomValue<actionAbstract>(actionList);
              }
              lastAction = result;
              //gameManager.Instance.battlemanager.showcontroll.CreateNewAction(result);
              return result;
          });
    }
}

public class monInfo_Cat : monsterInfo
{
    private int actionorder = 0;
    public monInfo_Cat()
    {
        name = "火云猫";
        health = 40;
        Id = 0;
        monsterLevel = 1;
        actionList.Add(new actionAdmix(new actionHurt(5), new actionDebuff(new ActionEffect_MonsterBurn(1))));
        actionList.Add(new actionDebuff(new ActionEffect_MonsterBurn(4)));
        actionList.Add(new actionHurt(1,3));


        //顺序选择
        selectAction = new selectWay((int x) =>
        {
            actionAbstract action;
            action = actionList[actionorder];
            actionorder++;
            if (actionorder == actionList.Count)
            {
                actionorder = 0;
            }
            return action;
        });
    }

    public override void Init()
    {
        actionorder = 0;
    }
}
public class monInfo_Bunny : monsterInfo
{
    private int actionorder = 0;
    public monInfo_Bunny()
    {
        name = "耳兔";
        health = 40;
        Id = 1;
        monsterLevel = 1;
        actionList.Add(new actionAdmix(new actionHurt(6,1,new PerformEffect(0,instantiateManager.instance.EffectGOList[4],0,1,0.8f)), new actionArmor(12)));
        actionList.Add(new actionPowerUp(new ActionEffect_MonsterPowerUp(3)));
        actionList.Add(new actionHurt(3, 2, new PerformEffect(0, instantiateManager.instance.EffectGOList[7], 0, 1, 0.8f)));
        actionList.Add(new actionHurt(6,1, new PerformEffect(0, instantiateManager.instance.EffectGOList[7], 0, 1, 0.8f)));


        //顺序选择
        selectAction = new selectWay((int x) =>
        {
            actionAbstract action;
            action = actionList[actionorder];
            actionorder++;
            if (actionorder == actionList.Count)
            {
                actionorder = 1;
            }
            return action;
        });
    }

    public override void Init()
    {
        actionorder = 0;
    }
}

public class monInfo_SnowMan : monsterInfo
{
    private int actionorder = 0;
    public monInfo_SnowMan()
    {
        name = "雪人";
        health = 40;
        Id = 2;
        monsterLevel = 1;
        actionList.Add(new actionDebuff(new ActionEffect_MonsterDownDrawCardNum(-1)));
        actionList.Add(new actionAdmix(new actionHurt(9,1, new PerformEffect(0, instantiateManager.instance.EffectGOList[2], 0, 1, 0.8f)), new actionArmor(6)));
        actionList.Add(new actionHurt(15,1, new PerformEffect(0, instantiateManager.instance.EffectGOList[5], 0, 1, 0.8f)));


        //顺序选择
        selectAction = new selectWay((int x) =>
        {
            actionAbstract action;
            action = actionList[actionorder];
            actionorder++;
            if (actionorder == actionList.Count)
            {
                actionorder = 1;
            }
            return action;
        });
    }

    public override void Init()
    {
        actionorder = 0;
    }
}

public class monInfo_Bear : monsterInfo
{
    private int actionorder = 0;
    public monInfo_Bear()
    {
        name = "雷熊";
        health = 80;
        Id = 3;
        monsterLevel = 2;
        actionList.Add(new actionDebuff(new ActionEffect_MonsterAddStateMabi(4)));
        actionList.Add(new actionAdmix(new actionHurt(12,1, new PerformEffect(0, instantiateManager.instance.EffectGOList[7], 0, 1, 0.8f)), new actionArmor(8)));
        actionList.Add(new actionHurt(24,1, new PerformEffect(0, instantiateManager.instance.EffectGOList[11], 0, 1, 0.8f)));


        //顺序选择
        selectAction = new selectWay((int x) =>
        {
            actionAbstract action;
            action = actionList[actionorder];
            actionorder++;
            if (actionorder == actionList.Count)
            {
                actionorder = 0;
            }
            return action;
        });
    }

    public override void Init()
    {
        actionorder = 0;
    }
}

public class monInfo_usagi : monsterInfo
{
    private int actionorder = 0;
    public monInfo_usagi()
    {
        name = "耳兔";
        health = 150;
        Id = 1;
        monsterLevel = 1;
        actionList.Add(new actionAdmix(new actionArmor(12), new actionHurt(9)));
        actionList.Add(new actionHurt(6));
        actionList.Add(new actionAdmix(new actionHurt(5), new actionHurt(5)));
        //顺序选择
        selectAction = new selectWay((int x) =>
        {
            actionAbstract action;
            if (actionorder < actionList.Count - 1)
            {
                action = actionList[actionorder];
                actionorder++;
            }
            else
            {
                action = actionList[0];
                actionorder = 1;
            }
            return action;
            actionList.Remove(new actionAdmix(new actionArmor(12), new actionHurt(9)));
        });
    }
}

public class monInfo_Sample : monsterInfo
{
    private int actionorder = 0;
    public monInfo_Sample(int damagenum)
    {
        name = "" + damagenum + "级怪物";
        health = damagenum * 5+10;
        monsterLevel = damagenum;
        actionList.Add(new actionHurt(damagenum * 2));
        actionList.Add(new actionAdmix(new actionArmor(damagenum), new actionHurt(damagenum)));
        actionList.Add(new actionHurt(damagenum * 2));

        //顺序选择
        selectAction = new selectWay((int x) =>
        {
            actionAbstract action;
            if (actionorder < actionList.Count - 1)
            {
                action = actionList[actionorder];
                actionorder++;
            }
            else
            {
                action = actionList[0];
                actionorder = 1;
            }
            return action;
            //actionList.Remove(new actionAdmix(new actionArmor(12), new actionHurt(9)));
        });
    }
}

public class monInfo_MoNv : monsterInfo
{
    private int actionorder = 0;
    public monInfo_MoNv()
    {
        name = "月之使者";
        health = 200;
        Id = 4;
        monsterLevel = 3;
        actionList.Add(new actionDebuff(new ActionEffect_MonsterBurn(4), new PerformEffect(0, instantiateManager.instance.EffectGOList[8], 0, 1, 0.8f)));
        actionList.Add(new actionHurt(8,2, new PerformEffect(0, instantiateManager.instance.EffectGOList[7], 0, 1, 0.8f)));
        actionList.Add(new actionHurt(15,1, new PerformEffect(0, instantiateManager.instance.EffectGOList[11], 0, 1, 0.8f)));
        actionList.Add(new actionAdmix(new actionArmor(12), new actionHurt(6,1, new PerformEffect(0, instantiateManager.instance.EffectGOList[4], 0, 1, 0.8f))));
        actionList.Add(new actionAdmix(new actionDebuff(new ActionEffect_MonsterAddStateMabi(3), new PerformEffect(1, instantiateManager.instance.EffectGOList[11], 0, 1, 0.3f)), new actionPowerUp(new ActionEffect_MonsterPowerUp(2))));
        actionList.Add(new actionHurt(4, 4, new PerformEffect(0, instantiateManager.instance.EffectGOList[1], 0, 1, 0.8f)));

        //顺序选择
        selectAction = new selectWay((int x) =>
        {
            actionAbstract action;
            action = actionList[actionorder];
            actionorder++;
            if (actionorder == actionList.Count)
            {
                actionorder = 1;
            }
            return action;
        });
    }

    public override void Init()
    {
        actionorder = 0;
    }
}