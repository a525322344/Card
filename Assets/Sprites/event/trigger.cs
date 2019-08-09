using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//事件触发器，响应事件的后，可以影响事件，产生事件
public abstract class Trigger
{
    public abstract bool canInffulenceEvent(singleEvent singleEvent);
    public virtual void dealTriggerEffect() { }
    public virtual int adjustTriggerEffect(int _num) { return _num; }
    public virtual bool Is_extraInfEffect(int n, cardEffectBase cardEffect) { return false; }
    public List<extraEffectBase> GetExtraList()
    {
        return extraEffectList;
    }

    protected List<extraEffectBase> extraEffectList = new List<extraEffectBase>();
    protected List<cardEffectBase> partEffectList = new List<cardEffectBase>();
}

public class PartTrigger : Trigger
{
    public PartTrigger()
    {

    }
    public override bool canInffulenceEvent(singleEvent _event)
    {
        bool can = false;
        foreach (extraEffectBase effectBase in extraEffectList)
        {
            foreach (cardEffectBase cardEffect in _event.getEffectList())
            {
                if (effectBase.canInfluence(cardEffect))
                {
                    can = true;
                    break;
                }
            }
        }
        return can;
    }
    public override bool Is_extraInfEffect(int n, cardEffectBase cardEffect)
    {
        return extraEffectList[n].canInfluence(cardEffect);
    }

    public void addExtraEffect(extraEffectBase extra)
    {
        extraEffectList.Add(extra);
    }
    public void addCardEffect(cardEffectBase cardeffect)
    {
        partEffectList.Add(cardeffect);
    }
   
}
