//卡的效果脚本
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//效果的枚举量
//对应了其产生的event及对其产生影响的reaction
public enum EventKind
{
    Event_Damage,
    Event_Armor,
    Event_PlayCard,
    Event_Discard,
    Event_DrawCard,
    Event_RoundStartDrawCard,
}

//对卡牌效果的委托
public delegate void DeleCardEffect(int num,battleInfo battleinfo);
//public delegate bool DeleIsorNot(int num, battleInfo battleInfo);

[System.Serializable]
public class Effect
{
    public DeleCardEffect istrue;
    public cardEffectBase cardEffect;
}
//抽象效果类，派生出所有效果
public abstract class EffectBase
{
    public abstract void DealEffect(int num, battleInfo battleInfo);

    public int getNum()
    {
        return num;
    }
    public EventKind GetEventKind()
    {
        return eventkind;
    }
    public DeleCardEffect getCardEffect()
    {
        return effectDele;
    }

    protected int num;
    protected DeleCardEffect effectDele;
    protected EventKind eventkind;          //该效果创建的事件类型
}

//一个效果抽象基类，派生出每个卡牌效果
[System.Serializable]
public abstract class cardEffectBase:EffectBase
{
    public abstract string DescribeEffect(int _i);
}
////该效果是空效果子类，是为了统合调用，表明CardEvent的种类
public abstract class emptyKind : cardEffectBase
{
    
}

//打出一张卡
public class emplyPlayCard:emptyKind
{
    public emplyPlayCard(){
        eventkind = EventKind.Event_PlayCard;
    }
    public override string DescribeEffect(int _i){ return ""; }
    public override void DealEffect(int num, battleInfo battleInfo) { }
}


public class Damage : cardEffectBase
{
    public Damage(int _num=0)
    {
        num = _num;
        effectDele= new DeleCardEffect(AllAsset.effectAsset.dealDemage);
        eventkind = EventKind.Event_Damage;
    }
    public override string DescribeEffect(int _i)
    {
        string result = "";
        result += "造成" + _i + "点伤害";
        return result;
    }
    public override void DealEffect(int newnum,battleInfo battleinfo)
    {
        effectDele(newnum, battleinfo);
    }
}

public class Armor : cardEffectBase
{
    public Armor(int _num=0)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.gainArmor);
        eventkind = EventKind.Event_Armor;
    }
    public override string DescribeEffect(int _i)
    {
        string result = "";
        result += "获得" + _i + "点护盾";
        return result;
    }
    public override void DealEffect(int newnum, battleInfo battleinfo)
    {
        effectDele(newnum, battleinfo);
    }
}

public class DrawCard : cardEffectBase
{
    public DrawCard(int _num=0)
    {
        num = _num;
    }
    public override string DescribeEffect(int _i)
    {
        string result = "";
        result += "抽" + _i + "张卡";
        return result;
    }
    public override void DealEffect(int num, battleInfo battleInfo)
    {
        //抽卡效果
        //
    }
}

public class RoundStartDrawCard:EffectBase
{
    public RoundStartDrawCard(int _num)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.drawCard);
        eventkind = EventKind.Event_RoundStartDrawCard;
    }
    public override void DealEffect(int num, battleInfo battleInfo)
    {
        effectDele(num, battleInfo);
    }
}