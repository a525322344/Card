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
    Event_Discard,              //弃牌     "系统抽象效果"
    Event_DrawCard,             //抽牌(repeat)
    Event_DrawACard,            //抽一张牌
    Event_RoundStartDrawCard,   //回合开始抽牌
    Event_RoundEndDisCard,      //回合结束弃牌
    Event_Repeat,               //重复，包含子效果
    Event_SystmeRepeat,         //系统重复
    Event_EnemyGetBurn,         //获得灼烧  "抽象效果"
    Event_EnemyBurnMultiply,    //灼烧翻n倍
    Event_EnemyBurnDamage,      //灼烧伤害(穿透
    Event_LinkRandom,           //随机链接
    Event_Whether,              //条件效果
    Event_DisOneCard,           //弃一张卡
    Event_DisSomeCard,          //卡牌弃卡
    Event_Fill,                 //触发补齐后的效果
    Event_Exhaust,              //耗尽
    //怪物行为
    Event_MonsterHurt,         //玩家受到伤害
    Event_MonsterArmor,        //怪物获得护甲
    Event_MonsterAction,       //怪物行动  "抽象效果"
    Event_MonsterBurn,
    //Event_MonsterBurnDebuff,(用null
    Event_PlayerBurnDamage,
}

//对卡牌效果的委托
public delegate void DeleCardEffect(int num,battleInfo battleinfo,out int returnnum);


//抽象效果类，派生出所有效果
public abstract class EffectBase
{
    public virtual void DealEffect(int num, battleInfo battleInfo)
    {
        effectDele(num, battleInfo,out returnnum);
    }
    public virtual string DescribeEffect() { return ""; }
    public virtual void InitNum() { }
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
    public int returnnum;
    public bool b_hideDesctibe = false;
    public string frontDesctibe;
    public string backDesctibe;
    public string returnDescribe;

    protected DeleCardEffect effectDele;
    
    public bool b_hasChildEffect = false;
    public List<EffectBase> childeffects = new List<EffectBase>();
    public void AddChildEffect(EffectBase effect)
    {
        childeffects.Add(effect);
    }
    //条件效果
    public bool b_judgeEffect = false;
    //停顿效果
    public bool b_stopEffect = false;
    public DeleCardEffect preEffect;
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
    //部件
    public MagicPart magicpart;

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
//衍生的打出卡牌事件
public class emplyVPlayCard : emptyKind
{
    public emplyVPlayCard()
    {
        eventkind = EventKind.Event_NULL;
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
//伤害增加judge返回值
public class CardEffect_DamageByJudge:cardEffectBase
{
    public judgeCondition numjudge;
    int initnum;
    public CardEffect_DamageByJudge(int _num,judgeCondition judge)
    {
        num = _num;
        mixnum = _num;
        initnum = _num;
        effectDele = AllAsset.effectAsset.EnemyGetHurt;
        eventkind = EventKind.Event_Damage;
        judgeConditions.Add(judge);
        b_judgeEffect = true;
        numjudge = judge;

        frontDesctibe = "造成";
        backDesctibe = "点伤害";
    }
    public CardEffect_DamageByJudge(int _num)
    {
        num = _num;
        mixnum = _num;
        initnum = _num;
        effectDele = AllAsset.effectAsset.EnemyGetHurt;
        eventkind = EventKind.Event_Damage;
        b_judgeEffect = true;

        frontDesctibe = "造成";
        backDesctibe = "点伤害";
    }
    public override void InitNum()
    {
        num = numjudge.returnNum + initnum;
    }
    public override string DescribeEffect()
    {
        return base.DescribeEffect()+numjudge.describe+",伤害+1";
    }
}

public class CardEffect_DamageByPartPower : cardEffectBase
{
    int beishu;
    public CardEffect_DamageByPartPower(int _num)
    {
        beishu = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.EnemyGetHurt);
        eventkind = EventKind.Event_Damage;

        frontDesctibe = "造成部件剩余能量*";
        backDesctibe = "的伤害";
    }
    public override void InitNum()
    {
        num = gameManager.Instance.battlemanager.lastPart.gridpower*beishu;
    }
    public override string DescribeEffect()
    {
        return frontDesctibe + beishu + backDesctibe;
    }
}
//重复效果，有子效果表
public class Repeat : cardEffectBase
{
    public Repeat() { }
    public Repeat(int _num,params EffectBase[] effects)
    {
        effectDele = new DeleCardEffect((int a, battleInfo b, out int c) => { c = a; });
        num = _num;
        mixnum = num;
        foreach(EffectBase effect in effects)
        {
            childeffects.Add(effect);
        }
        b_hasChildEffect = true;
        eventkind = EventKind.Event_Repeat;

        frontDesctibe = "<b>重复</b>";
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
public class CardEffect_ArmorByPartPower:cardEffectBase
{
    int beishu;
    public CardEffect_ArmorByPartPower(int _num = 1)
    {
        beishu = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.PlayerGetArmor);
        eventkind = EventKind.Event_Armor;

        frontDesctibe = "获得部件剩余能量*";
        backDesctibe = "的护甲";
    }
    public override void InitNum()
    {
        num = gameManager.Instance.battlemanager.lastPart.gridpower* beishu;
    }
    public override string DescribeEffect()
    {
        return frontDesctibe + beishu + backDesctibe;
    }
}
//部件产生的护甲(依据剩余能量
public class PartEffect_Armor : cardEffectBase
{
    int beishu;
    public PartEffect_Armor(int bei,MagicPart part)
    {
        effectDele = new DeleCardEffect(AllAsset.effectAsset.PlayerGetArmor);
        eventkind = EventKind.Event_Armor;
        magicpart = part;
        beishu = bei;
    }
    public override void InitNum()
    {
        num = magicpart.gridpower * beishu;
    }
    public override string DescribeEffect()
    {
        return "获得部件剩余能量*" + beishu + "的护甲";
    }
}
//抽一张卡(不直接使用)
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
        effectDele = new DeleCardEffect((int a,battleInfo b,out int c) => { c = a; });
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
//敌人灼烧层数加倍
public class DoubleBurn : cardEffectBase
{
    public DoubleBurn(int _num = 0)
    {
        num = _num;
        mixnum = num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.EnemyDoubleBurn);
        eventkind = EventKind.Event_EnemyBurnMultiply;

