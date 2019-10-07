using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void deleToDo();
public enum EVENTSTATE
{
    Wait,
    Do,
    Over
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
    public EventShow(singleEvent _event,List<EventShow> belongList)
    {
        thisevent = _event;
        belongEventShows = belongList;
        state = EVENTSTATE.Wait;

        //if (thisevent.b_haveChildEvent)
        //{
        //    foreach (singleEvent childevent in thisevent.childEvents)
        //    {
        //        childevent.b_logoutAfterDeal = false;
        //        belongEventShows.Add(new EventShow(childevent,belongEventShows));
        //    }
        //}
        StartToDo = new deleToDo(() => { });
        EndToDo = new deleToDo(() => { });
    }
    public bool upDateEvent()
    {
        switch (state)
        {
            case EVENTSTATE.Wait:
                StartToDo();
                state = EVENTSTATE.Do;
                break;
            case EVENTSTATE.Do:
                if (timecursor < 1)
                {
                    timecursor += Time.deltaTime / lasttime;
                }
                else
                {
                    state = EVENTSTATE.Over;
                }
                break;
            case EVENTSTATE.Over:
                EndToDo();
                return true;
        }
        return false;
    }

    public deleToDo StartToDo;
    public deleToDo EndToDo;
    public float lasttime=5;
    public float timecursor;
    public singleEvent thisevent;
    private List<EventShow> belongEventShows = new List<EventShow>();
    public EVENTSTATE state;
}