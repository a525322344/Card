using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ROUND
{
    OutOfRound,
    PlayerRound,
    EnemyRound
}
public enum ROUNDSTAGE
{
    Start,
    Battle,
    End,
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
    public playerpawn Player;            //玩家自己
    public List<extraEffectBase> CommonExtraEffects = new List<extraEffectBase>();
    public int roundStartDrawCardNum;

    public void Init(playerInfo info)
    {
        Player.healthmax = info.playerHealthMax;
        Player.healthnow = info.playerHealth;
        Player.armor = 0;
        roundStartDrawCardNum = info.roundStartDrawCardNum;
    }
}


public class battleManager : MonoBehaviour
{
    instantiateManager instantiatemanager;
    EventManager eventManager;
    
    //战斗回合状态
    public ROUND BattleRound = ROUND.OutOfRound;
    public ROUNDSTAGE RoundStage = ROUNDSTAGE.Start;
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
    public battleInfo battleInfoShow;

    public bool b_toEndRound = false;
    //准备打出卡牌的事件相关信息
    //当前选择的卡
    // [HideInInspector]
    public bool b_isSelectCard;
    public realCard selectedCard;
    public void SetSelectedCard(realCard _playerCard)
    {
        selectedCard = _playerCard;
    }
    //当前选择的部件
    //[HideInInspector]
    public realpart selectedPart;
    public void SetSelectPart(realpart _magicPart)
    {
        selectedPart = _magicPart;
    }


    public showcontroll showcontroll;

    /// <summary>
    /// 开始战斗
    /// </summary>
    public void startBattale()
    {
        playerinfo = gameManager.Instance.playerinfo;
        instantiatemanager = gameManager.Instance.instantiatemanager;
        eventManager = GetComponent<EventManager>();
        eventManager.battleManager = this;
        //初始化战斗卡组/洗牌
        dickInGame = new List<playerCard>(playerinfo.playerDick);
        dickInGame = ListOperation.Shufle<playerCard>(dickInGame);
        //实例化部件
        instantiatemanager.instanBattleStartPart(playerinfo.MagicPartDick);
        //注册玩家的部件
        playerinfo.MagicPartDick[0].SetinReactions();
        playerinfo.MagicPartDick[1].SetinReactions();
        //初始化battleinfo
        battleInfo.Init(playerinfo);
        battleInfoShow.Init(playerinfo);
        SetEnemyShow();
        //注册回合抽牌事件
        singleEvent drawCardEvent = new SystemEvent(new RoundStartDrawCard(battleInfo.roundStartDrawCardNum));
        EventShow drawcardShowEvent = new EventShow(drawCardEvent);
        eventManager.StartEventShows.Add(drawcardShowEvent);

        RoundStage = ROUNDSTAGE.Start;
        BattleRound = ROUND.PlayerRound;
    }
    public void SetEnemyShow()
    {
        showcontroll.init();
    }
    public void DrawACard()
    {
        if (dickInGame.Count == 0)
        {
            dickInGame = new List<playerCard>(dickDiscard);
            dickInGame = ListOperation.Shufle<playerCard>(dickInGame);
            dickDiscard.Clear();
        }
        playerCard thiscard = dickInGame[dickInGame.Count - 1];
        dickInGame.Remove(thiscard);
        dickHandCard.Add(thiscard);

        //实例化卡牌
        instantiatemanager.instanDrawACard(thiscard);
    }
    //打出一场卡牌
    public void PlayCard()
    {
        if (RoundStage == ROUNDSTAGE.Battle)
        {
            selectedPart.UseSelectGrids();
            singleEvent newevent = new CardEvent((playerCard)selectedCard.thisCard, selectedPart.thisMagicPart, new emplyPlayCard());
            newevent.dealEffect(battleInfo);
            eventManager.BattleEventShows.Add(new EventShow(newevent));

            selectedCard.realcost.lastrealgrid.fatherPart.SetDownCard(null);
            gameManager.Instance.battlemanager.b_isSelectCard = false;
            //从手牌删掉这张卡
            deleteHandCard(selectedCard);

        }
    }
    //丢弃全部手牌
    public void deleteHandCard()
    {
        List<realCard> realCards = gameManager.Instance.instantiatemanager.handCardControll.GetComponent<handcardControll>().playerHandCards;
        for(int i = realCards.Count - 1; i >= 0; i--)
        {
            realCard indexrealcard = realCards[i];
            dickHandCard.Remove((playerCard)realCards[i].thisCard);
            dickDiscard.Add((playerCard)realCards[i].thisCard);
            realCards.Remove(realCards[i]);
            Destroy(indexrealcard.transform.parent.gameObject);
        }
    }
    //丢弃一张手牌
    public void deleteHandCard(realCard real)
    {
        dickHandCard.Remove((playerCard)real.thisCard);
        dickDiscard.Add((playerCard)real.thisCard);
        gameManager.Instance.instantiatemanager.handCardControll.GetComponent<handcardControll>().playerHandCards.Remove(real);
        Destroy(real.transform.parent.gameObject);
    }
    public void ToEndPlayerRound()
    {
        b_toEndRound = true;
    }

    public void EndEnemyRound()
    {
        singleEvent drawCardEvent = new SystemEvent(new RoundStartDrawCard(battleInfo.roundStartDrawCardNum));
        EventShow drawcardShowEvent = new EventShow(drawCardEvent);
        eventManager.StartEventShows.Add(drawcardShowEvent);
        b_toEndRound = false;

        foreach(realpart rp in gameManager.Instance.instantiatemanager.bookFolderTran.GetComponent<bookFolderControll>().realparts){
            rp.PowerRealPart();
        }

        RoundStage = ROUNDSTAGE.Start;
        BattleRound = ROUND.PlayerRound;
    }
}
