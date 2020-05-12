using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public enum RealPartState
{
    Battle,
    Sort,
    Select,
}
public enum SortState
{
    Free,
    Enter,
    Select,
    Install,
    Other,
}
public class realpart : MonoBehaviour
{
    //实例化相关
    public GameObject realgridMode;
    public float distance = 0.333f;
    public Transform meshTran;
    public Transform jiaotiaoTran;
    public Transform gridFolder;
    public Transform saleposi;
    public TextMeshPro descritetext;
    //战斗相关类
    public MagicPart thisMagicPart;
    //public GameState realPartState;
    public RealPartState realPartState;
    public SortState sortState;
    #region linkPart
    private MagicPart linkSavePart;
    public List<realpart> linkrealParts = new List<realpart>();
    public void enterLinkPart(MagicPart linkPart)
    {
        thisMagicPart = linkPart;
    }
    public void exitLinkPart()
    {
        thisMagicPart = linkSavePart;
        linkrealParts.Clear();
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
    private float initpositionZ;
    private float initpositionmeshZ;
    //select
    public thingToSelect<MagicPart> partToSelect;

    //用于创建时调用初始化
    public void Init(MagicPart magicPart, RealPartState state,Transform father=null)
    {
        freeFater = father;
        realPartState = state;
        thisMagicPart = magicPart;
        thisMagicPart.realpart = this;
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
        descritetext.text = thisMagicPart.partDescribe();

        if (state == RealPartState.Battle)
        {
            b_ShowOutlineInMap = true;
            meshTran.gameObject.SetActive(false);
            jiaotiaoTran.gameObject.SetActive(false);
            sortState = SortState.Free;
            linkSavePart = thisMagicPart;
            GetComponent<BoxCollider>().enabled = false;
        }
        else if(state == RealPartState.Sort)
        {
            initpositionZ=transform.position.z;
            initpositionmeshZ = meshTran.position.z;
            meshTran.gameObject.SetActive(false);
            jiaotiaoTran.gameObject.SetActive(false);
            meshTran.GetChild(0).gameObject.SetActive(false);
            //GetComponent<BoxCollider>().size = new Vector3(100, 100, 2);
            //GetComponent<BoxCollider>().center = new Vector3(0, 0, 0);
            GetComponent<BoxCollider>().enabled = false;
        }
        else if (state == RealPartState.Select)
        {
            b_ShowOutlineInMap = false;
            sortState = SortState.Free;
            GetComponent<BoxCollider>().size = new Vector3(300, 350, 2);
            GetComponent<BoxCollider>().center = new Vector3(0, -100, 0);
        }
        RotateRealPart();
    }

    private List<realgrid> selectgrids = new List<realgrid>();
    public void PowerRealPart()
    {
        thisMagicPart.PowerAllGrid();
        foreach(var g in gridRealgridPairs)
        {
            g.Value.changeMaterial();
        }
    }
    //充能效果
    public void AllUsed()
    {
        bool isallused=true;
        int power = 0;
        foreach(var g in gridRealgridPairs)
        {
            if (g.Value.gridState != GridState.Used && g.Value.gridState != GridState.NotActive)
            {
                isallused = false;
            }
            if (g.Value.gridState == GridState.Power | g.Value.gridState == GridState.Can)
            {
                power++;
            }
        }
        Debug.Log(power);
        thisMagicPart.gridpower = power;
        if (isallused)
        {
            Debug.Log("充能：");
            foreach (singleEvent evnet in thisMagicPart.completeEvents)
            {
                EventShow neweventshow = new EventShow(evnet, gameManager.Instance.battlemanager.eventManager.BattleEventShows);
                gameManager.Instance.battlemanager.eventManager.BattleEventShows.Add(neweventshow);
            }
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
            case RealPartState.Battle:

                break;
            case RealPartState.Sort:
                switch (sortState)
                {
                    case SortState.Free:
                        break;
                    case SortState.Select:
                        //跟随鼠标部分
                        Vector3 mouseposition1 = Input.mousePosition;
                        mouseposition1 = Camera.main.ScreenToWorldPoint(mouseposition1);
                        mouseposition1=new Vector3(mouseposition1.x,mouseposition1.y, initpositionZ);
                        transform.DOMove(mouseposition1 , 0);
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
            case RealPartState.Select:
                switch (sortState)
                {
                    case SortState.Free:
                        transform.DOScale(Vector3.one, 0.1f);
                        break;
                    case SortState.Enter:
                        transform.DOScale(Vector3.one*1.1f, 0.1f);
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
        foreach(realpart realpart in linkrealParts)
        {
            foreach (var grg in realpart.gridRealgridPairs)
            {
                grg.Value.gridOutlineCS.lineMode = mode;
            }
        }
    }

    public void OnMouseDown()
    {
        if (realPartState == RealPartState.Sort)
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
        else if (realPartState == RealPartState.Select)
        {
            switch (sortState)
            {
                case SortState.Free:
                case SortState.Enter:
                    partToSelect.onSelectcard(thisMagicPart);
                    break;
            }
        }
    }
    public void OnMouseEnter()
    {
        if (realPartState == RealPartState.Select)
        {
            switch (sortState)
            {
                case SortState.Free:
                    sortState = SortState.Enter;
                    break;
            }
        }
        if (realPartState == RealPartState.Sort)
        {
            switch (sortState)
            {
                case SortState.Free:
                    break;
                case SortState.Install:
                    meshTran.gameObject.SetActive(true);
                    break;
            }
        }
        if (realPartState == RealPartState.Battle)
        {
            switch (sortState)
            {
                case SortState.Free:
                case SortState.Enter:
                    sortState = SortState.Enter;
                    Vector3 mouseposition = Input.mousePosition;
                    mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(mouseposition.x, mouseposition.y, instantiateManager.instance.battleuiRoot.uiCanvas.planeDistance));
                    meshTran.DOMove(mouseposition + Vector3.back * 1, 0);
                    meshTran.gameObject.SetActive(true);
                    break;
            }
        }
    }
    public void OnMouseExit()
    {
        if (realPartState == RealPartState.Select)
        {
            switch (sortState)
            {
                case SortState.Free:
                case SortState.Enter:
                    sortState = SortState.Free;
                    break;
            }
        }
        if (realPartState == RealPartState.Sort)
        {
            switch (sortState)
            {
                case SortState.Free:
                    break;
                case SortState.Install:
                    meshTran.gameObject.SetActive(false);
                    break;
            }
        }
        if (realPartState == RealPartState.Battle)
        {
            switch (sortState)
            {
                case SortState.Free:
                case SortState.Enter:
                    sortState = SortState.Free;
                    meshTran.gameObject.SetActive(false);
                    break;
            }
        }
    }
    public void OnMouseUp()
    {
        if (realPartState == RealPartState.Sort)
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