        frontDesctibe = "使敌人的<b>灼烧</b>翻";
        backDesctibe = "倍";
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
        backDesctibe = "层<b>灼烧</b>";
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

        frontDesctibe = "本回合内随机<b>链接</b>";
        backDesctibe = "个部件";
    }
}
public class LinkThisWithRandom : cardEffectBase
{
    public LinkThisWithRandom(int _num = 1)
    {
        num = _num;
        mixnum = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.ThisPartLinkRandomPArt);
        eventkind = EventKind.Event_LinkRandom;

        frontDesctibe = "本回合内,此部件与随机";
        backDesctibe = "个部件<b>链接</b>";
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
    public override string DescribeEffect()
    {
        return "添加状态：回合结束取消链接效果";
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
        effectDele = new DeleCardEffect((int a, battleInfo b, out int c) => { c = a; });
        eventkind = EventKind.Event_Whether;
    }
    public CardEffect_Whether()
    {
        b_judgeEffect = true;
        effectDele = new DeleCardEffect((int a, battleInfo b, out int c) => { c = a; });
        eventkind = EventKind.Event_Whether;
    }
    public override string DescribeEffect()
    {
        string describe = "";
        foreach(judgeCondition j in judgeConditions)
        {
            describe += j.describe + ",";
        }
        foreach(EffectBase e in childeffects)
        {
            describe += e.DescribeEffect() + "并";
        }
        if (describe.Length != 0)
        {
            describe = describe.Substring(0, describe.Length - 1);
        }

        return describe;
    }
}
//重复效果，通过判断(弃用)
public class CardEffect_RepeatByJudge:cardEffectBase
{
    judgeCondition numJudge;
    public CardEffect_RepeatByJudge(judgeCondition judge,EffectBase effect)
    {
        b_hasChildEffect = true;
        numJudge = judge;
        childeffects.Add(effect);
    }
}
//事件复制
public class PartEffect_CopeLastCardEvent : cardEffectBase
{
    public PartEffect_CopeLastCardEvent(int _num)
    {
        num = _num;
        effectDele = AllAsset.effectAsset.CopyLastCard;
        eventkind = EventKind.Event_NULL;
    }
    public override string DescribeEffect()
    {
        return "额外再打出" + num + "次";
    }
}

//补齐后置效果
public class CardEffect_RepeatByFill : cardEffectBase
{
    judgeCondition numJudge;
    public CardEffect_RepeatByFill(judgeCondition judge, EffectBase effect)
    {
        b_hasChildEffect = true;
        effectDele = new DeleCardEffect((int a, battleInfo b, out int c) => { c = a; });
        numJudge = judge;
        childeffects.Add(effect);
        eventkind = EventKind.Event_Fill;
    }
    public CardEffect_RepeatByFill(judgeCondition judge)
    {
        numJudge = judge;
        b_hasChildEffect = true;
        effectDele = new DeleCardEffect((int a, battleInfo b, out int c) => { c = a; });
        eventkind = EventKind.Event_Fill;
    }
    public override void InitNum()
    {
        num = numJudge.returnNum;
    }
    public override string DescribeEffect()
    {
        string result = "";
        foreach (EffectBase effect in childeffects)
        {
            result += effect.DescribeEffect() + "并";
        }
        if (result.Length != 0)
        {
            result = result.Substring(0, result.Length - 1);
        }

        return result;
    }
}
//选择弃一张卡(不直接使用)
public class CardEffect_DisACard : cardEffectBase
{
    public CardEffect_DisACard()
    {
        effectDele = AllAsset.effectAsset.DisCardASelectedCard;
        num = 0;
        eventkind = EventKind.Event_DisOneCard;
    }
    public override string DescribeEffect()
    {
        return "子效果，弃一张卡";
    }
}
public class CardEffect_DisSomeCard : Repeat
{
    public CardEffect_DisSomeCard(int _num)
    {
        num = _num;
        b_stopEffect = true;
        b_hasChildEffect = true;
        judgeConditions.Add(new Judge_HaveSelectedHandCard());
        //preEffect = AllAsset.effectAsset.PreSelectCard;
        effectDele = AllAsset.effectAsset.PreSelectCard;
        childeffects.Add(new CardEffect_DisACard());
        eventkind = EventKind.Event_DisSomeCard;

        frontDesctibe = "弃";
        backDesctibe = "张卡";
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
public class CardEffect_DisAllCard : cardEffectBase
{
    public CardEffect_DisAllCard()
    {
        num = 0;
        b_stopEffect = false;
        b_hasChildEffect = true;
        effectDele= new DeleCardEffect((int a, battleInfo b, out int c) => { c = a; });
        childeffects.Add(new CardEffect_DisACard());
        eventkind = EventKind.Event_DisSomeCard;
        returnDescribe = "每弃一张手牌,";
    }
    public override string DescribeEffect()
    {
        return "弃掉全部手牌";
    }
    public override void InitNum()
    {
        num = gameManager.Instance.battlemanager.realCardList.Count;
    }
}
//随机从弃牌堆拿牌
public class CardEffect_GetCardFormDiscard : cardEffectBase
{
    public CardEffect_GetCardFormDiscard(int _num = 1)
    {
        num = _num;
        mixnum = num;
        eventkind = EventKind.Event_NULL;
        effectDele = AllAsset.effectAsset.RandomGetCardFromDiscard;
        frontDesctibe = "随机将弃牌堆的";
        backDesctibe = "张牌加入手中";
    }

}
//以效果返回值为参数的重复
public class CardEffect_RepeatByEffect:cardEffectBase
{
    public cardEffectBase numeffect;
    public CardEffect_RepeatByEffect(cardEffectBase effect)
    {
        numeffect = effect;
        b_hasChildEffect = true;
        effectDele= new DeleCardEffect((int a, battleInfo b, out int c) => { c = a; }); ;
        eventkind = EventKind.Event_Repeat;
    }
    public CardEffect_RepeatByEffect()
    {
        b_hasChildEffect = true;
        effectDele = new DeleCardEffect((int a, battleInfo b, out int c) => { c = a; }); ;
        eventkind = EventKind.Event_Repeat;
    }
    public override void InitNum()
    {
        num = numeffect.returnnum;
    }
    public override string DescribeEffect()
    {
        string des = "";
        des += numeffect.returnDescribe;
        foreach(cardEffectBase ef in childeffects)
        {
            des += ef.DescribeEffect();
        }
        return des;
    }
}
//
public class CardEffect_Exhaust : cardEffectBase
{
    public CardEffect_Exhaust()
    {
        effectDele = AllAsset.effectAsset.ExhaustCard;
        eventkind = EventKind.Event_Exhaust;
    }
    public override string DescribeEffect()
    {
        return "<b>耗尽</b>";
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
        effectDele = new DeleCardEffect((int a, battleInfo b, out int c) => { c = a; });
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
public class ActionEffect_MonsterHurt : actionEffectBase
{
    public ActionEffect_MonsterHurt(int _num)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.PlayerGetHurt);
        eventkind = EventKind.Event_MonsterHurt;
    }
    public override string DescribeEffect()
    {
        return "怪物造成伤害：" + num;
    }
}
//怪物获得护甲
public class ActionEffect_MonsterArmor : actionEffectBase
{
    public ActionEffect_MonsterArmor(int _num)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.EnemyGetArmor);
        eventkind = EventKind.Event_MonsterArmor;
    }
    public override string DescribeEffect()
    {
        return "怪物获得护甲：" + num;
    }
}

public class ActionEffect_MonsterBurn : actionEffectBase
{
    public ActionEffect_MonsterBurn(int _num)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.PlayerGetBurn);
        eventkind = EventKind.Event_MonsterBurn;
    }
    public override string DescribeEffect()
    {
        return "给与玩家灼烧：" + num;
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
public class StateEffect_BurnHurt : stateEffectBase
{
    public StateEffect_BurnHurt(int _num = 0)
    {
        num = _num;
        effectDele = new DeleCardEffect(AllAsset.effectAsset.PlayerGetRealHurt);
        eventkind = EventKind.Event_PlayerBurnDamage;
    }
    public override string DescribeEffect()
    {
        return "对玩家造成灼烧伤害" + num;
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



