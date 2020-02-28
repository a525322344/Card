using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection
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

    public void InitWaitSelectCard(int num)
    {
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
    public override void selectOne(int num,out Selection select)
    {
        base.selectOne(num, out select);
        foreach(Selection st in PointSelections)
        {
            if (st.b_isNull)
            {
                st.b_isNull = false;
                st.saveCardnum = num;
                select = st;
            }
        }
    }
}
