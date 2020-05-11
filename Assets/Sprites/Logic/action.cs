using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ACTIONKIND
{
    Deffaut,
    Attack,
    Defense,
    StrongUP,
    Debuff,
    Combin,
}
public static class ActionOperation
{
    //检查是否有攻击意图
    public static bool IsActionHasAttack(actionAbstract action)
    {
        bool result = false;
        foreach(var ae in action.effects)
        {
            if (ae.GetEventKind() == EventKind.Event_PlayerGetHurt)
            {
                result = true;
            }
        }
        return result;
    }
}
//怪物行动的抽象类
[System.Serializable]
public abstract class actionAbstract
{
    public ACTIONKIND Kind;
    public int times;
    public int num;
    public List<EffectBase> effects = new List<EffectBase>();
    public virtual void DoAction(int num, battleInfo info)
    {
        effects[0].DealEffect(num, info);
    }
    public List<actionAbstract> actionList = new List<actionAbstract>();
}

public class actionHurt : actionAbstract
{
    public actionHurt(int n)
    {
        times = 1;
        num = n;
        Kind = ACTIONKIND.Attack;
        effects.Add(new effectActionHurt(n));
    }
    public actionHurt(int n,int num)
    {
        times = num;
        Kind = ACTIONKIND.Attack;
        for(int i = 0; i < times; i++)
        {
            effects.Add(new effectActionHurt(n));
        }
    }
}

public class actionArmor : actionAbstract
{
    public actionArmor(int n)
    {
        Kind = ACTIONKIND.Defense;
        effects.Add(new effectActionEnemyArmor(n));
    }
}

public class actionAdmix : actionAbstract
{
    public actionAdmix(params actionAbstract[] actions)
    {
        Kind = ACTIONKIND.Combin;
        foreach(actionAbstract actionab in actions)
        {
            ListOperation.InsertList<EffectBase>(effects, actionab.effects);
            actionList.Add(actionab);
        }
    }
    public override void DoAction(int n,battleInfo info)
    {
        foreach(EffectBase effect in effects)
        {
            effect.DealEffect(n, info);
        }
    }
}