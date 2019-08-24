using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ReactionKind
{
    Reaction_Affect_DamageUp,
    Reaction_Affect_ArmorUp,
    Reaction_Create_PlayCard,
    Reaction_Create_Discard,
}

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
                break;
        }
        Debug.Log("错误，没有写这个效果的额外效果表");
        return null;
    }
    public static void recesiveReactonToSetIn(Reaction reaction)
    {
        switch (reaction.getReactionKind())
        {
            case ReactionKind.Reaction_Affect_DamageUp:
                reactListToDamage.Add(reaction);
                break;
            case ReactionKind.Reaction_Affect_ArmorUp:
                reactListToArmor.Add(reaction);
                break;
            case ReactionKind.Reaction_Create_PlayCard:
                reactListToPlaycard.Add(reaction);
                break;
            case ReactionKind.Reaction_Create_Discard:
                break;
        }
    }

    private static List<Reaction> reactListToDamage = new List<Reaction>();
    private static List<Reaction> reactListToArmor = new List<Reaction>();
    private static List<Reaction> reactListToPlaycard = new List<Reaction>();
    private static List<Reaction> reactListToDiscard = new List<Reaction>();
    
}



//反应器
public abstract class Reaction
{
    public bool Active {
        set { m_Active = value; }
        get { return m_Active; }
    }
    //反应种类
    protected ReactionKind kind;
    public ReactionKind getReactionKind()
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
    public Reaction_Affect(extraEffectBase extraEffect,ReactionKind _kind)
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
    public Reaction_Create(singleEvent createEvent,ReactionKind _kind)
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