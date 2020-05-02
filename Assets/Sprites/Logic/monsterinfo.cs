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
        health = 150;
        Id = 1;
        monsterLevel = 1;
        actionList.Add(new actionAdmix(new actionArmor(12), new actionHurt(9)));
        actionList.Add(new actionHurt(6));
        actionList.Add(new actionAdmix(new actionHurt(5), new actionHurt(5)));
        actionList.Add(new actionHurt(8));
        actionList.Add(new actionAdmix(new actionHurt(7), new actionHurt(7)));
        actionList.Add(new actionHurt(10));
        actionList.Add(new actionAdmix(new actionHurt(7), new actionHurt(7)));
        actionList.Add(new actionHurt(12));
        actionList.Add(new actionAdmix(new actionHurt(9), new actionHurt(9)));
        actionList.Add(new actionHurt(14));
        actionList.Add(new actionAdmix(new actionHurt(11), new actionHurt(11)));
        actionList.Add(new actionHurt(16));
        actionList.Add(new actionAdmix(new actionHurt(13), new actionHurt(13)));
        actionList.Add(new actionHurt(18));
        actionList.Add(new actionAdmix(new actionHurt(15), new actionHurt(15)));
        actionList.Add(new actionHurt(20));
        actionList.Add(new actionAdmix(new actionHurt(17), new actionHurt(17)));

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