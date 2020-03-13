using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class place
{
    public int imageorder;
    public abstract void onclick();
}

public class battlePlace : place
{
    public monsterInfo monsterInfo;
    public int sceneId;
    public battlePlace(monsterInfo monsterinfo=null,int sceneid=1)
    {
        monsterInfo = monsterinfo;
        sceneId = sceneid;
        imageorder = 1;
    }
    public override void onclick()
    {
        //进入战斗
        gameManager.Instance.mapmanager.EnterBattle(this);
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
}

public class befallPlace : place
{
    public befallinfo m_befallinfo;
    public befallPlace(befallinfo beffalinfo)
    {
        m_befallinfo = beffalinfo;
        imageorder = 2;
    }
    public override void onclick()
    {
        //打开二级事件窗口
        gameManager.Instance.uimanager.uiBefallBoard.EnterEventBoard(m_befallinfo);
        gameManager.Instance.mapmanager.mapState = MapState.EventWindow;
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

}