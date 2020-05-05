using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
//1——普通敌人
//2——精英敌人
//3——Boss
//4——事件
//5——商店
//0——休息
//6——宝箱
//7——空
public abstract class place
{
    public int imageorder;
    public abstract void onclick();
}

public class battlePlace : place
{
    public int sceneId;
    public int storey;
    public battlePlace(int battleRank = 1,int _storey=1,int sceneid=0)
    {
        storey = _storey;
        sceneId = sceneid;
        imageorder = battleRank;
    }
    public override void onclick()
    {
        //进入战斗
        gameManager.Instance.mapmanager.EnterBattle(this);
    }
}

public class spacePlace : place
{
    public spacePlace()
    {
        imageorder = 7;
    }
    public override void onclick()
    {
        
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
        imageorder = 5;
    }
    public override void onclick()
    {
        //进入商店
        instantiateManager.instance.instanShopBoard();
        gameManager.Instance.mapmanager.EventWindow(true);// mapState = MapState.EventWindow;
    }
}

public class befallPlace : place
{
    public befallPlace()
    {
        imageorder = 4;
    }
    public override void onclick()
    {
        gameManager.Instance.mapmanager.EnterBefall();
    }

    public int eventnum;
}

public class sleepPlace : place
{
    public sleepPlace() {
        imageorder = 0;
    }
    public override void onclick()
    {
        gameManager.Instance.mapmanager.EnterSleep();
    }
}

public class treasurePlace : place
{
    public treasurePlace()
    {
        imageorder = 6;
    }
    public override void onclick()
    {
        gameManager.Instance.mapmanager.EnterTreasure();
    }
}

public enum PlaceState
{
    Cannot,     //不可选的
    ToGo,       //可选的
    NowOn,      //当前位置
    Used,       //过去的
}
[System.Serializable]
public class PlaceNode: IComparable
{
    public Vector2 PointPosi;
    public PlaceState placeState;
    public place thisplace;
    public Transform realplaceTran;
    public realPlace realPlace;
    public PlaceNode(place nowplace,Vector2 posi)
    {
        thisplace = nowplace;
        PointPosi = posi;
        placeState = PlaceState.Cannot;
    }


    public List<PlaceNode> nextNodeList = new List<PlaceNode>();
    public List<PlaceNode> lastNodeList = new List<PlaceNode>();
    public void LinkNode(PlaceNode placenode)
    {
        nextNodeList.Add(placenode);
        placenode.lastNodeList.Add(this);
    }
    public int CompareTo(object obj)
    {
        PlaceNode p = obj as PlaceNode;
        return this.PointPosi.x.CompareTo(p.PointPosi.x);
    }
}