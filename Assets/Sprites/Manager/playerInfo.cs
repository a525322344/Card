using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllAsset;
[System.Serializable]
//整局游戏储存的玩家信息
public class playerInfo
{
    //玩家的牌库
    public List<playerCard> playerDeck=new List<playerCard>();
    //玩家的魔纹部件
    public List<MagicPart> MagicPartDick = new List<MagicPart>();
    //玩家的魔法书
    public knapsack playerKnapsack;
    //战场buff
    public List<BattleBuff> battleBuffList = new List<BattleBuff>();
    //每回合抽牌数
    public int roundStartDrawCardNum = 5;
    //宝箱选择项数
    public int treasureToSelectNum = 3;

    public int playerHealthMax = 100;
    public int playerHealth = 100;
    public int money=200;
    //添加新卡牌
    public void AddNewCard(playerCard card)
    {
        playerDeck.Add(card);
    }
    //升级卡牌
    public void UpgradeCard(playerCard card)
    {
        if (card.IsGrade == false)
        {
            playerDeck.Add(cardAsset.AllGradeCards[card.Id]);
            playerDeck.Remove(card);
        }
        else
        {
            Debug.Log("错误");
        }
    }
    //移除卡牌
    public void RemoveCard(playerCard card)
    {
        if (playerDeck.Contains(card))
        {
            playerDeck.Remove(card);
        }
    }
    //获得金钱
    public void GetMoney(int m)
    {
        money += m;
        if (money < 0)
        {
            money = 0;
        }
        instantiateManager.instance.mapRootInfo.uiMapContrill.SetMoney();
    }
    //获得诅咒卡
    public void AddCurseCard()
    {

    }
    public void AddBattleBuff(BattleBuff buff)
    {
        battleBuffList.Add(buff);
    }
    //获得部件
    public void AddMagicPart(MagicPart magicPart=null)
    {
        if (magicPart != null)
        {
            MagicPartDick.Add(new MagicPart(magicPart));
        }
    }
    public void RecoveryHealth(int h)
    {
        playerHealth += h;
        if (playerHealth > playerHealthMax)
        {
            playerHealth = playerHealthMax;
        }
        gameManager.Instance.mapmanager.mapplayer.healthSlider.SetSlider(0, gameManager.Instance.playerinfo.playerHealth);
    }

    //初始化玩家的开局卡组
    public void PlayerDickInit(List<int> ts)
    {
        playerDeck.Clear();
        foreach(int i in ts)
        {
            playerDeck.Add(cardAsset.AllIdCards[i]);
        }
    }

    //初始化玩家的
    public void MagicPartDickInit()
    {
        MagicPartDick.Clear();
        AddMagicPart(magicpartAsset.AllMagicParts[0]);
        AddMagicPart(magicpartAsset.AllMagicParts[1]);
    }
    public void KnapSackInit(bool[] ise)
    {
        playerKnapsack = new knapsack(ise);
        playerKnapsack.installParts.Add(new Vector2(1, 2), MagicPartDick[0]);
        playerKnapsack.installParts.Add(new Vector2(2, 2), MagicPartDick[1]);
        //MagicPartDick.Remove(MagicPartDick[0]);
        //MagicPartDick.Remove(MagicPartDick[1]);
    }
}

public class BattleBuff
{
    public BattleBuff(string name,int times)
    {
        buffName = name;
        Times = times;
    }
    public string buffName;
    public string buffDescrie;
    public int Times;
}