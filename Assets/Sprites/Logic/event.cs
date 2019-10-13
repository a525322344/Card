﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//打出卡牌受到各种加成，buff影响，怪物能力调整的事件——的基类
public abstract class singleEvent
{
    public virtual void prepareEvent() { }
    public virtual void insertEvent() { }
    public virtual void dealEvent(battleInfo battleInfo) { }

    public abstract void dealEffect(battleInfo battleInfo);

    public abstract void recesiveNotice();
    public bool b_logoutAfterDeal = true;
    //事件类型，用于枚举
    protected EventKind m_eventKind;
    public bool b_haveChildEvent;
    public List<singleEvent> childEvents = new List<singleEvent>();
    public List<Reaction> EventReactionList = new List<Reaction>();
}

//系统事件，比如回合开始抽卡；回合结束弃卡；回合开始护甲归零
public class SystemEvent : singleEvent
{
    public SystemEvent(EffectBase effect)
    {
        m_eventKind = effect.GetEventKind();
        m_effect = effect;
        b_logoutAfterDeal = false;
    }
    //设置影响效果，执行反应事件
    public override void recesiveNotice()
    {
        List<Reaction> reactionlist = ReactionListController.GetReactionByEventkind(m_eventKind);
        foreach (Reaction reaction in reactionlist)
        {
            if (reaction.Active == true)
            {
                extraEffectBase indexextra = reaction.getExtreEffect();
                reaction.dealReaction();
                if (indexextra != null)
                {
                    m_extraEffectList.Add(indexextra);
                }
            }
        }
    }
    public override void dealEffect(battleInfo battleInfo)
    {
        //先接受信息，同时会先触发父事件的反应事件，将其添加在后面
        recesiveNotice();
        //重复几次
        int index = m_effect.getNum();
        foreach (extraEffectBase extraEffect in m_extraEffectList)
        {
            index = extraEffect.AdjustEffect(index);
        }
        m_effect.DealEffect(index, battleInfo);
        Debug.Log("系统；" + m_effect.DescribeEffect());
        //生成子事件表
        childEvents.Clear();
        if (m_effect.b_hasChildEffect)
        {
            b_haveChildEvent = true;
            for (int i = 0; i < index; i++)
            {
                foreach (EffectBase childeffect in m_effect.childeffects)
                {
                    childEvents.Add(new EffectEvent(childeffect, this));
                }
            }
            //根据子事件表，创建添加EventShow
            for(int i=childEvents.Count-1;i>=0;i--)
            {
                gameManager.Instance.battlemanager.eventManager.InsertEvent(childEvents[i]);
            }
        }
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
    public override void prepareEvent()
    {
        childEvents.Clear();

        recesiveNotice();
        int index = m_effect.getNum();
        foreach (extraEffectBase extraEffect in m_extraEffectList)
        {
            index = extraEffect.AdjustEffect(index);
        }
        m_effect.mixnum = index;
        Debug.Log("effect prepare:" + m_effect.DescribeEffect());

        if (b_haveChildEvent)
        {
            for (int i = 0; i < index; i++)
            {
                foreach (cardEffectBase effect in m_effect.childeffects)
                {
                    childEvents.Add(new EffectEvent(effect, this));
                }
            }
            foreach (EffectEvent _event in childEvents)
            {
                _event.prepareEvent();
            }
        }
    }
    public override void insertEvent()
    {
        Debug.Log("effect insert" + m_effect.DescribeEffect());
        foreach (Reaction react in EventReactionList)
        {
            react.dealReaction();
        }
        foreach (EffectEvent effectEvent in childEvents)
        {
            gameManager.Instance.battlemanager.eventManager.AddEvent(effectEvent);
            effectEvent.prepareEvent();
            effectEvent.insertEvent();
        }
    }
    public override void dealEvent(battleInfo battleInfo)
    {
        m_effect.DealEffect(m_effect.mixnum, battleInfo);
        Debug.Log("效果：" + m_effect.DescribeEffect());
    }
    //执行效果
    public override void dealEffect(battleInfo battleInfo)
    {
        //recesiveNotice();
        //int index = m_effect.getNum();
        //foreach(extraEffectBase extraEffect in m_extraEffectList)
        //{
        //    index = extraEffect.AdjustEffect(index);
        //}
        //m_effect.DealEffect(index, battleInfo);
        
        ////如果有子效果,则说明该effect为repeat类

        //if (b_haveChildEvent)
        //{
        //    childEvents.Clear();
        //    for (int i = 0; i < index; i++)
        //    {
        //        foreach (cardEffectBase effect in m_effect.childeffects)
        //        {
        //            childEvents.Add(new EffectEvent(effect, this));
        //        }
        //    }
        //    //根据子事件表，创建添加EventShow
        //    for (int i = childEvents.Count - 1; i >= 0; i--)
        //    {
        //        gameManager.Instance.battlemanager.eventManager.InsertEvent(childEvents[i]);
        //    }
        //}
    }
    //设置影响效果，执行反应事件
    public override void recesiveNotice()
    {
        m_extraEffectList.Clear();
        EventReactionList.Clear();
        List<Reaction> reactionlist = ReactionListController.GetReactionByEventkind(m_eventKind);
        foreach(Reaction reaction in reactionlist)
        {
            if (reaction.Active == true)
            {
                extraEffectBase indexextra = reaction.getExtreEffect();
                
                if (indexextra != null)
                {
                    m_extraEffectList.Add(indexextra);
                }
                if (reaction.b_haveEvent)
                {
                    EventReactionList.Add(reaction);
                }
            }  
        }
    }
    public void updateDescribe()
    {
        recesiveNotice();
        int index = m_effect.getNum();
        foreach (extraEffectBase extraEffect in m_extraEffectList)
        {
            index = extraEffect.AdjustEffect(index);
        }
        m_effect.mixnum = index;

        if (b_haveChildEvent)
        {
            childEvents.Clear();
            for (int i = 0; i < index; i++)
            {
                foreach (cardEffectBase effect in m_effect.childeffects)
                {
                    childEvents.Add(new EffectEvent(effect, this));
                }
            }
            //根据子事件表，创建添加EventShow
            for (int i = childEvents.Count - 1; i >= 0; i--)
            {
                EffectEvent e = (EffectEvent)childEvents[i];
                e.updateDescribe();
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

        b_logoutAfterDeal = true;
        b_haveChildEvent = true;
    }
    //预处理事件，当选择一张卡而不打出时，应显示实时数值，不改变reaction之类的效果
    public override void prepareEvent()
    {
        //激活使用的部件
        m_magicPart.activatePart();
        childEvents.Clear();
        //询问卡牌类的效果表，创建效果事件表
        foreach (cardEffectBase effect in playercard.getEffectList())
        {
            //创建效果事件
            singleEvent effectevent = new EffectEvent(effect, this);
            //把这个效果添加到该卡牌事件的事件子表中
            childEvents.Add(effectevent);
        }
        recesiveNotice();
        Debug.Log("card prepare");
        foreach (EffectEvent _event in childEvents)
        {
            _event.prepareEvent();
        }
        //休眠之前的部件
        m_magicPart.sleepPart();

    }
    //发动效果
    public override void dealEffect(battleInfo battleInfo)
    {
    }
    public override void insertEvent()
    {
        Debug.Log("card insert");
        foreach(Reaction react in EventReactionList)
        {
            react.dealReaction();
        }
        foreach(EffectEvent effectEvent in childEvents)
        {
            gameManager.Instance.battlemanager.eventManager.AddEvent(effectEvent);
            effectEvent.insertEvent();
        }
        m_magicPart.sleepPart();
    }
    public override void dealEvent(battleInfo battleInfo)
    {
        Debug.Log("卡牌:" + playercard.Name);
    }
    //接受反应表
    public override void recesiveNotice()
    {
        EventReactionList.Clear();
        List<Reaction> reactionlist = ReactionListController.GetReactionByEventkind(m_eventKind);
        foreach (Reaction reaction in reactionlist)
        {
            //是否激活，只有休眠的魔法部件才是false
            if (reaction.Active == true)
            {
                //目前没有对卡牌事件产生影响的反应
                //只储存会触发事件的反应器
                if (reaction.b_haveEvent)
                {
                    EventReactionList.Add(reaction);
                }
            }                    
        }
    }

    public string EventCardDescribe()
    {
        return playercard.CardDescribe();
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
        b_logoutAfterDeal = true;
    }

    public override void recesiveNotice()
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
        //根据子事件表，创建添加EventShow
        for (int i = childEvents.Count - 1; i >= 0; i--)
        {
            gameManager.Instance.battlemanager.eventManager.InsertEvent(childEvents[i]);
        }
    }
}
//状态事件
public class StateEvent : singleEvent
{
    public StateEvent(stateAbstarct state,EffectBase effect)
    {
        m_effect = effect;
        m_state = state;
        m_eventKind = effect.GetEventKind();
    }
    public override void dealEffect(battleInfo battleInfo)
    {
        recesiveNotice();
        int index = m_state.num;
        m_effect.SetNum(index);
        foreach (extraEffectBase extraEffect in m_extraEffectList)
        {
            index = extraEffect.AdjustEffect(index);
        }
        Debug.Log("状态：" + m_effect.DescribeEffect());
        m_effect.DealEffect(index, battleInfo);
        m_state.DealState();
    }
    public override void recesiveNotice()
    {
        List<Reaction> reactionlist = ReactionListController.GetReactionByEventkind(m_eventKind);
        foreach (Reaction reaction in reactionlist)
        {
            if (reaction.Active == true)
            {
                extraEffectBase indexextra = reaction.getExtreEffect();
                reaction.dealReaction();
                if (indexextra != null)
                {
                    m_extraEffectList.Add(indexextra);
                }
            }
        }
    }

    public EffectBase m_effect;
    public stateAbstarct m_state;
    public List<extraEffectBase> m_extraEffectList = new List<extraEffectBase>();
}