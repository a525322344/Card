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
    public int roundStartDrawCardNum = 5;
    public int playerHealthMax = 100;
    public int playerHealth = 100;


    
    //初始化玩家的开局卡组
    public void PlayerDickInit()
    {
        playerDick.Add(cardAsset.AllIdCards[0]);
        playerDick.Add(cardAsset.AllIdCards[0]);
        playerDick.Add(cardAsset.AllIdCards[1]);
        playerDick.Add(cardAsset.AllIdCards[1]);
        playerDick.Add(cardAsset.AllIdCards[4]);
        playerDick.Add(cardAsset.AllIdCards[2]);
        playerDick.Add(cardAsset.AllIdCards[2]);
        playerDick.Add(cardAsset.AllIdCards[3]);
        playerDick.Add(cardAsset.AllIdCards[5]);
    }

    //初始化玩家的
    public void MagicPartDickInit()
    {
        MagicPartDick.Add(magicpartAsset.AllMagicParts[0]);
        MagicPartDick.Add(magicpartAsset.AllMagicParts[1]);
    }
}
