﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//准备做realWaitSelectBoard的基类
public class realWaitSelectAB : MonoBehaviour
{
    protected bool b_finishSelect = false;
    protected int selectint = -1;

    public virtual bool IsFinishSelect()
    {
        return b_finishSelect;
    }
    public virtual void OnComplete() { }
    public virtual bool SelectOne(int num,out Selection select)
    {
        selectint = num;
        select = null;
        return false;
    }
}