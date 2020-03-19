using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public enum RealPartState
{
    Play,
    Sort,
}
public enum SortState
{
    Free,
    Select,
    Install,
}
public class realpart : MonoBehaviour
{
    public MagicPart thisMagicPart;
    public RealPartState realPartState;
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
    public GameObject realgridMode;
    public float distance = 0.333f;

    public Transform meshTran;
    public Text text;

    private Dictionary<grid, realgrid> gridRealgridPairs = new Dictionary<grid, realgrid>();

    private bool b_readyToPlayACard = false;
    private card selectedCard;

    //Sort
    public SortState sortState;

    //用于创建时调用初始化
    public void Init(MagicPart magicPart,RealPartState state)
    {
        if (state == RealPartState.Play)
        {
            realPartState = state;
            thisMagicPart = magicPart;
            linkSavePart = thisMagicPart;
            //根据magicpart中的grid表，创建realgird
            foreach (var g in thisMagicPart.Vector2GridRotate)
            {
                //创建realgril实例添加realgril
                GameObject realgridObject = Instantiate(realgridMode, transform);
                realgrid newrealgrid = realgridObject.GetComponent<realgrid>();
                newrealgrid.Init(g.Value, this);
                realgridObject.GetComponent<Transform>().localPosition = g.Value.getPosition() * distance;


                newrealgrid.changeMaterial();

                gridRealgridPairs.Add(g.Value, newrealgrid);
            }
            text.text = thisMagicPart.describe;
        }
        else if(state == RealPartState.Sort)
        {
            realPartState = state;
            thisMagicPart = magicPart;
            //根据magicpart中的grid表，创建realgird
            foreach (var g in thisMagicPart.Vector2GridRotate)
            {
                //创建realgril实例添加realgril
                GameObject realgridObject = Instantiate(realgridMode, transform);
                realgrid newrealgrid = realgridObject.GetComponent<realgrid>();
                newrealgrid.Init(g.Value, this);
                realgridObject.GetComponent<Transform>().localPosition = g.Value.getPosition() * distance;
                newrealgrid.changeMaterial();

                gridRealgridPairs.Add(g.Value, newrealgrid);
            }
            text.text = thisMagicPart.describe;
        }

    }

    private List<realgrid> selectgrids = new List<realgrid>();
    public bool CanCostPlay(grid bygrid,card selectcard)
    {
        bool result=true;
        int gridx = bygrid.PositionX;
        int gridy = bygrid.PositionY;
        //遍历selectcard,找到有1的cost块
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                //
                if (selectcard.costVector2[i, j] == 1)
                {
                    int costx = j - 1;
                    int costy = i - 1;
                    int fitx = gridx + costx;
                    int fity = gridy + costy;
                    if (thisMagicPart.Vector2GridRotate.ContainsKey(new Vector2(fitx, fity)))
                    {
                        grid fitgrid = thisMagicPart.Vector2GridRotate[new Vector2(fitx, fity)];
                        if (fitgrid.Opening && fitgrid.Power)
                        {
                            selectgrids.Add(gridRealgridPairs[fitgrid]);
                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
        }
        if (!result)
        {
            selectgrids.Clear();
        }
        return result;
    }
    public void SetDownCard(card _selectcard)
    {
        if (_selectcard == null)
        {
            for(int i = 0; i < selectgrids.Count; i++)
            {
                selectgrids[i].SetDownCard(null);
            }
            selectgrids.Clear();
            b_readyToPlayACard = false;
            selectedCard = null;
            //部件的激活与睡眠转移到了CardEvent中
            //thisMagicPart.sleepPart();

        }
        else
        {
            for (int i = 0; i < selectgrids.Count; i++)
            {
                selectgrids[i].SetDownCard(_selectcard);
            }
            b_readyToPlayACard = true;
            selectedCard = _selectcard;

            gameManager.Instance.battlemanager.SetSelectPart(this);
            //部件的激活与睡眠转移到了CardEvent中
            //thisMagicPart.activatePart();
           
        }
    }
    public void UseSelectGrids()
    {
        for(int i = 0; i < selectgrids.Count; i++)
        {
            selectgrids[i].thisgrid.Power = false;
            selectgrids[i].changeMaterial();
        }
    }
    public void PowerRealPart()
    {
        thisMagicPart.PowerAllGrid();
        foreach(var g in gridRealgridPairs)
        {
            g.Value.changeMaterial();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (realPartState)
        {
            case RealPartState.Play:
                if (b_readyToPlayACard)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        b_readyToPlayACard = false;
                        gameManager.Instance.battlemanager.PlayCard();
                    }
                }
                break;
            case RealPartState.Sort:
                switch (sortState)
                {
                    case SortState.Free:
                        break;
                    case SortState.Select:
                        Vector3 mouseposition = Input.mousePosition;
                        mouseposition = Camera.main.ScreenToWorldPoint(mouseposition);
                        mouseposition=new Vector3(mouseposition.x,mouseposition.y, gameManager.Instance.instantiatemanager.mapRootInfo.sortPositionZ());
                        Debug.Log(mouseposition);
                        transform.DOMove(mouseposition , 0);
                        if(Input.GetAxis("Mouse ScrollWheel") < 0)
                        {
                            Debug.Log("<0");
                        }
                        if(Input.GetAxis("Mouse ScrollWhell") > 0)
                        {
                            Debug.Log(">0");
                        }
                        break;
                    case SortState.Install:
                        break;
                }
                break;
        }
    }

    private void OnMouseDown()
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
                sortState = SortState.Select;
                break;
        }
    }
    private void OnMouseUp()
    {
        switch (sortState)
        {
            case SortState.Free:        
                break;
            case SortState.Select:
                sortState = SortState.Free;
                meshTran.gameObject.SetActive(true);
                break;
            case SortState.Install:
                
                break;
        }
    }
}
