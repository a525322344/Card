using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllAsset;
namespace AllAsset
{
    //声明所有的卡片效果
    public static class effectAsset
    {
        public static void giveDemage(int num)
        {
            //对敌人造成“num”点伤害。
            gameManager.Instance.EmenyClass.HurtHealth(num);
        }
        public static void getArmor(int num)
        {
            //获得“num”点护甲。
            gameManager.Instance.PlayerClass.GetArmor(num);
        }
    }
    //声明所有的卡牌
    public static class cardAsset
    {
        //从表格里获得的所有卡
        public static List<playerCard> AllIdCards=new List<playerCard>();

    }
    
}

public class InitAsset : MonoBehaviour
{
    private void Awake()
    {
        cardAsset.AllIdCards.Add(new playerCard(0, "法力冲击", CardKind.PlayerCard, 5, 0));
        cardAsset.AllIdCards.Add(new playerCard(1, "护盾", CardKind.PlayerCard, 0, 5));

        gameManager.Instance.playerAsset.playerDick.Add(cardAsset.AllIdCards[0]);
        gameManager.Instance.playerAsset.playerDick.Add(cardAsset.AllIdCards[0]);
        gameManager.Instance.playerAsset.playerDick.Add(cardAsset.AllIdCards[0]);
        gameManager.Instance.playerAsset.playerDick.Add(cardAsset.AllIdCards[1]);
        gameManager.Instance.playerAsset.playerDick.Add(cardAsset.AllIdCards[1]);
        gameManager.Instance.playerAsset.playerDick.Add(cardAsset.AllIdCards[1]);
        //读取表格
    }
}
