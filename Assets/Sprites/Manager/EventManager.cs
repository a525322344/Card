using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void toDo();

public class EventManager : MonoBehaviour
{
    public battleManager battleManager;

    public List<EventShow> StartEventShows = new List<EventShow>();
    public List<EventShow> BattleEventShows = new List<EventShow>();
    public List<EventShow> EndEventShows = new List<EventShow>();

    public List<EventShow> BattleEnemyShows = new List<EventShow>();

    public int testbattleeventnum;
    //事件游标  共用
    public int eventCursor = 0;
    private void Update()
    {
        testbattleeventnum = BattleEventShows.Count;
        if (battleManager.BattleRound == ROUND.PlayerRound)
        {
            switch (battleManager.RoundStage)
            {
                case ROUNDSTAGE.Start:
                    if (eventCursor >= StartEventShows.Count)
                    {
                        battleManager.RoundStage = ROUNDSTAGE.Battle;
                        eventCursor = 0;
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
                        battleManager.BattleRound = ROUND.EnemyRound;
                        eventCursor = 0;
                    }
                    else
                    {
                        advanceEventList(EndEventShows);
                    }
                    break;
            }
        }
        else if (battleManager.BattleRound == ROUND.EnemyRound)
        {
            //只进行怪物的
            if (eventCursor >= BattleEnemyShows.Count)
            {
                battleManager.EndEnemyRound();
                eventCursor = 0;
            }
            else
            {
                advanceEventList(BattleEnemyShows);
            }
            
        }
    }

    void advanceEventList(List<EventShow> eventShows)
    {
        switch (eventShows[eventCursor].state)
        {
            case EVENTSTATE.Wait:
                eventShows[eventCursor].state = EVENTSTATE.Do;
                eventShows[eventCursor].thisevent.dealEffect(battleManager.battleInfo);
                break;
            case EVENTSTATE.Do:
                eventShows[eventCursor].upDateEvent();
                eventShows[eventCursor].state = EVENTSTATE.Over;
                break;
            case EVENTSTATE.Over:
                if (eventShows[eventCursor].thisevent.b_logoutAfterDeal)
                {
                    eventShows.Remove(eventShows[eventCursor]);
                }
                else
                {
                    eventShows[eventCursor].state = EVENTSTATE.Wait;
                    eventCursor++;
                }
                break;
        }
    }
}
