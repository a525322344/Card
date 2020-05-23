﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattleState
{
    OutOfRound,
    PlayerRound,
    EnemyRound,
    Vectory,
}
public enum ROUNDSTAGE
{
    Start,
    Battle,
    WaitInput,
    End,
}
//此场战斗的信息
[System.Serializable]
public class battleInfo
{
    public pawnbase Enemy;             //敌人
    public pawnbase Player;            //玩家自己
    //当前打出卡牌位置信息
    public List<Vector2> currentPos = new List<Vector2>();

    //已经链接了的部件
    public List<realpart> havenLinkParts = new List<realpart>();
    //当前事件
    public singleEvent nowEvent;
    public CardEvent lastCardEvent;
    //敌人的意图
    public actionAbstract enemyAction;
    //选择的手牌
    public realWaitSelectAB realWaitSelectCard;
    public int selectHandCard = -1;
    public List<int> selectedHandCards = new List<int>();
    //是否得到确认
    public bool selectedConfirm = false;
    //每回合抽卡数量
    public int roundStartDrawCardNum;
    //手牌数量
    public int playerHandCardNum;



    public battleInfo(playerInfo info)
    {
        Enemy = new enemybase();
        Player = new playerpawn();
        Player.name = "阿斯蒂芬";
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
    public ReactionListController ReactionListController;
    //战斗回合状态
    public BattleState BattleRound = BattleState.OutOfRound;
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
    public realEnemy realenemy;
    public monsterInfo monster;
    public realPlayer realplayer;
    public realKnapsack realknapsack;
    public List<realpart> realPartList;
    public List<realCard> realCardList;
    //public battleInfo battleInfoShow;

    public bool b_toEndRound = false;
    #region 选择的牌和部件
    //准备打出卡牌的事件相关信息
    //当前选择的卡
    // [HideInInspector]
    public bool b_isSelectCard;
    public realCard selectedCard;
    public playerCard lastCard;
    public void SetSelectedCard(realCard _playerCard)
    {
        selectedCard = _playerCard;
    }
    //当前选择的部件
    //[HideInInspector]
    public MagicPart selectedPart;
    public MagicPart lastPart;
    public void SetSelectPart(MagicPart _magicPart)
    {
        selectedPart = _magicPart;
    }
    #endregion
    public void InitBattlemanaget()
    {
        //初始化控制器
        playerinfo = gameManager.Instance.playerinfo;
        instantiatemanager = gameManager.Instance.instantiatemanager;
        eventManager = new EventManager(this);
    }
    /// <summary>
    /// 开始战斗
    /// </summary>
    public void startBattale()
    {
        //初始化反应管理器
        ReactionListController = new ReactionListController();
        //初始化战斗卡组/洗牌
        dickInGame = new List<playerCard>(playerinfo.playerDeck);
        dickInGame = ListOperation.Shufle<playerCard>(dickInGame);

        //实例化部件(战斗纸板)
        realknapsack = instantiatemanager.instanBattleKnapsack(playerinfo.playerKnapsack);
        //注册玩家的部件
        realknapsack.SetinPart();
        //初始化battleinfo
        battleInfo=new battleInfo(playerinfo);
        realplayer = instantiatemanager.battleEnvRoot.realplayer;
        realplayer.Init(battleInfo.Player);
        battleInfo.Enemy = realenemy.enemy;

        realCardList = gameManager.Instance.instantiatemanager.battleuiRoot.handCardControll.GetComponent<handcardControll>().playerHandCards;

        //battleInfoShow.Init(playerinfo);
        //SetEnemyShow();
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
        ///先注销enemuControll nowenemy
        actionAbstract newaction = realenemy.chooseAction();
        battleInfo.enemyAction = newaction;
        realenemy.ShowAction(true);
        EventShow enemyNextActionEvent = new EventShow(
            new ActionEvent(newaction),
            eventManager.BattleEnemyShows);
        eventManager.BattleEnemyShows.Add(enemyNextActionEvent);

        RoundStage = ROUNDSTAGE.Start;
        BattleRound = BattleState.PlayerRound;
    }
    
    public void BattleStartEnemySet(monsterInfo monsterinfo)
    {
        //实例化怪物
        instantiatemanager.instanMonster(monsterinfo, out realenemy);
        monster = monsterinfo;
    }

    private void Update()
    {
        eventManager.EventListUpdate();
        if (Input.GetMouseButton(1))
        {
            dickInGame = ListOperation.Shufle<playerCard>(dickInGame);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            BattleRound = BattleState.Vectory;
            EndBattleScene();
            gameManager.Instance.uimanager.roundEndButton.interactable = false;
        }
    }


    /// /////具体操作
    public void ShufleCard()
    {
        dickInGame = ListOperation.Shufle<playerCard>(dickInGame);
    }
    public void DrawACard()
    {
        if (dickInGame.Count == 0)
        {
            dickInGame = new List<playerCard>(dickDiscard);
            dickInGame = ListOperation.Shufle<playerCard>(dickInGame);
            Debug.Log("洗牌");
            dickDiscard.Clear();
        }
        if (dickInGame.Count == 0)
            return;
        playerCard thiscard = dickInGame[0];
        dickInGame.Remove(thiscard);
        dickHandCard.Add(thiscard);

        //实例化卡牌
        instantiatemanager.instanDrawACard(thiscard);
    }
    //得到卡牌
    public void GetACard(playerCard card)
    {
        instantiatemanager.instanDrawACard(card);
    }
    //打出一场卡牌
    public void PlayCard()
    {
        if (RoundStage == ROUNDSTAGE.Battle)
        {
            realknapsack.UseSelectLatices();
            lastCard = selectedCard.thisCard as playerCard;
            lastPart = selectedPart;

            CardEvent newevent = new CardEvent((playerCard)selectedCard.thisCard, selectedPart, new emplyPlayCard());
            newevent.InitPerform();
            EventShow neweventshow = new EventShow(newevent, eventManager.BattleEventShows);
            eventManager.BattleEventShows.Add(neweventshow);

            b_isSelectCard = false;
            //从手牌删掉这张卡
            deleteHandCard(selectedCard);
        }
    }
    public void PlayLastCard()
    {
        CardEvent newevent = new CardEvent((playerCard)lastCard, lastPart, new emplyVPlayCard());
        newevent.InitPerform();
        EventShow neweventshow = new EventShow(newevent, eventManager.BattleEventShows);
        eventManager.BattleEventShows.Add(neweventshow);
    }
    //根据强化动态更新卡牌描述
    public void setCardDescribe(MagicPart magicPart)
    {
        CardEvent cardevent = new CardEvent((playerCard)gameManager.Instance.battlemanager.selectedCard.thisCard, magicPart, new emplyPlayCard());
        cardevent.preCardDescribe();
        //selectedCard.describeText.text = cardevent.EventCardDescribe();
        selectedCard.describeTextPro.text = cardevent.EventCardDescribe();
    }
    public void setCardDescribe(realCard realcard, MagicPart magicPart)
    {
        CardEvent cardevent = new CardEvent((playerCard)realcard.thisCard, magicPart, new emplyPlayCard());
        cardevent.preCardDescribe();
        //realcard.describeText.text = cardevent.EventCardDescribe();
        realcard.describeTextPro.text = cardevent.EventCardDescribe();
    }
    //丢弃全部手牌
    public void deleteAllHandCard()
    {
        for(int i = realCardList.Count - 1; i >= 0; i--)
        {
            realCard indexrealcard = realCardList[i];
            dickHandCard.Remove((playerCard)realCardList[i].thisCard);
            dickDiscard.Add((playerCard)realCardList[i].thisCard);
            realCardList.Remove(realCardList[i]);
            Destroy(indexrealcard.transform.parent.gameObject);
        }
    }
    //丢弃一张手牌
    public void deleteHandCard(realCard real)
    {
        dickHandCard.Remove((playerCard)real.thisCard);
        dickDiscard.Add((playerCard)real.thisCard);
        realCardList.Remove(real);
        Destroy(real.transform.parent.gameObject);
    }
    public void deleteHandCard(int num)
    {
        realCard todiscard = realCardList[num];
        dickHandCard.Remove((playerCard)todiscard.thisCard);
        dickDiscard.Add((playerCard)todiscard.thisCard);
        realCardList.Remove(todiscard);
        Destroy(todiscard.transform.parent.gameObject);
    }
    //储存当前打出卡牌位置信息
    public void CurrentPos(realKnapsack real)
    {

    }

    //储存所有可使用格子信息
    public void CanUsrPos(realKnapsack real)
    {
        
    }



    public void preWaitToDiscard(int num)
    {
        //生成选卡框
        instantiatemanager.instanWaitSelectCardBoard(num,battleInfo.nowEvent);
        //使所有手牌成为等待丢弃状态
        foreach(realCard rc in realCardList)
        {
            rc.EnterStateWaitSelect();
        }
    }
    //(手动挡)怪物回合结束的事
    public void EndEnemyRound()
    {
        b_toEndRound = false;
        realknapsack.PowerLatices();

        //注册怪物的下一次行动事件
        EventShow enemyNextActionEvent = new EventShow(
            new ActionEvent(realenemy.chooseAction()),
            eventManager.BattleEnemyShows
            );
        realenemy.ShowAction(true);
        battleInfo.Player.destoryArmor(battleInfo.Player.armor);
        eventManager.BattleEnemyShows.Add(enemyNextActionEvent);
        RoundStage = ROUNDSTAGE.Start;
        BattleRound = BattleState.PlayerRound;
    }

    //胜利要处理的事情
    public void EndBattleScene()
    {
        for(int i = battleInfo.Enemy.stateList.Count - 1; i >= 0; i--)
        {
            battleInfo.Enemy.stateList[i].SetOutState();
        }
        for (int i = battleInfo.Player.stateList.Count - 1; i >= 0; i--)
        {
            battleInfo.Player.stateList[i].SetOutState();
        }
        StartCoroutine(IEendBattle());

    }
    IEnumerator IEendBattle()
    {
        yield return new WaitForSeconds(0.5f);
        gameManager.Instance.uimanager.uiVectorBoard.Init(monster.monsterLevel);
        instantiateManager.instance.mapRootInfo.uiMapContrill.SetMoney();

        gameManager.Instance.mapmanager.mapplayer.healthSlider.SetSlider(0, gameManager.Instance.playerinfo.playerHealth, gameManager.Instance.playerinfo.playerHealthMax);
        instantiateManager.instance.mapRootInfo.uiMapContrill.healthSlider.SetSlider(0, gameManager.Instance.playerinfo.playerHealth, gameManager.Instance.playerinfo.playerHealthMax);
    }
    //回合结束按钮
    public void ButtonToEndPlayerRound()
    {
        b_toEndRound = true;
    }
}
