//卡的效果脚本
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//对卡牌效果的委托
public delegate void DeleCardEffect(int num,battleInfo battleinfo);

//弃用
//[System.Serializable]
//public class Effectclass
//{
//    public CardEffect cardEffect;
//    public int num;

//    public Effectclass(CardEffect _cardEffect, int _num)
//    {
//        cardEffect = _cardEffect;
//        num = _num;
//    }
//}

//一个效果抽象基类，派生出每个卡牌效果
[System.Serializable]
public abstract class cardEffectBase
{
    public abstract DeleCardEffect getEffect();
    public abstract string DescribeEffect(int _i);
    public abstract void DealEffect(int num,battleInfo battleInfo); 
    public int getNum()
    {
        return num;
    }
    public DeleCardEffect getCardEffect()
    {
        return effectDele;
    }

    protected int num;
    protected DeleCardEffect effectDele;
}

public class Damage : cardEffectBase
{
    public Damage(int _num)
    {
        num = _num;
        effectDele= new DeleCardEffect(AllAsset.effectAsset.dealDemage);
    }
    public override string DescribeEffect(int _i)
    {
        string result = "";
        result += "造成" + _i + "点伤害";
        return result;
    }
    public override DeleCardEffect getEffect()
    {
        return effectDele;
    }
    public override void DealEffect(int newnum,battleInfo battleinfo)
    {
        effectDele(num,battleinfo);
    }
}

public class Deffence : cardEffectBase
{
    public Deffence(int _num)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.gainArmor);
    }
    public override string DescribeEffect(int _i)
    {
        string result = "";
        result += "获得" + _i + "点护盾";
        return result;
    }
    public override DeleCardEffect getEffect()
    {
        return effectDele;
    }
    public override void DealEffect(int newnum, battleInfo battleinfo)
    {
        effectDele(num, battleinfo);
    }
}