using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class uiSecondBoard : MonoBehaviour
{
    public toDo enterToDo = () => { };
    public toDo exitToDo = () => { };
    public secondBoardInfo thisSecondInfo;
    public float downmove=20;
    public float moveUpTime = 0.2f;
    public virtual void EnterInit(secondBoardInfo secondInfo)
    {
        MoveUp();
        enterToDo += secondInfo.onEnter;
        exitToDo += secondInfo.onExit;
        enterToDo();
    }
    public virtual void Exit()
    {
        exitToDo();
    }

    public virtual void MoveUp()
    {
        transform.localPosition = new Vector3(0, -downmove, transform.localPosition.z);
        transform.DOLocalMoveY(0, moveUpTime);
    }
}
