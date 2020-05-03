using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constant;

public class realCost : MonoBehaviour
{
    #region 外部引用
    public Material mr_cyan;
    public Material mr_blue;
    private Transform[] costchildrens = new Transform[10];
    #endregion
    private List<MeshRenderer> _renderers = new List<MeshRenderer>();

    public card thiscard;
    public Dictionary<Vector2, int> initVecInt;
    public Dictionary<Vector2, int> VecIntRotate;
    public int rotateInt;
    //  costMode
    //1:当前位置不合适安放cost
    //2:当前位置合适
    public bool b_rotate;
    public int costMode = 0;
    private int nextCostMode = 0;

    ///public realgrid lastrealgrid;
    public realLatice lastRealLatice;

    public void Update()
    {
        //射线检测当前选中的是哪格
        RaycastHit hit;
        ///realgrid downRealgrid;
        realLatice nowRealLatice;
        
        if(Physics.Raycast(transform.position, Vector3.forward,out hit, 100, 1 << 10)){
            nowRealLatice = hit.transform.GetComponent<realLatice>();
            //判断是否切换格子，如若切换，reset last readgrid
            if (lastRealLatice)
            {
                if (lastRealLatice != nowRealLatice)
                {
                    lastRealLatice.ToSetPart(null);
                    lastRealLatice = null;
                    gameManager.Instance.battlemanager.setCardDescribe(new MagicPart());
                }
            }
            //如若没有过改变，则不用再次检测
            if (nowRealLatice != lastRealLatice||b_rotate)
            {
                b_rotate = false;
                if (nowRealLatice.CanCostPlay(VecIntRotate))
                {
                    //Debug.Log("canovercostPlay");
                    nextCostMode = 2;
                    lastRealLatice = nowRealLatice;
                    nowRealLatice.ToSetPart(thiscard);

                    if (nowRealLatice.realknapsack.selectPart == null)
                    {
                        Debug.Log("selectpart==null");
                    }
                    else
                    {
                        gameManager.Instance.battlemanager.setCardDescribe(nowRealLatice.realknapsack.selectPart);
                    }
                }
                else
                {
                    nextCostMode = 1;
                    if (lastRealLatice)
                    {
                        lastRealLatice.ToSetPart(null);
                        gameManager.Instance.battlemanager.setCardDescribe(new MagicPart());
                        lastRealLatice = null;
                    }
                }
            }
        }
        else
        {
            if (lastRealLatice)
            {
                lastRealLatice.ToSetPart(null);
                lastRealLatice = null;
                gameManager.Instance.battlemanager.setCardDescribe(new MagicPart());
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
    public void Init(card _playercard)
    {
        initVecInt = _playercard.vecCostPairs;
        VecIntRotate = new Dictionary<Vector2, int>(initVecInt);
        thiscard = _playercard;

        costMode = 1;
        nextCostMode = 1;

        for (int i = 0; i < 10; i++)
        {
            costchildrens[i] = transform.GetChild(i);
        }
        if (thiscard.Cost == 0)
        {
            foreach (var vecint in initVecInt)
            {
                int order = (int)vecint.Key.x + (int)vecint.Key.y * 3 + 4;
                costchildrens[order].gameObject.SetActive(false);
            }
            costchildrens[9].gameObject.SetActive(true);
            _renderers.Add(costchildrens[9].GetComponent<MeshRenderer>());
        }
        else
        {
            costchildrens[9].gameObject.SetActive(false);
            foreach (var vecint in initVecInt)
            {
                int order = (int)vecint.Key.x + (int)vecint.Key.y * 3 + 4;
                if (vecint.Value == 1)
                {
                    costchildrens[order].gameObject.SetActive(true);
                    _renderers.Add(costchildrens[order].GetComponent<MeshRenderer>());
                }
                else if (vecint.Value == 0)
                {
                    costchildrens[order].gameObject.SetActive(false);
                }
            }
        }
    }

    public void RotateCost(int r)
    {
        if (r == 1)
        {
            if (rotateInt == 3)
            {
                rotateInt = 0;
            }
            else
            {
                rotateInt++;
            }
        }
        else if (r == -1)
        {
            if (rotateInt == 0)
            {
                rotateInt = 3;
            }
            else
            {
                rotateInt--;
            }
        }
        VecIntRotate.Clear();
        foreach(var vg in initVecInt)
        {
            float angle = (float)rotateInt * 90 * Mathf.Deg2Rad;
            Vector2 newvec=new Vector2(
                (int)Mathf.Cos(angle) * vg.Key.x - (int)Mathf.Sin(angle) * vg.Key.y,
                (int)Mathf.Sin(angle) * vg.Key.x + (int)Mathf.Cos(angle) * vg.Key.y);
            VecIntRotate.Add(newvec, vg.Value);
        }
        transform.rotation = Quaternion.Euler(0, 0, rotateInt * 90);
        b_rotate = true;
    }
    private void Start()
    {
        mr_cyan = Resources.Load<Material>(Path.MATERIAL_COST_CYAN);
        mr_blue = Resources.Load<Material>(Path.MATERIAL_COST_BLUE);
    }
}
