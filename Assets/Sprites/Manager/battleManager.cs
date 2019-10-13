using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public pawnbase Enemy;             //敌人
    public pawnbase Player;            //玩家自己
    public List<extraEffectBase> CommonExtraEffects = new List<extraEffectBase>();
    public int roundStartDrawCardNum;
    public int playerHandCardNum;

    public battleInfo(playerInfo info)
    {
        Enemy = new enemybase();
        Player = new playerpawn();
        Player.healthmax = info.playerHealthMax;
        Player.healthnow = info.playerHealth;
        Player.armor = 0;
        roundStartDrawCardNum = info.roundStartDrawCardNum;
    }
    public void setHandCardNum()
    {
        playerHandCardNum = gameManager.Instance.battlemanager.dickHandCard.Count;
    }
}


public class battleManager : MonoBehaviour
{
    instantiateManager instantiatemanager;
    public EventManager eventManager;
    
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
    //public battleInfo battleInfoShow;

    public bool b_toEndRound = false;
    #region 选择的牌和部件
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
    #endregion


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
        battleInfo=new battleInfo(playerinfo);
        //battleInfoShow.Init(playerinfo);
        SetEnemyShow();
        //注册回合抽牌事件
        EventShow drawcardShowEvent = new EventShow(
            new SystemEvent(new RoundStartDrawCard(battleInfo.roundStartDrawCardNum)),
            eventManager.StartEventShows);
        eventManager.StartEventShows.Add(drawcardShowEvent);
        //注册回合结束弃牌事件
        EventShow discardShowEvent = new EventShow(
            new SystemEvent(new RoundEndDisCard(battleInfo.playerHandCardNum)), 
            eventManager.EndEventShows);
        eventManager.EndEventShows.Add(discardShowEvent);
        //注册怪物的下一次行动事件
        EventShow enemyNextActionEvent = new EventShow(
            new ActionEvent(nowenemy.chooseAction()),
            eventManager.BattleEnemyShows);
        eventManager.BattleEnemyShows.Add(enemyNextActionEvent);

        RoundStage = ROUNDSTAGE.Start;
        BattleRound = ROUND.PlayerRound;
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
            CardEvent newevent = new CardEvent((playerCard)selectedCard.thisCard, selectedPart.thisMagicPart, new emplyPlayCard());
            EventShow neweventshow = new EventShow(newevent, eventManager.BattleEventShows);
            eventManager.BattleEventShows.Add(neweventshow);
            newevent.prepareEvent();
            newevent.insertEvent();

            selectedCard.realcost.lastrealgrid.fatherPart.SetDownCard(null);
            gameManager.Instance.battlemanager.b_isSelectCard = false;
            //从手牌删掉这张卡
            deleteHandCard(selectedCard);

        }
    }

    public void setCardDescribe(MagicPart magicPart)
    {
        CardEvent cardevent = new CardEvent((playerCard)gameManager.Instance.battlemanager.selectedCard.thisCard, magicPart, new emplyPlayCard());
        cardevent.prepareEvent();
        selectedCard.describeText.text = cardevent.EventCardDescribe();        
    }

    //丢弃全部手牌
    public void deleteAllHandCard()
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
    //回合结束的事
    public void EndEnemyRound()
    {
        b_toEndRound = false;

        foreach(realpart rp in gameManager.Instance.instantiatemanager.bookFolderTran.GetComponent<bookFolderControll>().realparts){
            rp.PowerRealPart();
        }
        //注册怪物的下一次行动事件
        EventShow enemyNextActionEvent = new EventShow(
            new ActionEvent(nowenemy.chooseAction()),
            eventManager.BattleEnemyShows
            );
        eventManager.BattleEnemyShows.Add(enemyNextActionEvent);
        RoundStage = ROUNDSTAGE.Start;
        BattleRound = ROUND.PlayerRound;
    }


    //回合结束按钮
    public void ButtonToEndPlayerRound()
    {
        b_toEndRound = true;
    }
    #region 临时的
    public showcontroll showcontroll;
    public enemyControll nowenemy;
    public void SetEnemyShow()
    {
        battleInfo.Enemy = nowenemy.pikaqiu;
        showcontroll.init();
    }

    public Slider healthSlider;
    public Text healthText;
    private void Update()
    {
        healthSlider.value =(float) battleInfo.Player.healthnow/battleInfo.Player.healthmax;
        healthText.text = "" + battleInfo.Player.healthnow + " / " + battleInfo.Player.healthmax;

    }
    #endregion

}
