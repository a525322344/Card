using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection:IComparable
{
    public Selection(Transform pt)
    {
        b_isNull = true;
        PositionTran = pt;
        saveCardnum = -1;
    }
    public bool b_isNull;           //判断这个选择位置是否已经选择
    public Transform PositionTran;  //储存这个位置
    public int saveCardnum;         //选择的卡序号

    public int CompareTo(object obj)
    {
        Selection s = obj as Selection;
        return this.saveCardnum.CompareTo(s.saveCardnum);
    }
}

public class realWaitSelectBoard : realWaitSelectAB
{
    public int waitSelectNum;
    public singleEvent mainEvent;

    public float distance;
    public GameObject CardPointGB;
    public Transform board;
    public Transform cardPosition;

    public List<Selection> PointSelections = new List<Selection>();

    public void InitWaitSelectCard(int num,singleEvent e)
    {
        PointSelections.Clear();
        mainEvent = e;
        waitSelectNum = num;
        board.localScale = new Vector3(board.localScale.x * (0.125f + 0.875f * num), board.localScale.y, board.localScale.z);

        float initnum =(float) -(num - 1) / 2;
        for(int i = 0; i < num; i++)
        {
            GameObject newcardpoint = Instantiate(CardPointGB, cardPosition);
            Transform newtran = newcardpoint.GetComponent<Transform>();
            Selection newselect = new Selection(newtran);
            newtran.localPosition = new Vector3((initnum + i) * distance, 0, 0);
            PointSelections.Add(newselect);
        }
    }
    public override bool IsFinishSelect()
    {
        bool result = true;
        foreach(Selection st in PointSelections)
        {
            if (st.b_isNull)
            {
                result = false;
            }
        }

        return result;
    }
    public override void OnComplete()
    {
        //将手牌状态恢复
        foreach (realCard rc in gameManager.Instance.battlemanager.realCardList)
        {
            rc.ExitStateWaitSelect();
        }
        //将选卡框删掉
        gameManager.Instance.instantiatemanager.waitSelectBoard.SetActive(false);
        //给子事件传递信息
        PointSelections.Sort();
        for(int i = 0; i < PointSelections.Count; i++)
        {
            mainEvent.childEvents[0].m_effect.mixnum = PointSelections[PointSelections.Count-1-i].saveCardnum;
        }
    }
    public override bool SelectOne(int num,out Selection select)
    {
        bool result = false;
        select = null;
        foreach(Selection st in PointSelections)
        {
            if (st.b_isNull)
            {
                st.b_isNull = false;
                st.saveCardnum = num;
                select = st;

                result = true;
                break;
            }
        }
        return result;
    }
}
