using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public delegate void buttonTo();

public class befallinfo
{
    public befallinfo()
    {

    }
    public string befallName;
    public int textureOrder;
    public string befallDescribe;
    public List<buttoninfo> buttons = new List<buttoninfo>();
}

public abstract class buttoninfo
{
    public string buttonDescribe;
    public buttonTo buttonFun;
}

public class Button_ExitBefall : buttoninfo
{
    public Button_ExitBefall(string buttondes)
    {
        buttonDescribe = buttondes;
        //退出遭遇
        buttonFun = new buttonTo(() =>
        {

        });
    }
    public Button_ExitBefall()
    {
        buttonDescribe = "回避";
        //退出遭遇
        buttonFun = new buttonTo(() =>
        {

        });
    }
}