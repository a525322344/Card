using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//储存数据
namespace AllAsset
{
    //声明所有的卡片效果
    public static class effectAsset
    {
        public static void dealDemage(int num, battleInfo battleInfo)
        {
            //对敌人造成“num”点伤害。
            if (num < 0)
            {
                num = 0;
            }
            battleInfo.getEnemy().hurtHealth(num);
        }
        public static void gainArmor(int num, battleInfo battleInfo)
        {
            //获得“num”点护甲。
            if (num < 0)
            {
                num = 0;
            }
            gameManager.Instance.PlayerClass.GetArmor(num);
        }
    }
    //声明所有的卡牌
    public static class cardAsset
    {
        //从表格里获得的所有卡
        public static List<playerCard> AllIdCards = new List<playerCard>();

    }

}