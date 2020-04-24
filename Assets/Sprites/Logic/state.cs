using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class stateAbstarct
{
    public string name;
    public string key;
    public int num;
    public int texint;
    public virtual void SetInState()
    {
        gameManager.Instance.battlemanager.realenemy.StateUpdtae();
        gameManager.Instance.battlemanager.realplayer.StateUpdtae();
    }
    public virtual void SetOutState()
    {
        gameManager.Instance.battlemanager.realenemy.StateUpdtae();
        gameManager.Instance.battlemanager.realplayer.StateUpdtae();
    }
    public virtual void DealState()
    {
        gameManager.Instance.battlemanager.realenemy.StateUpdtae();
        gameManager.Instance.battlemanager.realplayer.StateUpdtae();
    }
    protected string ColorGold = "<color=#CFB53B>";
    protected string ColorBlue = "<color=#007FFF>";
    protected string ColorGreen = "<color=#32CD32>";
    protected string ColorEnd = "</color>";
    public virtual string DescribeState() { return ""; }
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
        name = "退出链接";
        key = "ExitLinkPart";
        num = -999;
        texint = 1;
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
        base.SetOutState();
    }
    public override string DescribeState()
    {
        string result = "";
        result += ColorGold + "退出连接" + ColorEnd;
        result += "\n";
        result += "回合结束时，取消所有连接状态";
        return result;
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
        texint = 0;
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
        base.SetOutState();
    }
    public override string DescribeState()
    {
        string result = "";
        result += ColorGold + "灼烧" + ColorEnd;
        result += "\n";
        result += "在其受到伤害时，生命值减";
        result += ColorBlue + num + ColorEnd;
        result += "，然后灼烧层数减少";
        result += ColorBlue + "1" + ColorEnd;
        return result;
    }
    public override void DealState()
    {
        num--;
        if (num == 0)
        {
            SetOutState();
        }
        base.DealState();
    }
    private Reaction m_reaction;
    private singleEvent m_reactionEvent;
}