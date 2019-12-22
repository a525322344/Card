using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ReactionListController
{
    public static List<Reaction> GetReactionByEventkind(EventKind eventKind)
    {
        switch (eventKind)
        {
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
            case EventKind.Event_PlayerGetHurt:
                return reactListToPlayerGetHurt;
            case EventKind.Event_EnemyGetArmor:
                return reactListToEnemyGetArmor;
            case EventKind.Event_Action:
                return reactListToAction;
            case EventKind.Event_Repeat:
                return reactListToRepeat;
            case EventKind.Event_SystmeRepeat:
                return reactListToSystemRepeat;
            case EventKind.Event_EnemyGetBurn:
                return reactListToEnemyGetBurn;
            case EventKind.Event_EnemyBurnDamage:
                return reactListToEnemyBurnDamage;
            case EventKind.Event_LinkRandom:
                return reactListToLinkRandom;
            //更新
            default:
                Debug.Log("错误，没有写这个效果的额外效果表");
                return null;
        }
    }
    public static void recesiveReactonToSetIn(Reaction reaction)
    {
        GetReactionByEventkind(reaction.getReactionKind()).Add(reaction);
    }

    private static List<Reaction> reactListToDamage = new List<Reaction>();
    private static List<Reaction> reactListToArmor = new List<Reaction>();
    private static List<Reaction> reactListToPlaycard = new List<Reaction>();
    private static List<Reaction> reactListToDiscard = new List<Reaction>();
    private static List<Reaction> reactListToDrawCard = new List<Reaction>();
    private static List<Reaction> reactListToDrawACard = new List<Reaction>();
    private static List<Reaction> reactListToRoundStartDrawCard = new List<Reaction>();
    private static List<Reaction> reactListToPlayerGetHurt = new List<Reaction>();
    private static List<Reaction> reactListToEnemyGetArmor = new List<Reaction>();
    private static List<Reaction> reactListToAction = new List<Reaction>();
    private static List<Reaction> reactListToRepeat = new List<Reaction>();
    private static List<Reaction> reactListToSystemRepeat = new List<Reaction>();
    private static List<Reaction> reactListToEnemyGetBurn = new List<Reaction>();
    private static List<Reaction> reactListToEnemyBurnDamage = new List<Reaction>();
    private static List<Reaction> reactListToRoundEndDisCard = new List<Reaction>();
    private static List<Reaction> reactListToLinkRandom = new List<Reaction>();
    //更新
}



//反应器
public abstract class Reaction
{
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

    //反应一次就移除
    protected bool m_Active = true;
    public bool b_haveEvent = false;
}

//对反应的效果事件造成影响，强化或削弱效果
public class Reaction_Affect : Reaction
{
    public Reaction_Affect(string _name,extraEffectBase extraEffect, EventKind _kind)
    {
        name = _name;
        kind = _kind;
        affectEffect = extraEffect;
        b_haveEvent = false;
    }

    public override extraEffectBase getExtreEffect()
    {
        //Debug.Log("reaction"+name);
        return affectEffect;
    }

    private extraEffectBase affectEffect;
}

//对反应的效果事件，产生新的事件
public class Reaction_Create : Reaction
{
    public Reaction_Create(singleEvent createEvent, EventKind _kind)
    {
        kind = _kind;
        toCreateEvent = createEvent;
        b_haveEvent = true;
    }
    public override void dealReaction()
    {
        gameManager.Instance.battlemanager.eventManager.AddEvent(toCreateEvent);
    }
    private singleEvent toCreateEvent;
}

