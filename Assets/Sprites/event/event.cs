using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一张卡有n个效果；对于每个效果，建立一个影响效果链表

//打出卡牌受到各种加成，buff影响，怪物能力调整的事件——的基类
public abstract class singleEvent
{
    public abstract void dealEffect();
    public abstract void launchEffect(battleInfo battle);
    public List<cardEffectBase> getEffectList()
    {
        return EffectList;
    }
    public List<List<extraEffectBase>> extraEffectLists = new List<List<extraEffectBase>>();
    public List<cardEffectBase> EffectList = new List<cardEffectBase>();
}

public abstract class triggerEvent : singleEvent
{

}

public class CardEvent : singleEvent
{
    /// <summary>
    /// 卡牌事件构造函数
    /// </summary>
    /// <param name="_playerCard">打出的卡牌</param>
    /// <param name="_parttrigger">发出部件</param>
    public CardEvent(playerCard _playerCard,Trigger _parttrigger)
    {
        playercard = _playerCard;
        CardEffectList = _playerCard.getEffectList();
        Parttrigger = _parttrigger;
      
    }
    public void SetCommonTrigger(List<Trigger> triggerlist)
    {
        pTriggers = triggerlist;
    }
    //处理额外效果
    public override void dealEffect()
    {
        //单独处理部件的触发
        //对于卡牌的每个效果，记录其可以触发的额外效果
        for(int i = 0; i < CardEffectList.Count; i++)
        {
            //对于每一个部件的强化，判断其是否可以影响效果
            List<extraEffectBase> ExtraList = Parttrigger.GetExtraList();

            List<extraEffectBase> canextralist = new List<extraEffectBase>();
            int extracount = ExtraList.Count;
            for(int j = 0; j < extracount; j++)
            {              
                if (ExtraList[j].canInfluence(CardEffectList[i])){
                    canextralist.Add(ExtraList[j]);
                }
            }
            extraEffectLists.Add(canextralist);
        }
        for(int t = 0; t < pTriggers.Count; t++)
        {
            for (int i = 0; i < CardEffectList.Count; i++)
            {
                List<extraEffectBase> ExtraList = pTriggers[t].GetExtraList();

                int extracount = ExtraList.Count;
                for (int j = 0; j < extracount; j++)
                {
                    if (ExtraList[j].canInfluence(CardEffectList[i]))
                    {
                        extraEffectLists[j].Add(ExtraList[j]);
                    }
                }
            }
        }
    }
    //发动效果
    public override void launchEffect(battleInfo battle)
    {
        for(int c = 0; c < CardEffectList.Count; c++)
        {
            int index=CardEffectList[c].getNum();
            for(int e = 0; e < extraEffectLists[c].Count; e++)
            {
                index = extraEffectLists[c][e].AdjustEffect(CardEffectList[c].getNum());
            }
            CardEffectList[c].getEffect()(index, battle);
        }
    }
    //触发的触发器
    private Trigger Parttrigger;
    private List<Trigger> pTriggers;
    //卡牌
    private playerCard playercard;
    private List<cardEffectBase> CardEffectList;
    private List<List<extraEffectBase>> extraEffectLists = new List<List<extraEffectBase>>();
}