//卡的效果脚本
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//效果的枚举量
//对应了其产生的event及对其产生影响的reaction
public enum EventKind
{
    Event_OutOfKind,            //默认null效果
    Event_NULL,                 //一些效果觉得不会有对其有反应的，就先用NULL;
    Event_Damage,               //伤害
    Event_Armor,                //护甲
    Event_PlayCard,             //打出卡片,"抽象效果"
    Event_Discard,              //弃牌     "抽象效果"
    Event_DrawCard,             //抽牌(repeat)
    Event_DrawACard,            //抽一张牌
    Event_RoundStartDrawCard,   //回合开始抽牌
    Event_RoundEndDisCard,      //回合结束弃牌
    Event_PlayerGetHurt,        //玩家受到伤害
    Event_EnemyGetArmor,        //怪物获得护甲
    Event_Action,               //怪物行动  "抽象效果"
    Event_Repeat,               //重复，包含子效果
    Event_SystmeRepeat,         //系统重复
    Event_EnemyGetBurn,         //获得灼烧  "抽象效果"
    Event_EnemyBurnDamage,      //灼烧伤害(穿透
    Event_LinkRandom,           //随机链接
    Event_Whether               //条件效果
}

//对卡牌效果的委托
public delegate void DeleCardEffect(int num,battleInfo battleinfo);


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
    public void SetNum(int _n)
    {
        num = _n;
    }

    protected int num;
    public int mixnum;
    public bool b_hideDesctibe = false;
    public string frontDesctibe;
    public string backDesctibe;

    protected DeleCardEffect effectDele;
    
    public bool b_hasChildEffect = false;
    public List<EffectBase> childeffects = new List<EffectBase>();
    public void AddChildEffect(EffectBase effect)
    {
        childeffects.Add(effect);
    }
    //如果
    public bool b_judgeEffect = false;
    public List<judgeCondition> judgeConditions = new List<judgeCondition>();
    public bool JudgeWhether(battleInfo battleinfo)
    {
        bool result = true;
        foreach(judgeCondition judge in judgeConditions)
        {
            if (!judge.Whether(battleinfo))
            {
                result = false;
            }
        }
        return result;
    }
    public void AddJudge(judgeCondition judge)
    {
        judgeConditions.Add(judge);
    }

    protected EventKind eventkind;          //该效果创建的事件类型
}
//一个效果抽象基类，派生出每个卡牌效果
public abstract class cardEffectBase : EffectBase
{
    protected string colorRed = "<color=red>";
    protected string colorGreen = "<color=green>";
    protected string colorEnd = "</color>";
    public override string DescribeEffect()
    {
        string result = "";
        result += frontDesctibe;
        if (mixnum > num)
        {
            result += colorGreen + mixnum + colorEnd;
        }
        else if (mixnum < num)
        {
            result += colorRed + mixnum + colorEnd;
        }
        else
        {
            result += mixnum;
        }
        result += backDesctibe;
        return result;
    }
}
//系统效果抽象基类
public abstract class stateEffectBase:EffectBase
{

}
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


//卡牌事件效果
//打出一张卡
public class emplyPlayCard:emptyKind
{
    public emplyPlayCard(){
        eventkind = EventKind.Event_PlayCard;
    }
}


//卡牌效果
public class Damage : cardEffectBase
{
    public Damage(int _num=0)
    {
        num = _num;
        mixnum = num;
        effectDele= new DeleCardEffect(AllAsset.effectAsset.EnemyGetHurt);
        eventkind = EventKind.Event_Damage;

        frontDesctibe = "造成";
        backDesctibe = "点伤害";
    }
}
//重复效果，有子效果表
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
        eventkind = EventKind.Event_Repeat;

        frontDesctibe = "重复";
        backDesctibe = "次";
    }
    public override string DescribeEffect()
    {
        string result = "";
        foreach (EffectBase effect in childeffects)
        {
            result += effect.DescribeEffect() + ",";
        }
        result += frontDesctibe;
        if (mixnum > num)
        {
            result += colorGreen + mixnum + colorEnd;
        }
        else if (mixnum < num)
        {
            result += colorRed + mixnum + colorEnd;
        }
        else
        {
            result += mixnum;
        }
        result += backDesctibe;
        return result;
    }
}
//玩家获得护甲
public class Armor : cardEffectBase
{
    public Armor(int _num=0)
    {
        num = _num;
        mixnum = num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.PlayerGetArmor);
        eventkind = EventKind.Event_Armor;

        frontDesctibe = "获得";
        backDesctibe = "点护甲";
    }
}
//抽一张卡
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
        return "子效果，抽一张卡";
    }
}
//抽num张卡
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

        frontDesctibe = "抽";
        backDesctibe = "张牌";
    }
    public override string DescribeEffect()
    {
        string result = "";
        result += frontDesctibe;
        if (mixnum > num)
        {
            result += colorGreen + mixnum + colorEnd;
        }
        else if (mixnum < num)
        {
            result += colorRed + mixnum + colorEnd;
        }
        else
        {
            result += mixnum;
        }
        result += backDesctibe;
        return result;
    }
}
//敌人获得灼伤
public class Burn : cardEffectBase
{
    public Burn(int _num = 0)
    {
        num = _num;
        mixnum = num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.EnemyGetBurn);
        eventkind = EventKind.Event_EnemyGetBurn;

