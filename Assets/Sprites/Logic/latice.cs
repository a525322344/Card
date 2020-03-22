using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LaticeState
{
    NotExploit,      //未开发的
    Exploit,        //已解锁的
    CanInstall,     //可以安装
    Install,        //安装
}

//latice 栅格
[System.Serializable]
public class latice
{
    public latice(int xposi, int yposi, bool isexploit)
    {
        position = new Vector2(xposi, yposi);
        if (isexploit)
        {
            state = LaticeState.Exploit;
        }
        else
        {
            state = LaticeState.NotExploit;
        }
    }
    public latice(Vector2 posi, bool isexploit)
    {
        position = posi;
        if (isexploit)
        {
            state = LaticeState.Exploit;
        }
        else
        {
            state = LaticeState.NotExploit;
        }
    }
    //储存位置
    public Vector2 position;
    //决定上限，是否开发
    public LaticeState state;
}