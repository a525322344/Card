using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum SortState
{
    Free,
    Select,
    Install,
}
public class realpart : MonoBehaviour
{
    //实例化相关
    public GameObject realgridMode;
    public float distance = 0.333f;
    public Transform meshTran;
    public Transform gridFolder;
    public Text text;
    //战斗相关类
    public MagicPart thisMagicPart;
    public GameState realPartState;
    public SortState sortState;
    #region linkPart
    private MagicPart linkSavePart;
    public void enterLinkPart(MagicPart linkPart)
    {
        thisMagicPart = linkPart;
    }
    public void exitLinkPart()
    {
        thisMagicPart = linkSavePart;
    }
    #endregion
    private bool b_readyToPlayACard = false;
    private card selectedCard;
    //sort相关类
    public Transform freeFater;
    public realLatice lastRealLatice;
    public bool b_caninstall;
    public bool b_rotate;
    public Transform installPosiTran;
    public Dictionary<grid, realgrid> gridRealgridPairs = new Dictionary<grid, realgrid>();
    public bool b_ShowOutlineInMap;
    public float installpartZOffset;



    //用于创建时调用初始化
    public void Init(MagicPart magicPart,GameState state,Transform father)
    {
        freeFater = father;
        if (state == GameState.BattleSence)
        {
            b_ShowOutlineInMap = true;
            meshTran.gameObject.SetActive(false);
            realPartState = state;
            thisMagicPart = magicPart;
            linkSavePart = thisMagicPart;
            //根据magicpart中的grid表，创建realgird
            foreach (var g in thisMagicPart.Vector2GridPairs)
            {
                //创建realgril实例添加realgril
                GameObject realgridObject = Instantiate(realgridMode, gridFolder);
                realgrid newrealgrid = realgridObject.GetComponent<realgrid>();
                newrealgrid.Init(g.Value, this);
                newrealgrid.gridOutlineCS.Sides = OutlineSides(g.Key);
                realgridObject.GetComponent<Transform>().localPosition = new Vector3(g.Key.x, g.Key.y, 0) * distance;
                newrealgrid.changeMaterial();

                gridRealgridPairs.Add(g.Value, newrealgrid);
            }
            text.text = thisMagicPart.describe;
        }
        else if(state == GameState.MapSence)
        {
            realPartState = state;
            thisMagicPart = magicPart;
            //根据magicpart中的grid表，创建realgird
            foreach (var g in thisMagicPart.Vector2GridPairs)
            {
                //创建realgril实例添加realgril
                GameObject realgridObject = Instantiate(realgridMode, gridFolder);
                realgrid newrealgrid = realgridObject.GetComponent<realgrid>();
                newrealgrid.Init(g.Value, this);
                newrealgrid.gridOutlineCS.Sides = OutlineSides(g.Key);
                realgridObject.GetComponent<Transform>().localPosition = new Vector3(g.Key.x, g.Key.y, 0) * distance;
                newrealgrid.changeMaterial();

                gridRealgridPairs.Add(g.Value, newrealgrid);
            }
            text.text = thisMagicPart.describe;
        }
        RotateRealPart();
    }

    private List<realgrid> selectgrids = new List<realgrid>();
    //public bool CanCostPlay(grid bygrid,card selectcard)
    //{
    //    bool result=true;
    //    int gridx = (int)bygrid.position.x;
    //    int gridy = (int)bygrid.position.y;
    //    //遍历selectcard,找到有1的cost块
    //    for(int i = 0; i < 3; i++)
    //    {
    //        for(int j = 0; j < 3; j++)
    //        {
    //            //
    //            if (selectcard.costVector2[i, j] == 1)
    //            {
    //                int costx = j - 1;
    //                int costy = i - 1;
    //                int fitx = gridx + costx;
    //                int fity = gridy + costy;
    //                if (thisMagicPart.Vector2GridRotate.ContainsKey(new Vector2(fitx, fity)))
    //                {
    //                    grid fitgrid = thisMagicPart.Vector2GridRotate[new Vector2(fitx, fity)];
    //                    if (fitgrid.Opening && fitgrid.Power)
    //                    {
    //                        selectgrids.Add(gridRealgridPairs[fitgrid]);
    //                    }
    //                    else
    //                    {
    //                        result = false;
    //                    }
    //                }
    //                else
    //                {
    //                    result = false;
    //                }
    //            }
    //        }
    //    }
    //    if (!result)
    //    {
    //        selectgrids.Clear();
    //    }
    //    return result;
    //}

    //public void SetDownCard(card _selectcard)
    //{
    //    if (_selectcard == null)
    //    {
    //        for(int i = 0; i < selectgrids.Count; i++)
    //        {
    //            selectgrids[i].SetDownCard(null);
    //        }
    //        selectgrids.Clear();
    //        b_readyToPlayACard = false;
    //        selectedCard = null;
    //        //部件的激活与睡眠转移到了CardEvent中
    //        //thisMagicPart.sleepPart();

    //    }
    //    else
    //    {
    //        for (int i = 0; i < selectgrids.Count; i++)
    //        {
    //            selectgrids[i].SetDownCard(_selectcard);
    //        }
    //        b_readyToPlayACard = true;
    //        selectedCard = _selectcard;

    //        gameManager.Instance.battlemanager.SetSelectPart(this);
    //        //部件的激活与睡眠转移到了CardEvent中
    //        //thisMagicPart.activatePart();
           
