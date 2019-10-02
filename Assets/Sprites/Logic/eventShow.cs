using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public EventShow(singleEvent _event)
    {
        thisevent = _event;
        state = EVENTSTATE.Wait;
        if (thisevent.b_haveChildEvent)
        {
            foreach (singleEvent childevent in thisevent.childEvents)
            {
                childEventShows.Add(new EventShow(childevent));
            }
        }
    }
    public bool upDateEvent()
    {
        switch (state)
        {
            case EVENTSTATE.Wait:
                break;
            case EVENTSTATE.Do:
                if (timecursor < 1)
                {
                    timecursor += Time.deltaTime / lasttime;
                    foreach (EventShow eventShow in childEventShows)
                    {
                        eventShow.upDateEvent();
                    }
                }
                else
                {
                    bool allchildover = true;
                    foreach (EventShow eventShow in childEventShows)
                    {
                        if (!eventShow.upDateEvent())
                        {
                            allchildover = false;
                        }
                    }
                    if (allchildover)
                    {
                        //to over
                        state = EVENTSTATE.Over;
                    }
                }
                break;
            case EVENTSTATE.Over:
                return true;
        }
        //子事件update
        if (thisevent.b_haveChildEvent)
        {
            foreach (EventShow eventShow in childEventShows)
            {
                eventShow.upDateEvent();
            }
        }
        return false;
    }


    public List<EventShow> childEventShows = new List<EventShow>();
    public float lasttime=0.5f;
    public float timecursor;
    public singleEvent thisevent;
    public EVENTSTATE state;
}