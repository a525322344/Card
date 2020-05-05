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
        buttons.Add(new Button_ExitBefall("普通的退出"));
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
public class secondBoardInfo
{
    public int order;
    public string describe;
    public secondBoardInfo(int _o)
    {
        order = _o;
    }
    public secondBoardInfo(int _o,string str)
    {
        describe = str;
        order = _o;
    }
    //作为second Board
    public toDo onEnter;
    public toDo onExit;
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
//通用按钮
public class Button_Info : buttoninfo
{
    public Button_Info(string des,buttonTo buttonTo)
    {
        buttonFun = new buttonTo(() =>
        {
        });
        buttonDescribe = des;
        buttonFun += buttonTo;
    }
}
//通用离开——关闭事件页
public class Button_Exit : buttoninfo
{
    public Button_Exit(string des, buttonTo buttonTo)
    {
        buttonFun = new buttonTo(() =>
        {
            gameManager.Instance.uimanager.uiBefallBoard.ExitEventBoard();
            gameManager.Instance.mapmanager.EventWindow(false);// mapState = MapState.MainMap;
        });
        buttonDescribe = des;
        buttonFun += buttonTo;
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
            gameManager.Instance.mapmanager.EventWindow(false); //mapState = MapState.MainMap;
        });
    }
    public Button_ExitBefall()
    {
        buttonDescribe = "退出";
        //退出遭遇
        buttonFun = new buttonTo(() =>
        {
            gameManager.Instance.uimanager.uiBefallBoard.ExitEventBoard();
            gameManager.Instance.mapmanager.EventWindow(false); //mapState = MapState.MainMap;
        });
    }
}
//进入下一个事件
public class Button_NextBeffal:buttoninfo
{
    public befallinfo nextBefall;
    public Button_NextBeffal()
    {
        buttonFun = new buttonTo(() =>
        {
            gameManager.Instance.uimanager.uiBefallBoard.EnterEventBoard(nextBefall);
        });
    }
}
//进入下一级页面
public class Button_SecondBoard : buttoninfo
{
    public secondBoardInfo secondInfo;
    public Button_SecondBoard(secondBoardInfo secondBoardInfo,bool exitToMap=true)
    {
        secondInfo = secondBoardInfo;
        buttonDescribe = secondBoardInfo.describe;
        buttonFun = new buttonTo(() =>
          {
              gameManager.Instance.uimanager.uiBefallBoard.ExitEventBoard();
              GameObject sbui= gameManager.Instance.instantiatemanager.instanSecondBoard(secondInfo);
              if (exitToMap)
              {
                  sbui.GetComponent<uiSecondBoard>().exitToDo += () =>
                  {
                      gameManager.Instance.mapmanager.EventWindow(false); //mapState = MapState.MainMap;
                  };
              }
          });
    }
}

