using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
//grid应该不储存位置信息，位置信息就该存在part中，残留先留着
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

    public bool Opening {
        get;
        set;
    }
    public bool Power
    {
        get { return m_Power; }
        set { m_Power = value; }
    }

    private bool m_Opening;
    private bool m_Power;


    public Vector2 position;
    public MagicPart fatherPart;
}

public abstract class gridCover
{
    public abstract void dealCover();
    public singleEvent coverEvent;
}