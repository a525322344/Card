using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventKind
{
    Event_Damage,
    Event_Armor,
    Event_PlayCard,
    Event_Discard,
}

//打出卡牌受到各种加成，buff影响，怪物能力调整的事件——的基类
public abstract class singleEvent
{
    public abstract void dealEffect(battleInfo battleInfo);
    public abstract void recesiveNotice();

    //事件类型，用于枚举
    protected EventKind eventKind;
}

public abstract class triggerEvent : singleEvent
{

}


/// <summary>
/// 独立的效果事件
/// </summary>
public class EffectEvent : singleEvent
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="effect">效果</param>
    /// <param name="extralist">反应表</param>
    /// <param name="fatherevent">事件父类</param>
    public EffectEvent(cardEffectBase effect,singleEvent fatherevent)
    {
        Effect = effect;
        fatherEvent = fatherevent;
        eventKind = effect.GetEventKind();
    }
    //执行效果
    public override void dealEffect(battleInfo battleInfo)
    {
        int index = Effect.getNum();
        foreach(extraEffectBase extraEffect in extraEffectList)
        {
            Debug.Log("0.0");
            index = extraEffect.AdjustEffect(index);
            Debug.Log(index);
        }
        Effect.DealEffect(index, battleInfo);
    }
    //设置影响效果，执行反应事件
    public override void recesiveNotice()
    {
        List<Reaction> reactionlist = ReactionListController.GetReactionByEventkind(eventKind);
        foreach(Reaction reaction in reactionlist)
        {
            if (reaction.Active == true)
            {
                extraEffectBase indexextra = reaction.dealReaction();
                if (indexextra != null)
                {
                    extraEffectList.Add(indexextra);
                }
            }  
        }
    }


    //效果类
    private cardEffectBase Effect;
    //强化效果表
    private List<extraEffectBase> extraEffectList = new List<extraEffectBase>();
    //父类事件
    private singleEvent fatherEvent;
    //场景战场信息
    private battleInfo battleInfo;
}


/// <summary>
/// 卡牌事件
/// </summary>
public class CardEvent : singleEvent
{
    /// <summary>
    /// 卡牌事件构造函数
    /// </summary>
    /// <param name="_playerCard">打出的卡牌</param>
    /// <param name="_parttrigger">发出部件</param>
    public CardEvent(playerCard _playerCard,MagicPart magicPart,cardEffectBase cardkind)
    {
        //记录卡牌类
        playercard = _playerCard;
        eventKind = cardkind.GetEventKind();
        m_magicPart = magicPart;
        m_magicPart.activatePart();
        //询问卡牌类的效果表创建效果时间表
        foreach (cardEffectBase effect in playercard.getEffectList())
        {
            //创建效果事件
            singleEvent effectevent = new EffectEvent(effect, this);
            //为这个效果事件接受相应的反应表
            effectevent.recesiveNotice();
            //把这个效果添加到该卡牌事件的事件子表中
            EffectChildEvents.Add(effectevent);
        }  
    }
    //发动效果
    public override void dealEffect(battleInfo battleInfo)
    {
        //对于卡牌的每个效果事件，触发其效果
        foreach(singleEvent effectevent in EffectChildEvents)
        {
            effectevent.dealEffect(battleInfo);
        }
        //临时的
        //  处理卡牌事件，使使用的部件休眠
        m_magicPart.sleepPart();
    }
    //接受反应表
    public override void recesiveNotice()
    {
        List<Reaction> reactionlist = ReactionListController.GetReactionByEventkind(eventKind);
        foreach (Reaction reaction in reactionlist)
        {
            if (reaction.Active == true)
            {
                //目前没有对卡牌事件产生影响的反应
                //只进行反应器被触发的效果
                reaction.dealReaction();
            }                    
        }
        //List<Reaction> magicReactions = m_magicPart.getMagicReactionList();

    }

    //卡牌
    private MagicPart m_magicPart;
    private playerCard playercard;
    private List<singleEvent> EffectChildEvents = new List<singleEvent>();
}