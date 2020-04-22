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
        }

        public static void EnemyGetRealHurt(int num,battleInfo battleInfo)
        {
            if (num < 0)
            {
                num = 0;
            }
            battleInfo.Enemy.realHurtHealth(num);
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

        public static void EnemyDoubleBurn(int num, battleInfo battleinfo)
        {
            stateAbstarct burnstate = new StateBurn(num);
            if (battleinfo.Enemy.nameStatePairs.ContainsKey(burnstate.key))
            {
                battleinfo.Enemy.nameStatePairs[burnstate.key].num *= num;
                gameManager.Instance.battlemanager.realenemy.StateUpdtae();
            }
            else
            {
                burnstate.SetInState();
                battleinfo.Enemy.nameStatePairs.Add(burnstate.key, burnstate);
                battleinfo.Enemy.stateList.Add(burnstate);
                gameManager.Instance.battlemanager.realenemy.StateUpdtae();
            }
        }

        public static void EnemyGetBurn(int num,battleInfo battleinfo)
        {
            stateAbstarct burnstate = new StateBurn(num);
            if (battleinfo.Enemy.nameStatePairs.ContainsKey(burnstate.key))
            {
                battleinfo.Enemy.nameStatePairs[burnstate.key].num += num;
                gameManager.Instance.battlemanager.realenemy.StateUpdtae(); 
            }
            else
            {             
                burnstate.SetInState();
                battleinfo.Enemy.nameStatePairs.Add(burnstate.key, burnstate);
                battleinfo.Enemy.stateList.Add(burnstate);
                gameManager.Instance.battlemanager.realenemy.StateUpdtae();
            }
        }

        public static void RandomLinkPart(int num,battleInfo battleinfo)
        {
            List<realpart> copyrealparts = new List<realpart>(gameManager.Instance.battlemanager.realknapsack.realparts);
            int partnum= copyrealparts.Count;
            List<realpart> getlist = new List<realpart>();
            if (partnum > num)
            {
                for(int i = 0; i < num; i++)
                {
                    realpart get = ListOperation.RandomValue<realpart>(copyrealparts);
                    copyrealparts.Remove(get);
                    getlist.Add(get);
                }
                
                List<MagicPart> getmagicParts = new List<MagicPart>();
                foreach (realpart rp in getlist)
                {
                    if (!battleinfo.havenLinkParts.Contains(rp))
                    {
                        battleinfo.havenLinkParts.Add(rp);
                    }
                    getmagicParts.Add(rp.thisMagicPart);
                }
                LinkPart linkPart = new LinkPart(getmagicParts);
                foreach(realpart rp in getlist)
                {
                    rp.enterLinkPart(linkPart);
                }
            }
            else
            {
                LinkAllPart(0,battleinfo);
            }
        }
        public static void LinkAllPart(int num,battleInfo battleinfo)
        {
            List<realpart> getlist = gameManager.Instance.battlemanager.realknapsack.realparts;
            List<MagicPart> getmagicParts = new List<MagicPart>();
            foreach (realpart rp in getlist)
            {
                if (!battleinfo.havenLinkParts.Contains(rp))
                {
                    battleinfo.havenLinkParts.Add(rp);
                }
                getmagicParts.Add(rp.thisMagicPart);
            }
            LinkPart linkPart = new LinkPart(getmagicParts);
            foreach (realpart rp in getlist)
            {
                rp.enterLinkPart(linkPart);
            }
        }
        public static void CreatState_ExitLinkPart(int num ,battleInfo battleinfo)
        {
            stateAbstarct exitLinkState = new StateExitLinkPart();
            if (!battleinfo.Player.nameStatePairs.ContainsKey(exitLinkState.key)){
                exitLinkState.SetInState();
                battleinfo.Player.nameStatePairs.Add(exitLinkState.key, exitLinkState);
                battleinfo.Player.stateList.Add(exitLinkState);
            }
        }
        public static void ExitLinkPark(int num,battleInfo battleinfo)
        {
            foreach(realpart rp in battleinfo.havenLinkParts)
            {
                rp.exitLinkPart();
            }
        }

        public static void PreSelectCard(int num,battleInfo battleinfo)//准备弃卡
        {
            gameManager.Instance.battlemanager.preWaitToDiscard(num);
        }
        public static void DisCardASelectedCard(int num,battleInfo battleinfo)
        {
            gameManager.Instance.battlemanager.deleteHandCard(num);
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
    public static class judgeAsset
    {
        public static bool EnemyWillAttack(int num,battleInfo battleinfo)
        {
            return ActionOperation.IsActionHasAttack(gameManager.Instance.battlemanager.battleInfo.enemyAction);
        }
        public static bool HaveSelectHandCard(int num,battleInfo battleinfo)
        {
            return gameManager.Instance.battlemanager.battleInfo.realWaitSelectCard.IsFinishSelect();
        }
        public static bool EnemyBurnNumber(int num, battleInfo battleinfo)
        {
            bool EnemyBurnnumber = false;
            stateAbstarct burnstate = new StateBurn(num);
            if (battleinfo.Enemy.nameStatePairs.ContainsKey(burnstate.key))
            {
                if (battleinfo.Enemy.nameStatePairs[burnstate.key].num >= num)
                    EnemyBurnnumber = true;
                else
                    EnemyBurnnumber = false;
            }
            else
            {
                burnstate.SetInState();
                battleinfo.Enemy.nameStatePairs.Add(burnstate.key, burnstate);
                battleinfo.Enemy.stateList.Add(burnstate);
            }
            return EnemyBurnnumber;
        }
        public static bool BuQiHeng(int num, battleInfo battleinfo)
        {
            bool isBuQiHeng = false;
            foreach (Vector2 currentpos in battleinfo.currentPos)
            {
                foreach(Vector2 canusepos in battleinfo.canUsePos)
                {
                    battleinfo.canUsePos.Remove(currentpos);
                    if (currentpos.x == canusepos.x)
                    {
                        isBuQiHeng = true;
                    }
                    else
                        isBuQiHeng = false;
                }
                num++;
            }
            return isBuQiHeng;
        }

        public static bool BuQiShu(int num, battleInfo battleinfo)
        {
            bool isBuQiShu = false;
            foreach (Vector2 currentpos in battleinfo.currentPos)
            {
                foreach (Vector2 canusepos in battleinfo.canUsePos)
                {
                    battleinfo.canUsePos.Remove(currentpos);
                    if (currentpos.y == canusepos.y)
                    {
                        isBuQiShu = true;
                    }
                    else
                        isBuQiShu = false;
                }
                num++;
            }
            return isBuQiShu;
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
                case 0:
                    return "battleTest0";
                case 1:
                    return "battleScene_mounton";
                case 2:
                    return "battleScene_forest";
                case 3:
                    return "battleScene_snow";
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