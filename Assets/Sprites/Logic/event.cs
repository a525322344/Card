﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//打出卡牌受到各种加成，buff影响，怪物能力调整的事件——的基类
public abstract class singleEvent
{
    public abstract void dealEffect(battleInfo battleInfo);
    protected abstract void recesiveNotice();
    public bool b_logoutAfterDeal = true;
    //事件类型，用于枚举
    protected EventKind m_eventKind;
    public bool b_haveChildEvent;
    public List<singleEvent> childEvents = new List<singleEvent>();
}

//系统事件，比如回合开始抽卡；回合结束弃卡；回合开始护甲归零
public class SystemEvent : singleEvent
{
    public SystemEvent(EffectBase effect)
    {
        m_eventKind = effect.GetEventKind();
        m_effect = effect;
        b_logoutAfterDeal = false;
        b_haveChildEvent = false;
    }
    //设置影响效果，执行反应事件
    protected override void recesiveNotice()
    {
        List<Reaction> reactionlist = ReactionListController.GetReactionByEventkind(m_eventKind);
        foreach (Reaction reaction in reactionlist)
        {
            if (reaction.Active == true)
            {
                extraEffectBase indexextra = reaction.dealReaction();
                if (indexextra != null)
                {
                    m_extraEffectList.Add(indexextra);
                }
            }
        }
    }
    public override void dealEffect(battleInfo battleInfo)
    {
        recesiveNotice();
        int index = m_effect.getNum();
        foreach (extraEffectBase extraEffect in m_extraEffectList)
        {
            index = extraEffect.AdjustEffect(index);
            Debug.Log(index);
        }
        m_effect.DealEffect(index, battleInfo);
    }
    //效果类
    private EffectBase m_effect;
    //强化效果表
    private List<extraEffectBase> m_extraEffectList = new List<extraEffectBase>();
    //场景战场信息
    private battleInfo m_battleInfo;
}

//独立的效果事件，卡牌的效果子事件
public class EffectEvent : singleEvent
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="effect">效果</param>
    /// <param name="extralist">反应表</param>
    /// <param name="fatherevent">事件父类</param>
    public EffectEvent(EffectBase effect,singleEvent fatherevent)
    {
        m_effect = effect;
        m_fatherEvent = fatherevent;
        m_eventKind = effect.GetEventKind();
        if (effect.b_hasChildEffect)
        {
            b_haveChildEvent = true;
        }
        else
        {
            b_haveChildEvent = false;
        }
    }
    //执行效果
    public override void dealEffect(battleInfo battleInfo)
    {
        recesiveNotice();
        int index = m_effect.getNum();
        foreach(extraEffectBase extraEffect in m_extraEffectList)
        {
            index = extraEffect.AdjustEffect(index);
        }
        m_effect.DealEffect(index, battleInfo);
        Debug.Log(m_effect.DescribeEffect());
        //如果有子效果,则说明该effect为repeat类

        if (b_haveChildEvent)
        {
            Debug.Log("repeat:" + index);
            for (int i = 0; i < index; i++)
            {
                foreach (cardEffectBase effect in m_effect.childeffects)
                {
                    childEvents.Add(new EffectEvent(effect, this));
                }
            }
            foreach (EffectEvent childevent in childEvents)
            {
                childevent.dealEffect(battleInfo);
            }
        }
    }
    //设置影响效果，执行反应事件
    protected override void recesiveNotice()
    {
        List<Reaction> reactionlist = ReactionListController.GetReactionByEventkind(m_eventKind);
        foreach(Reaction reaction in reactionlist)
        {
            if (reaction.Active == true)
            {
                extraEffectBase indexextra = reaction.dealReaction();
                if (indexextra != null)
                {
                    m_extraEffectList.Add(indexextra);
                }
            }  
        }
    }


    //效果类
    public EffectBase m_effect;
    //强化效果表
    private List<extraEffectBase> m_extraEffectList = new List<extraEffectBase>();
    //父类事件
    private singleEvent m_fatherEvent;
    //场景战场信息
    private battleInfo battleInfo;
}

//卡牌事件
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
        m_eventKind = cardkind.GetEventKind();
        m_magicPart = magicPart;
        m_magicPart.activatePart();
        //询问卡牌类的效果表，创建效果事件表
        foreach (cardEffectBase effect in playercard.getEffectList())
        {
            //创建效果事件
            singleEvent effectevent = new EffectEvent(effect, this);
            ////为这个效果事件接受相应的反应表
            //effectevent.recesiveNotice();
            //把这个效果添加到该卡牌事件的事件子表中
            childEvents.Add(effectevent);
        }
        b_logoutAfterDeal = true;
        b_haveChildEvent = true;
    }
    //发动效果
    public override void dealEffect(battleInfo battleInfo)
    {
        recesiveNotice();
        //对于卡牌的每个效果事件，触发其效果
        foreach (singleEvent effectevent in childEvents)
        {
            effectevent.dealEffect(battleInfo);
        }
        //临时的
        //  处理卡牌事件，使使用的部件休眠
        m_magicPart.sleepPart();
    }
    //接受反应表
    protected override void recesiveNotice()
    {
        List<Reaction> reactionlist = ReactionListController.GetReactionByEventkind(m_eventKind);
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
}

//行动事件
public class ActionEvent:singleEvent
{   
    public ActionEvent(actionAbstract actionAb)
    {
        foreach (EffectBase effect in actionAb.effects)
        {
            childEvents.Add(new EffectEvent(effect, this));
        }            
        m_eventKind = EventKind.Event_Action;
        b_haveChildEvent = true;
    }

    protected override void recesiveNotice()
    {
        List<Reaction> reactionlist = ReactionListController.GetReactionByEventkind(m_eventKind);
        foreach (Reaction reaction in reactionlist)
        {
            if (reaction.Active == true)
            {
                reaction.dealReaction();
            }
        }
    }
    public override void dealEffect(battleInfo battleInfo)
    {
        recesiveNotice();
        foreach(EffectEvent effectevent in childEvents)
        {
            effectevent.dealEffect(battleInfo);
        }
    }
}