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
    Event_DrawACard,
    Event_RoundStartDrawCard,
    Event_PlayerGetHurt,
    Event_EnemyGetArmor,
    Event_Action,
}

//对卡牌效果的委托
public delegate void DeleCardEffect(int num,battleInfo battleinfo);
//public delegate bool DeleIsorNot(int num, battleInfo battleInfo);

//抽象效果类，派生出所有效果
public abstract class EffectBase
{
    public virtual void DealEffect(int num, battleInfo battleInfo)
    {
        effectDele(num, battleInfo);
    }
    public virtual string DescribeEffect() { return ""; }
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
    protected int mixnum;
    protected DeleCardEffect effectDele;
    public bool b_hasChildEffect = false;
    public List<EffectBase> childeffects = new List<EffectBase>();
    protected EventKind eventkind;          //该效果创建的事件类型
}
//一个效果抽象基类，派生出每个卡牌效果
public abstract class cardEffectBase : EffectBase
{
}
//系统效果抽象基类
public abstract class systemEffectBase : EffectBase
{

}
//怪物行为效果抽象基类
public abstract class actionEffectBase : EffectBase
{

}
////该效果是空效果子类，是为了统合调用，表明CardEvent的种类
public abstract class emptyKind : cardEffectBase
{
    
}

////
//卡牌事件效果
//打出一张卡
public class emplyPlayCard:emptyKind
{
    public emplyPlayCard(){
        eventkind = EventKind.Event_PlayCard;
    }
}

////
//卡牌效果
public class Damage : cardEffectBase
{
    public Damage(int _num=0)
    {
        num = _num;
        mixnum = num;
        effectDele= new DeleCardEffect(AllAsset.effectAsset.EnemyGetHurt);
        eventkind = EventKind.Event_Damage;
    }
    public override string DescribeEffect()
    {
        string result = "";
        result += "造成" + mixnum + "点伤害";
        return result;
    }
}
public class Repeat : cardEffectBase
{
    public Repeat() { }
    public Repeat(int _num,params EffectBase[] effects)
    {
        effectDele = new DeleCardEffect((a, b) => { });
        num = _num;
        mixnum = num;
        foreach(EffectBase effect in effects)
        {
            childeffects.Add(effect);
        }
        b_hasChildEffect = true;
    }
    public override string DescribeEffect()
    {
        string result = "";
        foreach (EffectBase effect in childeffects)
        {
            result += effect.DescribeEffect() + ",";
        }
        result += "重复" + mixnum + "次";
        return result;
    }
}
public class Armor : cardEffectBase
{
    public Armor(int _num=0)
    {
        num = _num;
        mixnum = num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.PlayerGetArmor);
        eventkind = EventKind.Event_Armor;
    }
    public override string DescribeEffect()
    {
        string result = "";
        result += "获得" + mixnum + "点护盾";
        return result;
    }
}

public class drawACard : cardEffectBase
{
    public drawACard()
    {
        num = 0;
        mixnum = 0;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.drawACard);
        eventkind = EventKind.Event_DrawACard;
    }
    public override string DescribeEffect()
    {
        return "Test:抽一张卡";
    }
}

public class DrawCard : Repeat
{
    public DrawCard(int _num)
    {
        num = _num;
        mixnum = num;
        effectDele = new DeleCardEffect((a, b) => { });
        childeffects.Add(new drawACard());
        eventkind = EventKind.Event_DrawCard;
        b_hasChildEffect = true;
    }
    public override string DescribeEffect()
    {
        string result = "";
        result += "抽" + mixnum + "张卡";
        return result;
    }
}

////
//系统效果
public class RoundStartDrawCard:systemEffectBase
{
    public RoundStartDrawCard(int _num)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.drawACard);
        eventkind = EventKind.Event_RoundStartDrawCard;
    }
}

public class RoundEndDisCard : systemEffectBase
{
    public RoundEndDisCard(int _num)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.disAllCard);
    }
}

////
//行为效果
//攻击玩家
public class effectActionHurt : actionEffectBase
{
    public effectActionHurt(int _num)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.PlayerGetHurt);
        eventkind = EventKind.Event_PlayerGetHurt;
    }
}
//获得护甲
public class effectActionEnemyArmor : actionEffectBase
{
    public effectActionEnemyArmor(int _num)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.EnemyGetArmor);
        eventkind = EventKind.Event_EnemyGetArmor;
    }
}