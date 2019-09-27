using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EVENTSTATE
{
    Wait,
    Do,
    Over
}

public class EventShow
{
    public EventShow(singleEvent _event)
    {
        thisevent = _event;
        state = EVENTSTATE.Wait;
    }
    public void upDateEvent()
    {
        lasttime -= Time.deltaTime;
    }
    public float lasttime;
    public singleEvent thisevent;
    public EVENTSTATE state;
}
public class EventManager : MonoBehaviour
{
    public battleManager battleManager;

    public List<EventShow> StartEventShows = new List<EventShow>();
    public List<EventShow> BattleEventShows = new List<EventShow>();
    public List<EventShow> EndEventShows = new List<EventShow>();

    public int testbattleeventnum;

    private void Update()
    {
        testbattleeventnum = BattleEventShows.Count;
        if (battleManager.BattleRound == ROUND.PlayerRound)
        {
            switch (battleManager.RoundStage)
            {
                case ROUNDSTAGE.Start:
                    if (StartEventShows.Count == 0)
                    {
                        battleManager.RoundStage = ROUNDSTAGE.Battle;
                    }
                    else
                    {
                        advanceEventList(StartEventShows);
                    }
                    break;
                case ROUNDSTAGE.Battle:
                    //应该再加一个按了回合结束以后
                    if (BattleEventShows.Count == 0&&battleManager.b_toEndRound)
                    {
                        battleManager.RoundStage = ROUNDSTAGE.End;
                    }
                    else
                    {
                        if (BattleEventShows.Count != 0)
                        {
                            advanceEventList(BattleEventShows);
                        }                        
                    }
                    break;
                case ROUNDSTAGE.End:
                    if (EndEventShows.Count == 0)
                    {
                        battleManager.BattleRound = ROUND.EnemyRound;
                        battleManager.deleteHandCard();
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
            battleManager.EndEnemyRound();
        }
    }

    void advanceEventList(List<EventShow> eventShows)
    {
        switch (eventShows[0].state)
        {
            case EVENTSTATE.Wait:
                eventShows[0].state = EVENTSTATE.Do;
                eventShows[0].thisevent.dealEffect(battleManager.battleInfoShow);
                break;
            case EVENTSTATE.Do:
                eventShows[0].upDateEvent();
                eventShows[0].state = EVENTSTATE.Over;
                break;
            case EVENTSTATE.Over:
                eventShows.Remove(eventShows[0]);
                break;
        }
    }
}
