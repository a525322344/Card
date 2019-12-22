using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class stateAbstarct
{
    public string name;
    public string key;
    public int num;
    public virtual void SetInState() { }
    public virtual void SetOutState() { }
    public virtual void DealState() { }
}
//反应类状态，包含reaction
public class stateWithReaction:stateAbstarct
{
    public EventKind eventkind;
}
public class stateWithEvent : stateAbstarct
{
    public StateEvent m_event;
}

public class StateExitLinkPart : stateWithEvent
{
    public StateExitLinkPart()
    {
        name = "退出链接状态";
        key = "ExitLinkPart";
        num = 0;
        m_event = new StateEvent(this, new StateEffect_ExitLinkPart());
    }
    public override void SetInState()
    {
        gameManager.Instance.battlemanager.eventManager.EndEventShows.Add(new EventShow(m_event,
            gameManager.Instance.battlemanager.eventManager.EndEventShows));
    }
    public override void DealState()
    {
        SetOutState();
    }
    public override void SetOutState()
    {
        gameManager.Instance.battlemanager.battleInfo.Player.nameStatePairs.Remove(key);
        gameManager.Instance.battlemanager.battleInfo.Player.stateList.Remove(this);
    }
}

[System.Serializable]
public class StateBurn : stateWithReaction
{
    public StateBurn(int _num)
    {
        name = "灼烧";
        key = "Burn";
        num = _num;
        m_reactionEvent = new StateEvent(this,new effectBurnDamage(0));
        eventkind = EventKind.Event_Damage;
        m_reaction = new Reaction_Create(m_reactionEvent, eventkind);
    }
    public override void SetInState()
    {
        ReactionListController.recesiveReactonToSetIn(m_reaction);
    }
    public override void SetOutState()
    {
        ReactionListController.GetReactionByEventkind(eventkind).Remove(m_reaction);
        gameManager.Instance.battlemanager.battleInfo.Enemy.nameStatePairs.Remove("Burn");
        gameManager.Instance.battlemanager.battleInfo.Enemy.stateList.Remove(this);
    }
    public override void DealState()
    {
        num--;
        if (num == 0)
        {
            SetOutState();
        }
        //gameManager.Instance.battlemanager.showcontroll.ShowFire(num);
    }
    private Reaction m_reaction;
    private singleEvent m_reactionEvent;
}