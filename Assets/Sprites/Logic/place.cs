using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class place
{

    public abstract void onclick();
}

public class battlePlace : place
{
    public battlePlace(enemybase enemybase=null)
    {

    }
    public override void onclick()
    {
        //进入战斗
        Debug.Log("计入战斗");
    }

    public int enemynum;
}

public class shopPlace : place
{
    public shopPlace()
    {

    }
    public override void onclick()
    {
        //进入商店
    }
}