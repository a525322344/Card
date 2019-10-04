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
            case EventKind.Event_PlayerGetHurt:
                return reactListToPlayerGetHurt;
            case EventKind.Event_EnemyGetArmor:
                return reactListToEnemyGetArmor;
            case EventKind.Event_Action:
                return reactListToAction;
            //更新
            default:
                Debug.Log("错误，没有写这个效果的额外效果表");
                return null;
        }
    }
    public static void recesiveReactonToSetIn(Reaction reaction)
    {
        switch (reaction.getReactionKind())
        {
            case EventKind.Event_Damage:
                reactListToDamage.Add(reaction);
                break;
            case EventKind.Event_Armor:
                reactListToArmor.Add(reaction);
                break;
            case EventKind.Event_PlayCard:
                reactListToPlaycard.Add(reaction);
                break;
            case EventKind.Event_DrawCard:
                reactListToDrawCard.Add(reaction);
                break;
            case EventKind.Event_DrawACard:
                reactListToDrawACard.Add(reaction);
                break;
            case EventKind.Event_RoundStartDrawCard:
                reactListToRoundStartDrawCard.Add(reaction);
                break;
            case EventKind.Event_PlayerGetHurt:
                reactListToPlayerGetHurt.Add(reaction);
                break;
            case EventKind.Event_EnemyGetArmor:
                reactListToEnemyGetArmor.Add(reaction);
                break;
            case EventKind.Event_Action:
                reactListToAction.Add(reaction);
                break;
                //需要更新
        }
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
    //更新
}



//反应器
public abstract class Reaction
{
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
    public abstract extraEffectBase dealReaction();

    //反应一次就移除
    protected bool m_Active = true;
}

//对反应的效果事件造成影响，强化或削弱效果
public class Reaction_Affect : Reaction
{
    public Reaction_Affect(extraEffectBase extraEffect, EventKind _kind)
    {
        kind = _kind;
        affectEffect = extraEffect;
    }

    public override extraEffectBase dealReaction()
    {
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
    }
    public override extraEffectBase dealReaction()
    {
        //创建事件toCreateEvent,并添加到事件队列中
        //……

        return null;
    }
    private singleEvent toCreateEvent;
}