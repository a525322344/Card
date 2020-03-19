using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateManager : MonoBehaviour
{
    public static instantiateManager instance
    {
        get
        {
            return _instance;
        }
    }
    private static instantiateManager _instance;
    #region 实时获得隐藏
    [HideInInspector]
    public battleUIRoot battleuiRoot;
    [HideInInspector]
    public BattleEnvRoot battleEnvRoot;
    [HideInInspector]
    public MapRootInfo mapRootInfo;

    //储存对象
    [HideInInspector]
    public GameObject waitSelectBoard;
    [HideInInspector]
    public GameObject knapsack;
    #endregion

    public Canvas uiCanvas;
    public GameObject knapsackGO;
    public GameObject partGO;
    public GameObject cardGO;
    public GameObject gridGO;
    public GameObject[] costs;
    public GameObject actionAttack;
    public GameObject actionDefense;
    public GameObject fireState;

    public List<GameObject> MonsterAll = new List<GameObject>();
    //map
    public GameObject placeGO;
    public List<Sprite> beFallSprites = new List<Sprite>();
    public List<Sprite> mapPlaceSprites = new List<Sprite>();

    //弃卡选择框
    public GameObject waitSelectBoardGO;


    public Camera Encamera;
    public Camera UIcamera;

    private void Awake()
    {
        _instance = GetComponent<instantiateManager>();
    }


    public void instanBattleStartPart(List<MagicPart> magicParts)
    {
        for(int i=0;i<magicParts.Count;i++)
        {
            GameObject part = Instantiate(partGO, battleuiRoot.parttransforms[i]);
            realpart realpart = part.GetComponent<realpart>();
            realpart.Init(magicParts[i],RealPartState.Play);
            battleuiRoot.parttransforms[0].parent.GetComponent<bookFolderControll>().realparts.Add(realpart);
        }
        gameManager.Instance.battlemanager.realPartList = battleuiRoot.parttransforms[0].parent.GetComponent<bookFolderControll>().realparts;
    }
    //地图遭遇——生成整理背包页面
    public void instanSortPart(List<MagicPart> magicParts)
    {
        //把部件生成
        for(int i = 0; i < magicParts.Count; i++)
        {
            GameObject part = Instantiate(partGO, mapRootInfo.sortPartPosition);
            part.transform.localPosition = Vector3.down * mapRootInfo.sortPartDistance * i;
            realpart rp = part.GetComponent<realpart>();
            rp.Init(magicParts[i],RealPartState.Sort);
        }
        //把背包生成
        if (knapsack)
        {

        }
        else
        {
            knapsack = Instantiate(knapsackGO, mapRootInfo.knapsackPosition);
            realKnapsack rk = knapsack.GetComponent<realKnapsack>();
            rk.Init(gameManager.Instance.InitControllBoard.knapsackLaticInit);
        }
    }

    public void instanDrawACard(card playercard)
    {
        GameObject card = Instantiate(cardGO, battleuiRoot.handCardControll);
        realCard realcard = card.GetComponentInChildren<realCard>();
        realcard.SetThiscard(playercard,RealCardState.RealCard);
        realcard.ShowDraw();
        battleuiRoot.handCardControll.GetComponent<handcardControll>().playerHandCards.Add(realcard);
    }

    //战斗——生成怪物
    public void instanMonster(monsterInfo moninfo,out realEnemy realEnemy)
    {
        GameObject monster = Instantiate(MonsterAll[moninfo.Id], battleEnvRoot.monsterPosi);
        realEnemy realenemy = monster.GetComponent<realEnemy>();
        realenemy.Init(moninfo);
        realEnemy = realenemy;
    }
    //战斗——生成弃牌框
    public void instanWaitSelectCardBoard(int num,singleEvent single)
    {
        if (waitSelectBoard)
        {
            waitSelectBoard.SetActive(true);
            waitSelectBoard.GetComponent<realWaitSelectBoard>().InitWaitSelectCard(num, single);
        }
        else
        {
            waitSelectBoard = Instantiate(waitSelectBoardGO, battleuiRoot.waitSelectCard);
            realWaitSelectBoard realWSB = waitSelectBoard.GetComponent<realWaitSelectBoard>();
            gameManager.Instance.battlemanager.battleInfo.realWaitSelectCard = realWSB;
            realWSB.InitWaitSelectCard(num, single);
        }
    }
}
