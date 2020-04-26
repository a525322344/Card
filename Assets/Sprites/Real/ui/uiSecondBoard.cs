using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uiSecondBoard : MonoBehaviour
{
    public toDo enterToDo = () => { };
    public toDo exitToDo = () => { };
    public secondBoardInfo thisSecondInfo;
    public virtual void EnterInit(secondBoardInfo secondInfo)
    {
        enterToDo += secondInfo.onEnter;
        exitToDo += secondInfo.onExit;
        enterToDo();
    }
    public virtual void Exit()
    {
        exitToDo();
    }
}
