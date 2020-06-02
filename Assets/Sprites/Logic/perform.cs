using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class perform:IComparable
{
    public float timeTurn
    {
        set
        {
            if (value < 0)
            {
                m_timeturn = 0;
            }
            else if (value > 1)
            {
                m_timeturn = 1;
            }
            else
            {
                m_timeturn = value;
            }
        }
        get
        {
            return m_timeturn;
        }
    }
    protected float m_timeturn;
    public abstract void Play();

    public int CompareTo(object obj)
    {
        perform p = obj as perform;
        return this.timeTurn.CompareTo(p.timeTurn);
    }
}

public class PerformAnima : perform
{
    //动画主角，0是player 1是monster
    public int charactor;
    public int animation;
    public float animaspeed;
    public PerformAnima() { }
    public PerformAnima(int c,int a,float speed){
        charactor = c;
        animation = a;
        animaspeed = speed;
    }
    public PerformAnima(int c,int a,float speed,float time){
        charactor = c;
        animation = a;
        animaspeed = speed;
        timeTurn = time;
    }
    public override void Play()
    {
        if (charactor == 0)
        {
            gameManager.Instance.battlemanager.realplayer.changeAnimation(animation);
        }
        else if (charactor == 1)
        {
            gameManager.Instance.battlemanager.realenemy.changeAnimation(animation);
        }
    }
}

public class PerformEffect:perform
{
    //特效种类，0在主角位置，1在敌人位置，2从主角到敌人，3从敌人到主角，4主角武器位置
    public int kind;
    public GameObject effect;
    public float speed;
    public float lasttime;
    public PerformEffect(){}
    public PerformEffect(int _kind,GameObject _effect,float _speed,float time)
    {
        kind = _kind;
        effect = _effect;
        speed = _speed;
        lasttime = time;
    }
    public PerformEffect(int _kind,GameObject _effect,float _speed,float time,float ttrun)
    {
        kind = _kind;
        effect = _effect;
        speed = _speed;
        lasttime = time;
        timeTurn = ttrun;
    }

    public override void Play()
    {
        //创建特效
        gameManager.Instance.instantiatemanager.instanPerformEffect(this);
    }
}