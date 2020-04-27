using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllAsset;
[System.Serializable]
//整局游戏储存的玩家信息
public class playerInfo
{
    //玩家的牌库
    public List<playerCard> playerDick=new List<playerCard>();
    //玩家的魔纹部件
    public List<MagicPart> MagicPartDick = new List<MagicPart>();
    //玩家的魔法书
    public knapsack playerKnapsack;
    public int roundStartDrawCardNum = 5;
    public int playerHealthMax = 100;
    public int playerHealth = 100;
    public int money=200;

    public void AddNewCard(playerCard card)
    {
        playerDick.Add(card);
    }
    public void AddNewPart(MagicPart magic)
    {
        MagicPartDick.Add(magic);
    }
    //初始化玩家的开局卡组
    public void PlayerDickInit(List<int> ts)
    {
        playerDick.Clear();
        foreach(int i in ts)
        {
            playerDick.Add(cardAsset.AllIdCards[i]);
        }
    }

    //初始化玩家的
    public void MagicPartDickInit()
    {
        MagicPartDick.Clear();
        MagicPartDick.Add(magicpartAsset.AllMagicParts[0]);
        MagicPartDick.Add(magicpartAsset.AllMagicParts[1]);
    }
    public void KnapSackInit(bool[] ise)
    {
        playerKnapsack = new knapsack(ise);
    }
}
