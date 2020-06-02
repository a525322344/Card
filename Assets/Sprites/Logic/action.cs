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
    public List<perform> performList = new List<perform>();
}

public class actionHurt : actionAbstract
{
    public actionHurt(int n)
    {
        times = 1;
        num = n;
        Kind = ACTIONKIND.Attack;
        performList.Add(new PerformAnima(1, 2, 1,0.15f));
        performList.Add(new PerformAnima(0, 3, 1,0.7f));

        effects.Add(new ActionEffect_MonsterHurt(n));
    }
    public actionHurt(int n,int timess,params perform[] performs)
    {
        times = timess;
        num = n;
        Kind = ACTIONKIND.Attack;
        performList.Add(new PerformAnima(1, 2, 1, 0.15f));
        performList.Add(new PerformAnima(0, 3, 1, 0.7f));
        for (int i = 0; i < times; i++)
        {
            effects.Add(new ActionEffect_MonsterHurt(n));
        }
        foreach(perform per in performs)
        {
            performList.Add(per);
        }
    }
}

public class actionDebuff : actionAbstract
{
    public actionDebuff(EffectBase effect, params perform[] performs)
    {
        times = 1;
        num = 1;
        Kind = ACTIONKIND.Debuff;
        performList.Add(new PerformAnima(1, 2, 1, 0.1f));
        performList.Add(new PerformEffect(1, instantiateManager.instance.EffectGOList[9], 0, 1, 0.3f));
        effects.Add(effect);
        foreach (perform per in performs)
        {
            performList.Add(per);
        }
    }
}

public class actionPowerUp : actionAbstract
{
    public actionPowerUp(EffectBase effect, params perform[] performs)
    {
        times = 1;
        num = 1;
        Kind = ACTIONKIND.StrongUP;
        performList.Add(new PerformEffect(1, instantiateManager.instance.EffectGOList[9], 0, 1, 0.3f));
        performList.Add(new PerformAnima(1, 4, 1, 0.1f));
        effects.Add(effect);
        foreach (perform per in performs)
        {
            performList.Add(per);
        }
    }
}

public class actionArmor : actionAbstract
{
    public actionArmor(int n, params perform[] performs)
    {
        Kind = ACTIONKIND.Defense;
        performList.Add(new PerformAnima(1, 3, 1, 0.1f));
        performList.Add(new PerformEffect(1, instantiateManager.instance.EffectGOList[0], 0, 1, 0.3f));
        effects.Add(new ActionEffect_MonsterArmor(n));
        foreach (perform per in performs)
        {
            performList.Add(per);
        }
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
            foreach(perform per in actionab.performList)
            {
                performList.Add(per);
            }
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