using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//储存数据
namespace AllAsset
{
    //声明所有的效果，供委托，无标签
    public static class effectAsset
    {
        public static void EmptyEffect(int num,battleInfo battleinfo) { }
        public static void EnemyGetHurt(int num, battleInfo battleInfo)
        {
            //对敌人造成“num”点伤害。
            if (num < 0)
            {
                num = 0;
            }
            battleInfo.Enemy.hurtHealth(num);
            gameManager.Instance.battlemanager.showcontroll.gethurt(num);
        }
        public static void EnemyGetRealHurt(int num,battleInfo battleInfo)
        {
            if (num < 0)
            {
                num = 0;
            }
            battleInfo.Enemy.healthnow -= num;
            gameManager.Instance.battlemanager.showcontroll.gethurt(num);
        }
        public static void PlayerGetArmor(int num, battleInfo battleInfo)
        {
            //获得“num”点护甲。
            if (num < 0)
            {
                num = 0;
            }
            battleInfo.Player.GetArmor(num);
        }
        public static void drawACard(int num,battleInfo battleInfo)
        {
            gameManager.Instance.battlemanager.DrawACard();
        }
        public static void disAllCard(int num,battleInfo battleInfo)
        {
            gameManager.Instance.battlemanager.deleteAllHandCard();
        }
        public static void PlayerGetHurt(int num,battleInfo battleInfo)
        {
            if (num < 0)
            {
                num = 0;
            }
            battleInfo.Player.hurtHealth(num);
        }
        public static void EnemyGetArmor(int num,battleInfo battleinfo)
        {
            if (num < 0)
            {
                num = 0;
            }
            battleinfo.Enemy.GetArmor(num);
        }
        public static void EnemyGetBurn(int num,battleInfo battleinfo)
        {
            if (battleinfo.Enemy.nameStatePairs.ContainsKey("Burn"))
            {
                battleinfo.Enemy.nameStatePairs["Burn"].num += num;
                gameManager.Instance.battlemanager.showcontroll.ShowFire(battleinfo.Enemy.nameStatePairs["Burn"].num);
            }
            else
            {
                stateAbstarct burnstate = new StateBurn(num);               
                burnstate.SetInState();
                battleinfo.Enemy.nameStatePairs.Add("Burn", burnstate);
                battleinfo.Enemy.stateList.Add(burnstate);
                gameManager.Instance.battlemanager.showcontroll.ShowFire(battleinfo.Enemy.nameStatePairs["Burn"].num);
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
    public static class MapAsset
    {
        public static List<monsterInfo> AllMonsters = new List<monsterInfo>();
        public static string GetSceneStr(int id)
        {
            switch (id)
            {
                case 1:
                    return "myFirstBattle";
                case 2:
                    return "battleTest0";
                default:
                    Debug.Log("没有设置");
                    return "";
            }
        }
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