using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate actionAbstract selectWay(int index);

public abstract class monsterInfo
{
    public int Id;
    public int monsterLevel;
    public int health;
    public List<actionAbstract> actionList = new List<actionAbstract>();
    public selectWay selectAction;
}

public class monInfo_Slima : monsterInfo
{
    private actionAbstract lastAction;
    public monInfo_Slima()
    {
        actionList.Add(new actionHurt(6));
        actionList.Add(new actionHurt(9));
        actionList.Add(new actionAdmix(new actionArmor(5), new actionHurt(5)));
        selectAction = new selectWay((int x) =>
          {
              actionAbstract result = ListOperation.RandomValue<actionAbstract>(actionList);
              //如果和上一次相同，则再随机取一次，但就取这两次了，保证大概率不连着相同
              if (result == lastAction)
              {
                  result = ListOperation.RandomValue<actionAbstract>(actionList);
              }
              lastAction = result;
              gameManager.Instance.battlemanager.showcontroll.CreateNewAction(result);
              return result;
          });
        health = 60;
        Id = 1;
        monsterLevel = 1;
    }


}