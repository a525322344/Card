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
            battleInfo.getPlayer().GetArmor(num);
        }
        public static void drawCard(int num,battleInfo battleInfo)
        {
            for(int i = 0; i < num; i++)
            {
                gameManager.Instance.battlemanager.DrawACard();
            }
        }
    }
    //声明所有的额外强化效果
    public static class extraAsset
    {
        public static int addSubNum(int _num,int adjust)
        {
            return _num + adjust;
        }
    }
    //声明所有的卡牌
    public static class cardAsset
    {
        //从表格里获得的所有卡
        public static List<playerCard> AllIdCards = new List<playerCard>();
    }
    //声明所有的魔力部件
    public static class magicpartAsset
    {
        public static List<MagicPart> AllMagicParts = new List<MagicPart>();
    }

}

namespace Constant
{
    public class Path
    {
        public const string GRID_MATERIAL_CYAN = "Materials/Grid/grid_cyan";
        public const string GRID_MATERIAL_BLACK = "Materials/Grid/grid_black";
        public const string GRID_MATERIAL_GRAY = "Materials/Grid/grid_gray";
        public const string GRID_MATERIAL_WRITE = "Materials/Grid/grid_write";
        public const string MATERIAL_COST_CYAN = "Materials/Grid/cost_cyan";
        public const string MATERIAL_COST_BLUE = "Materials/Grid/cost_blue";
    } 
    
}