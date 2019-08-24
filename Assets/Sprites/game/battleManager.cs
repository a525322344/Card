using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//此场战斗的信息
[System.Serializable]
public class battleInfo
{
    public enemybase getEnemy()
    {
        return Enemy;
    }
    public enemybase Enemy;             //敌人
    public enemybase Player;            //玩家自己
    public List<extraEffectBase> CommonExtraEffects = new List<extraEffectBase>();

}


public class battleManager : MonoBehaviour
{
    //战场信息
    public battleInfo battleInfo;
    //卡牌
    public playerCard falichongji;
    //部件
    public Part PatrAttackUp = new MagicPart();

    //延后事件队列
    public List<singleEvent> realtimeEvents = new List<singleEvent>();

    //初始化信息
    void initSetting()
    {
        //完成部件
        Reaction attackUpReaction = new Reaction_Affect(new extraAttackUp(1), ReactionKind.Reaction_Affect_DamageUp);
        PatrAttackUp.addAndSetin(attackUpReaction);
    }
    //打出一张牌
    void playerthecard()
    {
        CardEvent newcardevent = new CardEvent(falichongji,(MagicPart)PatrAttackUp,new emplyPlayCard());
        newcardevent.recesiveNotice();
        realtimeEvents.Add(newcardevent);
    }

    // Start is called before the first frame update
    void Start()
    {
        falichongji = AllAsset.cardAsset.AllIdCards[0];
        initSetting();
        playerthecard();

 
    }

    // Update is called once per frame
    void Update()
    {
        if (realtimeEvents.Count > 0)
        {
            realtimeEvents[0].dealEffect(battleInfo);
            realtimeEvents.Remove(realtimeEvents[0]);
        }
    }
    //[System.Serializable]
    public class a
    {
        public a(int a)
        {
            _a = a;
        }
        public void seta(int i)
        {
            _a = i;
        }
        public int _a;
    }
    public List<a> aList = new List<a>();
    public List<a> As = new List<a>();
    void test()
    {
        //test
        a a1 = new a(1);
        a a2 = new a(2);
        aList.Add(a1);
        aList.Add(a2);

        As.Add(aList[1]);
        As.Add(aList[0]);
        aList[0].seta(10);
    }
}
