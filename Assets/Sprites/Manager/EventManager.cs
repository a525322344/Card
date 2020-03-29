using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void toDo();
[System.Serializable]
public class EventManager
{
    public EventManager(battleManager _bm)
    {
        battleManager = _bm;
    }
    private battleManager battleManager;

    public List<EventShow> StartEventShows = new List<EventShow>();
    public List<EventShow> BattleEventShows = new List<EventShow>();
    public List<EventShow> EndEventShows = new List<EventShow>();

    public List<EventShow> BattleEnemyShows = new List<EventShow>();

    public List<EventShow> nowEventShowList = new List<EventShow>();
    public EventShow nowEventShow;
    public int testbattleeventnum;
    //事件游标  共用
    public int eventCursor = 0;
    public void EventListUpdate()
    {
        testbattleeventnum = BattleEventShows.Count;
        if (battleManager.BattleRound == BattleState.PlayerRound)
        {
            switch (battleManager.RoundStage)
            {
                case ROUNDSTAGE.Start:
                    if (eventCursor >= StartEventShows.Count)
                    {
                        battleManager.RoundStage = ROUNDSTAGE.Battle;
                        eventCursor = 0;
                        nowEventShowList = BattleEventShows;
                        //回合开始摧毁护盾
                        //gameManager.Instance.battlemanager.battleInfo.Player.destoryArmor(gameManager.Instance.battlemanager.battleInfo.Player.armor);
                    }
                    else
                    {
                        advanceEventList(StartEventShows);
                    }
                    break;
                case ROUNDSTAGE.Battle:
                    if (eventCursor >= BattleEventShows.Count&&battleManager.b_toEndRound)
                    {
                        battleManager.RoundStage = ROUNDSTAGE.End;
                        nowEventShowList = EndEventShows;
                        eventCursor = 0;
                    }
                    else
                    {
                        //保证没按结束按钮的时候，不调用空链表
                        if (BattleEventShows.Count != 0)
                        {
                            advanceEventList(BattleEventShows);
                        }                        
                    }
                    break;
                case ROUNDSTAGE.End:
                    if (eventCursor >= EndEventShows.Count)
                    {
                        battleManager.BattleRound = BattleState.EnemyRound;
                        eventCursor = 0;
                        nowEventShowList = BattleEnemyShows;
                        //
                        gameManager.Instance.battlemanager.battleInfo.Enemy.destoryArmor(gameManager.Instance.battlemanager.battleInfo.Enemy.armor);
                    }
                    else
                    {
                        advanceEventList(EndEventShows);
                    }
                    break;
            }
        }
        else if (battleManager.BattleRound == BattleState.EnemyRound)
        {
            //只进行怪物的
            if (eventCursor >= BattleEnemyShows.Count)
            {
                battleManager.EndEnemyRound();
                eventCursor = 0;
                nowEventShowList = StartEventShows;
            }
            else
            {
                advanceEventList(BattleEnemyShows);
            }
            
        }
        else if (battleManager.BattleRound == BattleState.Vectory)
        {
            //停止执行事件
        }
    }

    void advanceEventList(List<EventShow> eventShows)
    {
        nowEventShowList = eventShows;
        nowEventShow = eventShows[eventCursor];

        battleManager.battleInfo.nowEvent = eventShows[eventCursor].thisevent;
        //先执行一次效果
        if (eventShows[eventCursor].state == EVENTSTATE.Wait)
        {
            //
            eventShows[eventCursor].thisevent.dealEvent(battleManager.battleInfo);
        }
        //在做事件结束判断
        if (eventShows[eventCursor].upDateEvent(battleManager.battleInfo))
        {
            if (eventShows[eventCursor].thisevent.b_logoutAfterDeal)
            {
                eventShows.Remove(eventShows[eventCursor]);
            }
            else
            {
                eventShows[eventCursor].state = EVENTSTATE.Wait;
                eventCursor++;
            }
        }
        //做胜利判断
        if (battleManager.battleInfo.Enemy.healthnow <= 0)
        {
            battleManager.BattleRound = BattleState.Vectory;
            gameManager.Instance.uimanager.roundEndButton.interactable = false;
            gameManager.Instance.uimanager.uiVectorBoard.EnterVectorBoard();
        }
    }

    public void InsertEvent(singleEvent singleevent){
        nowEventShowList.Insert(eventCursor+1,new EventShow(singleevent, nowEventShowList));
    }
    public void AddEvent(singleEvent singleevent)
    {
        nowEventShowList.Add(new EventShow(singleevent,nowEventShowList));
    }
}
