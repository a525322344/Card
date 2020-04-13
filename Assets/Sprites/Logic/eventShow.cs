using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void deleToDo();
public enum EVENTSTATE
{
    Wait,
    Do,
    Stop,
    Over,
}
public abstract class ShowAbstract
{
    public float lastTime;
    public float timeCursor;
}
//事件演示动画类
[System.Serializable]
public class EventShow
{
    public string name;
    public EventShow(singleEvent _event,List<EventShow> belongList)
    {
        thisevent = _event;
        belongEventShows = belongList;
        state = EVENTSTATE.Wait;
        name = _event.eventDescribe;

        StartToDo = new deleToDo(() => { });
        EndToDo = new deleToDo(() => { });

        if (_event.m_eventKind == EventKind.Event_PlayCard)
        {
            CardEvent cardevent = _event as CardEvent;
            lasttime = cardevent.alltime;
            performList = cardevent.performList;
            turn = 0;
        }

    }
    public bool upDateEvent(battleInfo battleinfo)
    {
        switch (state)
        {
            case EVENTSTATE.Wait:
                //Debug.Log("wait");
                StartToDo();
                
                if (thisevent.isStopEffect())
                {
                    //要去控制总输入暂时停止
                    state = EVENTSTATE.Stop;
                }
                else
                {
                    //Debug.Log("do");
                    state = EVENTSTATE.Do;
                }
                break;
            case EVENTSTATE.Stop:
                //如果达成停顿条件，结束Stop状态
                //Debug.Log("wait");
                if (thisevent.m_effect.JudgeWhether(battleinfo))
                {
                    //Debug.Log("do");
                    battleinfo.realWaitSelectCard.OnComplete();
                    state = EVENTSTATE.Do;
                }
                break;
            case EVENTSTATE.Do:
                if (timecursor < 1)
                {
                    timecursor += Time.deltaTime / lasttime;
                    if (turn + 1 <= performList.Count)
                    {
                        if(timecursor > performList[turn].timeTurn)
                        {
                            performList[turn].Play();
                            turn++;
                        }
                    }
                }
                else
                {
                    state = EVENTSTATE.Over;
                }
                break;
            case EVENTSTATE.Over:
               // Debug.Log("end");
                EndToDo();
                return true;
        }
        return false;
    }

    public deleToDo StartToDo;
    public deleToDo EndToDo;
    public float lasttime=0.1f;
    public float timecursor;
    public singleEvent thisevent;
    private List<EventShow> belongEventShows = new List<EventShow>();
    private int turn;
    private List<perform> performList = new List<perform>();
    public EVENTSTATE state;
}