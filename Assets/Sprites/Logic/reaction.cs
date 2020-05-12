using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReactionListController
{
    public List<Reaction> GetReactionByEventkind(EventKind eventKind)
    {
        switch (eventKind)
        {
            case EventKind.Event_NULL:
                //返回空表
                return new List<Reaction>();
            case EventKind.Event_Damage:
                return reactListToDamage;
            case EventKind.Event_Armor:
                return reactListToArmor;
            case EventKind.Event_PlayCard:
                return reactListToPlaycard;
            case EventKind.Event_Discard:
                return reactListToDiscard;
            case EventKind.Event_DrawCard:
                return reactListToDrawCard;
            case EventKind.Event_DrawACard:
                return reactListToDrawACard;
            case EventKind.Event_RoundStartDrawCard:
                return reactListToRoundStartDrawCard;
            case EventKind.Event_RoundEndDisCard:
                return reactListToRoundEndDisCard;
            case EventKind.Event_MonsterHurt:
                return reactListToPlayerGetHurt;
            case EventKind.Event_MonsterArmor:
                return reactListToEnemyGetArmor;
            case EventKind.Event_MonsterAction:
                return reactListToAction;
            case EventKind.Event_Repeat:
                return reactListToRepeat;
            case EventKind.Event_SystmeRepeat:
                return reactListToSystemRepeat;
            case EventKind.Event_EnemyGetBurn:
                return reactListToEnemyGetBurn;
            case EventKind.Event_EnemyBurnMultiply:
                return reactListToEnemyBurnMultiply;
            case EventKind.Event_EnemyBurnDamage:
                return reactListToEnemyBurnDamage;
            case EventKind.Event_LinkRandom:
                return reactListToLinkRandom;
            case EventKind.Event_Whether:
                return reactListToWhether;
            case EventKind.Event_DisOneCard:
                return reactListToDisOneCard;
            case EventKind.Event_DisSomeCard:
                return reactListToDisSomeCard;
            case EventKind.Event_Fill:
                return reactListToFill;
            case EventKind.Event_Exhaust:
                return reactListToExhaust;
            case EventKind.Event_MonsterBurn:
                return reactListMonsterBurn;
            case EventKind.Event_PlayerBurnDamage:
                return reactListPlayerBurnDamage;
            //更新
            default:
                Debug.Log("错误，没有写这个效果的额外效果表");
                return null;
        }
    }
    public void recesiveReactonToSetIn(Reaction reaction)
    {
        GetReactionByEventkind(reaction.getReactionKind()).Add(reaction);
    }

    private List<Reaction> reactListToDamage = new List<Reaction>();
    private List<Reaction> reactListToArmor = new List<Reaction>();
    private List<Reaction> reactListToPlaycard = new List<Reaction>();
    private List<Reaction> reactListToDiscard = new List<Reaction>();
    private List<Reaction> reactListToDrawCard = new List<Reaction>();
    private List<Reaction> reactListToDrawACard = new List<Reaction>();
    private List<Reaction> reactListToRoundStartDrawCard = new List<Reaction>();
    private List<Reaction> reactListToPlayerGetHurt = new List<Reaction>();
    private List<Reaction> reactListToEnemyGetArmor = new List<Reaction>();
    private List<Reaction> reactListToAction = new List<Reaction>();
    private List<Reaction> reactListToRepeat = new List<Reaction>();
    private List<Reaction> reactListToSystemRepeat = new List<Reaction>();
    private List<Reaction> reactListToEnemyGetBurn = new List<Reaction>();
    private List<Reaction> reactListToEnemyBurnMultiply = new List<Reaction>();
    private List<Reaction> reactListToEnemyBurnDamage = new List<Reaction>();
    private List<Reaction> reactListToRoundEndDisCard = new List<Reaction>();
    private List<Reaction> reactListToLinkRandom = new List<Reaction>();
    private List<Reaction> reactListToWhether = new List<Reaction>();
    private List<Reaction> reactListToDisOneCard = new List<Reaction>();
    private List<Reaction> reactListToDisSomeCard = new List<Reaction>();
    private List<Reaction> reactListToFill = new List<Reaction>();
    private List<Reaction> reactListToExhaust = new List<Reaction>();
    private List<Reaction> reactListMonsterBurn = new List<Reaction>();
    private List<Reaction> reactListPlayerBurnDamage = new List<Reaction>();

    //更新
}



//反应器
public abstract class Reaction
{
    public Part fatherpart;
    public string name;
    public bool Active {
        set { m_Active = value; }
        get { return m_Active; }
    }
    //反应种类
    protected EventKind kind;
    public EventKind getReactionKind()
    {
        return kind;
    }
    public virtual void dealReaction() { }
    public virtual extraEffectBase getExtreEffect() { return null; }

    public virtual string ReactionDescribe()
    {
        return "";
    }

    protected bool m_Active = false;
    public bool b_haveEvent = false;
    protected string judgeDescribe(EventKind eventKind)
    {
        switch (eventKind)
        {
            case EventKind.Event_PlayCard:
                return "每打出一张牌";
        }
        return "没写";
    }
}

//对反应的效果事件造成影响，强化或削弱效果
public class Reaction_Affect : Reaction
{
    public Reaction_Affect(string _name,extraEffectBase extraEffect, EventKind _kind,Part _fatherpart)
    {
        fatherpart = _fatherpart;
        name = _name;
        kind = _kind;
        affectEffect = extraEffect;
        b_haveEvent = false;
    }
    public Reaction_Affect(Reaction reaction, Part _fatherpart)
    {
        name = reaction.name;
        fatherpart = _fatherpart;
        kind = reaction.getReactionKind();
        affectEffect = reaction.getExtreEffect();
        b_haveEvent = reaction.b_haveEvent;
        Debug.Log("reaction_affect");
    }
    public override extraEffectBase getExtreEffect()
    {
        return affectEffect;
    }
    public override string ReactionDescribe()
    {
        return affectEffect.Describe();
    }

    private extraEffectBase affectEffect;
}

//对反应的效果事件，产生新的事件
public class Reaction_Create : Reaction
{
    public Reaction_Create(string name_, singleEvent createEvent, EventKind _kind,Part _fatherpart)
    {
        fatherpart = _fatherpart;
        name = name_;
        kind = _kind;
        toCreateEvent = createEvent;
        b_haveEvent = true;
    }
    public Reaction_Create(Reaction reaction, Part _fatherpart)
    {
        name = reaction.name;
        fatherpart = _fatherpart;
        kind = reaction.getReactionKind();
        toCreateEvent = (reaction as Reaction_Create).toCreateEvent;
        toCreateEvent.m_effect.magicpart = fatherpart as MagicPart;
        b_haveEvent = reaction.b_haveEvent;
        Debug.Log("reaction_create");
    }
    public override void dealReaction()
    {
        gameManager.Instance.battlemanager.eventManager.InsertEvent(toCreateEvent);
    }
    public override string ReactionDescribe()
    {
        return judgeDescribe(kind) + "," + toCreateEvent.eventDescribe;
    }
    public singleEvent toCreateEvent;
}


public class biaoji
{
    public Reaction reaction;
    public void setReaction()
    {
        reaction.Active = false;
    }
    public void ActiveBiaoji()
    {
        reaction.Active = true;
    }
    public void SleepBiaoji()
    {
        reaction.Active = false;
    }
    public void deleteReaction()
    {
        reaction.Active = false;
    }
}