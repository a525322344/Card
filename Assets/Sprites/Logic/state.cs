using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class stateAbstarct
{
    public string name;
    public int num;
    public EventKind eventkind;
    public virtual void SetInState() { }
    public virtual void SetOutState() { }
    public virtual void DealState() { }
}
[System.Serializable]
public class StateBurn : stateAbstarct
{
    public StateBurn(int _num)
    {
        name = "灼烧";
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