    //    }
    //}
    //public void UseSelectGrids()
    //{
    //    for(int i = 0; i < selectgrids.Count; i++)
    //    {
    //        selectgrids[i].thisgrid.Power = false;
    //        selectgrids[i].changeMaterial();
    //    }
    //}
    public void PowerRealPart()
    {
        thisMagicPart.PowerAllGrid();
        foreach(var g in gridRealgridPairs)
        {
            g.Value.changeMaterial();
        }
    }

    public Vector4 OutlineSides(Vector2 posi)
    {
        float left = 1;
        if (thisMagicPart.Vector2GridPairs.ContainsKey(posi + Vector2.left))
        {
            if (thisMagicPart.Vector2GridPairs[posi + Vector2.left].Opening)
            {
                left = 0;
            }
        }
        float right = 1;
        if (thisMagicPart.Vector2GridPairs.ContainsKey(posi + Vector2.right))
        {
            if (thisMagicPart.Vector2GridPairs[posi + Vector2.right].Opening)
            {
                right = 0;
            }
        }
        float up = 1;
        if (thisMagicPart.Vector2GridPairs.ContainsKey(posi + Vector2.up))
        {
            if (thisMagicPart.Vector2GridPairs[posi + Vector2.up].Opening)
            {
                up = 0;
            }
        }
        float down = 1;
        if (thisMagicPart.Vector2GridPairs.ContainsKey(posi + Vector2.down))
        {
            if (thisMagicPart.Vector2GridPairs[posi + Vector2.down].Opening)
            {
                down = 0;
            }
        }
        return new Vector4(left, right, up, down);
    }

    // Update is called once per frame
    void Update()
    {
        switch (realPartState)
        {
            case GameState.BattleSence:
                break;
            case GameState.MapSence:
                switch (sortState)
                {
                    case SortState.Free:
                        break;
                    case SortState.Select:
                        //跟随鼠标部分
                        Vector3 mouseposition = Input.mousePosition;
                        mouseposition = Camera.main.ScreenToWorldPoint(mouseposition);
                        mouseposition=new Vector3(mouseposition.x,mouseposition.y, gameManager.Instance.instantiatemanager.mapRootInfo.sortPositionZ());
                        transform.DOMove(mouseposition , 0);
                        //旋转部分
                        if(Input.GetAxis("Mouse ScrollWheel") < 0)//下
                        {
                            thisMagicPart.RotatePart(-1);
                            RotateRealPart();
                            b_rotate = true;
                        }
                        if(Input.GetAxis("Mouse ScrollWheel") > 0)//上
                        {
                            thisMagicPart.RotatePart(1);
                            RotateRealPart();
                            b_rotate = true;
                        }
                        //映射位置部分
                        RaycastHit hit;
                        realLatice downRealLatice;
                        if (Physics.Raycast(transform.position, Vector3.forward, out hit, 100, 1 << 10))
                        {
                            downRealLatice = hit.transform.GetComponent<realLatice>();

                            if (downRealLatice != lastRealLatice||b_rotate)
                            {
                                b_rotate = false;
                                if (lastRealLatice)
                                {
                                    lastRealLatice.ExitCanInstall();
                                }
                                lastRealLatice = downRealLatice;
                                //如果可以安置
                                if (downRealLatice.CanInstallPart(thisMagicPart))
                                {
                                    b_caninstall = true;
                                }
                                else
                                {
                                    b_caninstall = false;
                                }
                            }
                        }
                        else
                        {
                            if (lastRealLatice)
                            {
                                b_caninstall = false;
                                lastRealLatice.ExitCanInstall();
                                lastRealLatice = null;
                            }
                        }
                        break;
                    case SortState.Install:
                        //莫名的，再次进入后，就不能一次触发了，临时在这里添加
                        transform.position = installPosiTran.position- new Vector3(0, 0, installpartZOffset);
                        transform.parent = installPosiTran.parent;
                        break;
                }
                break;
        }
    }

    public void RotateRealPart()
    {
        gridFolder.rotation = Quaternion.Euler(0,0, thisMagicPart.rotateInt * 90);
    }
    public void InitInstall(Transform tran)
    {
        sortState = SortState.Install;
        installPosiTran = tran;
        transform.position = installPosiTran.position-new Vector3(0,0, installpartZOffset);
        transform.parent = installPosiTran.parent;
    }
    public void OutlineShow(int mode)
    {
        foreach(var grg in gridRealgridPairs)
        {
            grg.Value.gridOutlineCS.lineMode = mode;
        }
    }

    private void OnMouseDown()
    {
        if (realPartState == GameState.MapSence)
        {
            switch (sortState)
            {
                case SortState.Free:
                    sortState = SortState.Select;
                    meshTran.gameObject.SetActive(false);
                    break;
                case SortState.Select:
                    break;
                case SortState.Install:
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + new Vector3(0, 0, -5), Vector3.forward, out hit, 100, 1 << 10))
                    {
                        hit.transform.GetComponent<realLatice>().ExitInstall();
                        transform.parent = freeFater;
                        sortState = SortState.Select;
                    }
                    else
                    {
                        Debug.Log("没有检测到latice");
                    }
                    break;
            }
        }
    }

    private void OnMouseUp()
    {
        if (realPartState == GameState.MapSence)
        {
            switch (sortState)
            {
                case SortState.Free:
                    break;
                case SortState.Select:
                    if (b_caninstall)
                    {
                        //install
                        lastRealLatice.InstallPart(thisMagicPart, out installPosiTran);
                        transform.position = installPosiTran.position;
                        transform.parent = installPosiTran.parent;
                        sortState = SortState.Install;
                    }
                    else
                    {
                        sortState = SortState.Free;
                        meshTran.gameObject.SetActive(true);
                    }
                    break;
                case SortState.Install:

                    break;
            }
        }
    }
}
