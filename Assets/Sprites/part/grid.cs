using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class grid 
{
    //构造函数
    public grid(bool open)
    {
        m_Opening = open;
        if (open)
        {
            m_Power = true;
        }
        else
        {
            m_Power = false;
        }
    }
    //坐标
    public void setPosition(int x,int y)
    {
        m_PositionX = x;
        m_PositionY = y;
    }
    public Vector3 getPosition()
    {
        Vector3 result = new Vector3(m_PositionX - 1, m_PositionY - 1, 0);
        return result;
    }

    public bool Opening {
        get;
        set;
    }
    public bool Power
    {
        get { return m_Power; }
        set { m_Power = value; }
    }

    private int m_PositionX;
    private int m_PositionY;
    private bool m_Opening;
    private bool m_Power;
    private gridCover m_gridcover;

}

public abstract class gridCover
{
    public abstract void dealCover();
    public singleEvent coverEvent;
}