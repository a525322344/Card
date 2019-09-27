using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constant;

public class realCost : MonoBehaviour
{
    public Transform[] costchildrens = new Transform[9];
    public card thiscard;

    //  costMode
    //1:当前位置不合适安放cost
    //2:当前位置合适
    public int costMode = 0;
    private int nextCostMode = 0;

    private List<MeshRenderer> _renderers = new List<MeshRenderer>();
    private Material mr_cyan;
    private Material mr_blue;

    public realgrid lastrealgrid;
    //public Transform lastrealgridTran=null;
    public void Update()
    {
        //射线检测当前选中的是哪格
        RaycastHit hit;
        realgrid downRealgrid; 
        if(Physics.Raycast(transform.position, Vector3.forward,out hit, 100, 1 << 9)){
            downRealgrid = hit.transform.GetComponent<realgrid>();
            //判断是否切换格子，如若切换，reset last readgrid
            if (lastrealgrid)
            {
                if (lastrealgrid != downRealgrid)
                {
                    lastrealgrid.ToSetPart(null);
                    lastrealgrid = null;
                }
            }
            //如若没有过改变，则不用再次检测
            if (downRealgrid != lastrealgrid)
            {
                if (downRealgrid.CanOverCostPlay(thiscard))
                {
                    nextCostMode = 2;
                    lastrealgrid = downRealgrid;
                    downRealgrid.ToSetPart(thiscard);
                }
                else
                {
                    nextCostMode = 1;
                    if (lastrealgrid)
                    {
                        lastrealgrid.ToSetPart(null);
                        lastrealgrid = null;
                    }
                }
            }
        }

        //根据costMode进行切换
        if (costMode != nextCostMode)
        {
            costMode = nextCostMode;
            switch (nextCostMode)
            {
                case 1:
                    for(int i=0;i<_renderers.Count;i++)
                    {
                        _renderers[i].material = mr_blue;
                    }                 
                    break;
                case 2:
                    for (int i = 0; i < _renderers.Count; i++)
                    {
                        _renderers[i].material = mr_cyan;
                    }
                    break;
            }
        }
    }

    //初始化调用
    public void setCost(card _playercard)
    {
        costMode = 1;
        nextCostMode = 1;
        for (int i = 0; i < 9; i++)
        {
            costchildrens[i] = transform.GetChild(i);

        }
        thiscard = _playercard;
        foreach (Transform costchild in costchildrens)
        {
            costchild.gameObject.SetActive(false);
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (thiscard.costVector2[i, j] == 0)
                {
                    costchildrens[i * 3 + j].gameObject.SetActive(false);
                }
                else if(thiscard.costVector2[i, j] == 1)
                {
                    costchildrens[i * 3 + j].gameObject.SetActive(true);
                    _renderers.Add(costchildrens[i * 3 + j].GetComponent<MeshRenderer>());
                }
            }
        }
    }
    private void Start()
    {
        mr_cyan = Resources.Load<Material>(Path.MATERIAL_COST_CYAN);
        mr_blue = Resources.Load<Material>(Path.MATERIAL_COST_BLUE);
    }
}
