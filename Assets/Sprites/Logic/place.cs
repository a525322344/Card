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

public class startPlace : place
{
    public startPlace()
    {

    }
    public override void onclick()
    {
        
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

public class sleepPlace : place
{
    public sleepPlace() { }
    public override void onclick()
    {
        throw new System.NotImplementedException();
    }
}

public class treasurePlace : place
{
    public treasurePlace()
    {
        imageorder = 2;
    }
    public override void onclick()
    {
    }
}

public enum PlaceState
{
    DenseFog,   //迷雾，还未解锁
    ToGo,       //可选的
    ToGoOut,    //事件中
    NowOn,      //当前位置
    Used,       //过去的
}
[System.Serializable]
public class PlaceNode
{
    public Vector2 PointPosi;
    public PlaceState placeState;
    public place thisplace;
    public Transform realplaceTran;
    public PlaceNode(place nowplace,Vector2 posi)
    {
        thisplace = nowplace;
        PointPosi = posi;
        placeState = PlaceState.DenseFog;
    }


    public List<PlaceNode> nextNodeList = new List<PlaceNode>();
    public List<PlaceNode> lastNodeList = new List<PlaceNode>();
    public void LinkNode(PlaceNode placenode)
    {
        nextNodeList.Add(placenode);
        placenode.lastNodeList.Add(this);
    }
}