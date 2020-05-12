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
            if (ae.GetEventKind() == EventKind.Event_MonsterHurt)
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
        effects.Add(new ActionEffect_MonsterHurt(n));
    }
    public actionHurt(int n,int timess)
    {
        times = timess;
        num = n;
        Kind = ACTIONKIND.Attack;
        for(int i = 0; i < times; i++)
        {
            effects.Add(new ActionEffect_MonsterHurt(n));
        }
    }
}

public class actionDebuff : actionAbstract
{
    public actionDebuff(EffectBase effect)
    {
        times = 1;
        num = 1;
        Kind = ACTIONKIND.Debuff;
        effects.Add(effect);
    }
}

public class actionPowerUp : actionAbstract
{
    public actionPowerUp(EffectBase effect)
    {
        times = 1;
        num = 1;
        Kind = ACTIONKIND.StrongUP;
        effects.Add(effect);
    }
}

public class actionArmor : actionAbstract
{
    public actionArmor(int n)
    {
        Kind = ACTIONKIND.Defense;
        effects.Add(new ActionEffect_MonsterArmor(n));
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