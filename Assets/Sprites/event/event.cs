using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//事件触发器，响应事件的后，可以影响事件，产生事件
public abstract class eventTrigger
{
    public abstract bool canInffulenceEvent(singleEvent singleEvent);
    public virtual void dealTriggerEffect() { }
    public virtual int adjustTriggerEffect(int _num) { return _num; }
}

//一张卡有n个效果；对于每个效果，建立一个影响效果链表

//打出卡牌受到各种加成，buff影响，怪物能力调整的事件——的基类
public abstract class singleEvent
{
    public abstract void dealEffect(battleInfo battleinfo);

}

public abstract class triggerEvent : singleEvent
{

}
public class PartEvent : singleEvent
{
    public override void dealEffect(battleInfo battleinfo)
    {

    }
    public List<extraEffectBase> getExtraEffects()
    {
        return extraEffects;
    }
    public List<extraEffectBase> extraEffects;
}
public class CardEvent : singleEvent
{
    //public void setCardEvent(playerCard _playercard, singleEvent _partextraEvent,List<singleEvent> _extraEvents)
    //{
    //    CardEffectList = _playercard.EffectPlayList;
    //    List<extraEffectBase> extraEffects = new List<extraEffectBase>();
    //    foreach(singleEvent single in _extraEvents)
    //    {
    //        //foreach(extraEffectBase extra in single.get)
    //    }
    //    //对于卡的每一个单独效果，建立一个“额外加成表”，把所有的试用加成效果添加到表中。
    //    foreach (cardEffectBase effect in _effectclasses)
    //    {
    //        List<extraEffectBase> thisEffectExtarList = new List<extraEffectBase>();
    //        //遍历场上的加成效果，将试用的添加到该“额外加成表”中
    //        foreach (extraEffectBase inplayEffect in inplayExtarEffects)
    //        {
    //            if (inplayEffect.canInffence(effect))
    //            {
    //                thisEffectExtarList.Add(inplayEffect);
    //            }
    //        }
    //        extraEffectLists.Add(thisEffectExtarList);
    //    }
    //}
    public override void dealEffect(battleInfo battleinfo)
    {
        for (int i = 0; i < CardEffectList.Count; i++)
        {
            int index = CardEffectList[i].getNum();
            foreach (extraEffectBase extra in extraEffectLists[i])
            {
                index = extra.AdjustEffect(index);
            }
            CardEffectList[i].getCardEffect()(index, battleinfo);
        }
    }

    private List<cardEffectBase> CardEffectList;
    private List<List<extraEffectBase>> extraEffectLists = new List<List<extraEffectBase>>();
}