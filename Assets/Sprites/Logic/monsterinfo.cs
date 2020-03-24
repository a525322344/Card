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
        health = 60;
        Id = 1;
        monsterLevel = 1;
        actionList.Add(new actionHurt(6));
        actionList.Add(new actionHurt(9));
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
        Id = 2;
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