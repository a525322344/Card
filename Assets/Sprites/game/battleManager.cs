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
    //玩家要打的手牌
    public playerCard falichongji;
    //部件触发
    public PartTrigger attackUp = new PartTrigger();
    //环境触发
    public List<Trigger> enveTriggerList = new List<Trigger>();
    //延后事件队列
    public List<singleEvent> realtimeEvents = new List<singleEvent>();
    //打出一张牌
    void playerthecard()
    {
        attackUp.addExtraEffect(new extraAttackUp(1));
        CardEvent newcardevent = new CardEvent(falichongji, attackUp);
        newcardevent.SetCommonTrigger(enveTriggerList);
        realtimeEvents.Add(newcardevent);
    }

    // Start is called before the first frame update
    void Start()
    {
        falichongji = AllAsset.cardAsset.AllIdCards[0];
        //falichongji.
        playerthecard();
    }

    // Update is called once per frame
    void Update()
    {
        if (realtimeEvents.Count > 0)
        {
            realtimeEvents[0].dealEffect();
            realtimeEvents[0].launchEffect(battleInfo);
            realtimeEvents.Remove(realtimeEvents[0]);
        }
    }
}
