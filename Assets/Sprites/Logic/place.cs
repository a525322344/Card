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
    public monsterInfo monsterInfo;
    public int sceneId;
    public battlePlace(monsterInfo monsterinfo=null,int sceneid=1)
    {
        monsterInfo = monsterinfo;
        sceneId = sceneid;
    }
    public override void onclick()
    {
        //进入战斗
        gameManager.Instance.mapmanager.EnterBattle(this);
    }

    public override void onover()
    {
        //图标高亮
    }
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

public class befallPlace : place
{
    public befallPlace()
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