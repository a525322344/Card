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

    public battleUIRoot battleuiRoot;
    public BattleEnvRoot battleEnvRoot;
    public MapRootInfo mapRootInfo;

    public Canvas uiCanvas;
    public GameObject partGO;
    public GameObject cardGO;
    public GameObject gridGO;
    public GameObject[] costs;
    public GameObject actionAttack;
    public GameObject actionDefense;
    public GameObject fireState;
    public List<GameObject> placeGOs = new List<GameObject>();
    public List<GameObject> MonsterAll = new List<GameObject>();

    //弃卡选择框
    public GameObject waitSelectBoardGO;
    public GameObject waitSelectBoard;

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
            realpart.setThisMagicPart(magicParts[i]);
            battleuiRoot.parttransforms[0].parent.GetComponent<bookFolderControll>().realparts.Add(realpart);
        }
        gameManager.Instance.battlemanager.realPartList = battleuiRoot.parttransforms[0].parent.GetComponent<bookFolderControll>().realparts;
    }

    public void instanDrawACard(card playercard)
    {
        GameObject card = Instantiate(cardGO, battleuiRoot.handCardControll);
        realCard realcard = card.GetComponentInChildren<realCard>();
        realcard.SetThiscard(playercard,RealCardState.RealCard);
        realcard.ShowDraw();
        battleuiRoot.handCardControll.GetComponent<handcardControll>().playerHandCards.Add(realcard);
    }


    public void instanMonster(monsterInfo moninfo,out realEnemy realEnemy)
    {
        GameObject monster = Instantiate(MonsterAll[moninfo.Id], battleEnvRoot.monsterPosi);
        realEnemy realenemy = monster.GetComponent<realEnemy>();
        realenemy.Init(moninfo);
        realEnemy = realenemy;
    }

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
