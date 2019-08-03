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
    //实时事件队列
    public List<singleEvent> realtimeEvents = new List<singleEvent>();
    //打出一张牌
    void playerthecard(playerCard playerCard,singleEvent partevent)
    {
        singleEvent newcardevent = new CardEvent();
    }
    // Start is called before the first frame update
    void Start()
    {
        falichongji = AllAsset.cardAsset.AllIdCards[0];
    }

    // Update is called once per frame
    void Update()
    {
        //if (realtimeEvents.Count > 0)
        //{
        //    realtimeEvents[0].dealEffect(battleInfo);
        //    realtimeEvents.Remove(realtimeEvents[0]);
        //}
    }
}
