using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ACTIONKIND
{
    Attack,
    Defense,
    StrongUP,
    Debuff,
    Combin,
}
//怪物行动的抽象类
public abstract class actionAbstract
{
    public ACTIONKIND Kind;
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
        Kind = ACTIONKIND.Attack;
        effects.Add(new effectActionHurt(n));
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