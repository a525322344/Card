using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate void buttonTo();

public class befallinfo
{
    public befallinfo(string _name,int textureint,string _describe)
    {
        name = _name;
        textureOrder = textureint;
        describe = _describe;
    }
    public befallinfo(string _name, int textureint, string _describe,params buttoninfo[] buttoninfos)
    {
        name = _name;
        textureOrder = textureint;
        describe = _describe;
        foreach(buttoninfo bi in buttoninfos)
        {
            buttons.Add(bi);
        }
    }

    public string name;
    public int textureOrder;
    public string describe;
    public List<buttoninfo> buttons = new List<buttoninfo>();
}

public abstract class buttoninfo
{
    public string buttonDescribe;
    protected buttonTo buttonFun;
    public void onclick()
    {
        buttonFun();
    }
}
//退出
public class Button_ExitBefall : buttoninfo
{
    public Button_ExitBefall(string buttondes)
    {
        buttonDescribe = buttondes;
        //退出遭遇
        buttonFun = new buttonTo(() =>
        {
            gameManager.Instance.uimanager.uiBefallBoard.ExitEventBoard();
            gameManager.Instance.mapmanager.mapState = MapState.MainMap;
        });
    }
    public Button_ExitBefall()
    {
        buttonDescribe = "回避";
        //退出遭遇
        buttonFun = new buttonTo(() =>
        {
            gameManager.Instance.uimanager.uiBefallBoard.ExitEventBoard();
            gameManager.Instance.mapmanager.mapState = MapState.MainMap;
        });
    }
}
//整理背包
public class Button_SortPart : Button_NextBeffal
{

    public Button_SortPart(befallinfo nextBefallInfo)
    {
        Debug.Log("sort");
        nextBefall = nextBefallInfo;
        buttonDescribe = "好，做好准备";
        buttonFun += new buttonTo(() =>
        {
            gameManager.Instance.instantiatemanager.instanSortPart(gameManager.Instance.playerinfo.MagicPartDick,gameManager.Instance.playerinfo.playerKnapsack);
        });
    }
}
public class Button_NextBeffal:buttoninfo
{
    public befallinfo nextBefall;
    public Button_NextBeffal()
    {
        Debug.Log("nextbutton");
        buttonFun = new buttonTo(() =>
        {
            gameManager.Instance.uimanager.uiBefallBoard.EnterEventBoard(nextBefall);
        });
    }
}

public class Button_OverSortPart : buttoninfo
{
    public Button_OverSortPart()
    {
        buttonDescribe = "完成！";
        buttonFun=new buttonTo(() =>
        {
            gameManager.Instance.uimanager.uiBefallBoard.ExitEventBoard();
            gameManager.Instance.instantiatemanager.ExitSortPart();
            gameManager.Instance.mapmanager.mapState = MapState.MainMap;
        });
    }
}