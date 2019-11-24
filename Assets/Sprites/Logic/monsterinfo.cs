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
    monInfo_Slima()
    {

    }


}