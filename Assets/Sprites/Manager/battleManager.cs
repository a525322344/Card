using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ROUND
{
    PlayerRound,
    EnemyRound
}

//此场战斗的信息
[System.Serializable]
public class battleInfo
{
    public enemybase getEnemy()
    {
        return Enemy;
    }
    public enemybase getPlayer()
    {
        return Player;
    }
    public enemybase Enemy;             //敌人
    public enemybase Player;            //玩家自己
    public List<extraEffectBase> CommonExtraEffects = new List<extraEffectBase>();

}


public class battleManager : MonoBehaviour
{
    instantiateManager instantiatemanager;

    //战斗回合状态
    public ROUND BattleRound = ROUND.PlayerRound;
    //玩家数据类
    public playerInfo playerinfo;
    //玩家的卡组
    public List<playerCard> dickInGame = new List<playerCard>();
    //玩家的弃牌堆
    public List<playerCard> dickDiscard = new List<playerCard>();
    //玩家的手牌
    public List<playerCard> dickHandCard = new List<playerCard>();

    //战场信息
    public battleInfo battleInfo;

    public int testDrawCardnum;


    //准备打出卡牌的事件相关信息
    //当前选择的卡
    // [HideInInspector]
    public bool b_isSelectCard;
    public card selectedCard;
    public void SetSelectedCard(card _playerCard)
    {
        selectedCard = _playerCard;
    }
    //当前选择的部件
    [HideInInspector]
    public MagicPart selectedPart;
    public void SetSelectPart(MagicPart _magicPart)
    {
        selectedPart = _magicPart;
    }



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
    /// 开始战斗
    /// </summary>
    public void startBattale()
    {
        playerinfo = gameManager.Instance.playerinfo;
        instantiatemanager = gameManager.Instance.instantiatemanager;
        //初始化战斗卡组/洗牌
        dickInGame = new List<playerCard>(playerinfo.playerDick);
        dickInGame = ListOperation.Shufle<playerCard>(dickInGame);
        //实例化部件
        instantiatemanager.instanBattleStartPart(playerinfo.MagicPartDick);
    }

    /// <summary>
    /// 你的回合开始
    /// </summary>
    public void startRound()
    {
        //按抽牌数抽牌
        for (int i = 0; i < testDrawCardnum; i++)
        {
            playerCard thiscard = dickInGame[dickInGame.Count - 1];
            dickInGame.Remove(thiscard);
            dickHandCard.Add(thiscard);

            //实例化卡牌
            instantiatemanager.instanDrawACard(thiscard);
        }
    }

}
