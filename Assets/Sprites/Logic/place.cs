using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class place
{

    public abstract void onclick();
    public abstract void onover();
}

public class battlePlace : place
{
    public battlePlace(enemybase enemybase=null)
    {

    }
    public override void onclick()
    {
        //进入战斗
        Debug.Log("进入战斗");
        SceneManager.LoadScene("SampleScene");
    }

    public override void onover()
    {
        //图标高亮
        Debug.Log("图标高亮");
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
        Debug.Log("进入商店");
    }

    public override void onover()
    {
        //图标高亮
        Debug.Log("图标高亮");
    }
}

public class eventPlace : place
{
    public eventPlace()
    {

    }
    public override void onclick()
    {
        //触发事件
        Debug.Log("触发事件");
    }
    public override void onover()
    {
        //图标高亮
        Debug.Log("图标高亮");
    }

    public int eventnum;
}

public class deckPlace : place
{
    public deckPlace()
    {

    }
    public override void onclick()
    {
        //查看卡组
        Debug.Log("查看卡组");
    }
    public override void onover()
    {
        //图标高亮
        Debug.Log("图标高亮");
    }
}