        frontDesctibe = "给与敌人";
        backDesctibe = "层灼烧";
    }
}
//随机链接部件
public class LinkRandom : cardEffectBase
{
    public LinkRandom(int _num = 2)
    {
        num = _num;
        mixnum = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.RandomLinkPart);
        eventkind = EventKind.Event_LinkRandom;

        frontDesctibe = "本回合内随机链接";
        backDesctibe = "个部件";
    }
}
public class CardEffect_ToExitLink : cardEffectBase
{
    public CardEffect_ToExitLink()
    {
        b_hideDesctibe = true;
        eventkind = EventKind.Event_NULL;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.CreatState_ExitLinkPart);
    }
}
//判断效果 
public class Effect_Whether : EffectBase
{
    public Effect_Whether()
    {
        b_judgeEffect = true;   
    }
}
public class CardEffect_Whether : cardEffectBase
{
    public CardEffect_Whether(judgeCondition judge,EffectBase effect)
    {
        b_judgeEffect = true;
        judgeConditions.Add(judge);
        childeffects.Add(effect);
        effectDele = new DeleCardEffect((a, b) => { });
        eventkind = EventKind.Event_Whether;
    }
    public CardEffect_Whether()
    {
        b_judgeEffect = true;
    }
    public override string DescribeEffect()
    {
        string describe = "如果";
        foreach(judgeCondition j in judgeConditions)
        {
            describe += j.describe + ",";
        }
        describe += "则";
        foreach(EffectBase e in childeffects)
        {
            describe += e.DescribeEffect() + ",";
        }
        describe=describe.Substring(0, describe.Length - 1);
        return describe;
    }
}



public class CardEffect_DisCard : cardEffectBase
{
    public CardEffect_DisCard(int _num)
    {
        num = _num;
        mixnum = _num;

    }
}
////
//系统效果 一般排除在卡的影响效果外；
//比如，“每当弃一张卡”，回合结束弃卡效果不触发这种reaction;
//换句话说，Event_kind不同
public class SystemRepeat : systemEffectBase
{
    public SystemRepeat() { }
    public SystemRepeat(int _num,params EffectBase[] effects)
    {
        effectDele = new DeleCardEffect((a, b) => { });
        num = _num;
        mixnum = _num;
        foreach(EffectBase effect in effects)
        {
            childeffects.Add(effect);
        }
        b_hasChildEffect = true;
        eventkind = EventKind.Event_SystmeRepeat;
    }
}
//回合开始抽卡
public class RoundStartDrawCard:SystemRepeat
{
    public RoundStartDrawCard(int _num)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.EmptyEffect);
        childeffects.Add(new drawACard());
        b_hasChildEffect = true;
        eventkind = EventKind.Event_RoundStartDrawCard;
    }
    public override string DescribeEffect()
    {
        return "回合开始抽卡";
    }
}
//回合结束弃卡
public class RoundEndDisCard : systemEffectBase
{
    public RoundEndDisCard(int _num)
    {
        num = _num;
        eventkind = EventKind.Event_RoundEndDisCard;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.disAllCard);
    }
    public override string DescribeEffect()
    {
        return "回合结束，弃掉所有手牌";
    }
}

////
//行为效果
//攻击玩家，玩家受到伤害
public class effectActionHurt : actionEffectBase
{
    public effectActionHurt(int _num)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.PlayerGetHurt);
        eventkind = EventKind.Event_PlayerGetHurt;
    }
    public override string DescribeEffect()
    {
        return "怪物造成伤害：" + num;
    }
}
//怪物获得护甲
public class effectActionEnemyArmor : actionEffectBase
{
    public effectActionEnemyArmor(int _num)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.EnemyGetArmor);
        eventkind = EventKind.Event_EnemyGetArmor;
    }
    public override string DescribeEffect()
    {
        return "怪物获得护甲：" + num;
    }
}

//状态效果
//添加给状态事件，状态事件由状态reaction反应添加
public class effectBurnDamage : stateEffectBase
{
    public effectBurnDamage(int _num=0)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.EnemyGetRealHurt);
        eventkind = EventKind.Event_EnemyBurnDamage;
    }
    public override string DescribeEffect()
    {
        return "灼烧伤害：" + num;
    }
}

public class StateEffect_ExitLinkPart : stateEffectBase
{
    public StateEffect_ExitLinkPart()
    {
        effectDele = new DeleCardEffect(AllAsset.effectAsset.ExitLinkPark);
        eventkind = EventKind.Event_NULL;
    }
}