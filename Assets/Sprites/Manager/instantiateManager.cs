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

    #endregion

    public GameObject knapsackGO;
    public GameObject partGO;
    public GameObject cardGO;
    public GameObject gridGO;
    public GameObject[] costs;
    public GameObject actionAttack;
    public GameObject actionDefense;

    public List<GameObject> MonsterAll = new List<GameObject>();
    public List<Sprite> monsterHeadList = new List<Sprite>();
    //map
    public GameObject placeGO;
    public GameObject loadGO;
    public GameObject shopUiGO;
    public List<GameObject> uiSecondBoardGOList;
    public List<Sprite> beFallSprites = new List<Sprite>();
    public List<Sprite> mapPlaceSprites = new List<Sprite>();
    public List<Sprite> mapPlaceDiSprites = new List<Sprite>();
    public List<Sprite> cardSprites = new List<Sprite>();
    public List<Sprite> stateSprites = new List<Sprite>();

    //弃卡选择框
    public GameObject waitSelectBoardGO;

    private void Awake()
    {
        _instance = GetComponent<instantiateManager>();
    }



    //地图遭遇——生成整理背包页面(功能下放)
    List<realpart> realparts = new List<realpart>();
    [HideInInspector]
    public GameObject knapscak;
    public void instanSortPart(List<MagicPart> magicParts,knapsack _knapsack)
    {
        //外部生成还没有安装的部件
        int j = 0;
        for(int i = 0; i < magicParts.Count; i++)
        {
            if (!_knapsack.installParts.ContainsValue(magicParts[i]))
            {
                GameObject part = Instantiate(partGO, mapRootInfo.sortPartPosition);
                part.transform.localPosition = Vector3.down * mapRootInfo.sortPartDistance * j;
                realpart rp = part.GetComponent<realpart>();
                rp.Init(magicParts[i], RealPartState.Sort, mapRootInfo.sortPartPosition);
                j++;
                realparts.Add(rp);
            }
        }
        //把背包生成
        knapscak = Instantiate(knapsackGO, mapRootInfo.knapsackPosition);
        realKnapsack rk = knapscak.GetComponent<realKnapsack>();
        rk.Init(_knapsack,GameState.MapSence);
        //生成安装了的部件
        foreach (var i in _knapsack.installParts)
        {
            GameObject part = Instantiate(partGO, rk.laticePairs[i.Key].transform.parent);
            realpart rp = part.GetComponent<realpart>();
            rp.meshTran.gameObject.SetActive(false);
            realparts.Add(rp);
            rp.InitInstall(rk.laticePairs[i.Key].transform);
            //lastRealLatice.InstallPart(thisMagicPart, out installPosiTran);
            rp.Init(i.Value, RealPartState.Sort, mapRootInfo.sortPartPosition);
        }

    }
    public void ExitSortPart()
    {
        for(int i = realparts.Count - 1; i >= 0; i--)
        {
            Destroy(realparts[i].gameObject);
        }
        realparts.Clear();
        Destroy(knapscak);
    }
    [HideInInspector]
    public GameObject secondBoard;
    //生成二级菜单
    public GameObject instanSecondBoard(secondBoardInfo secondboardInfo)
    {
        secondBoard = Instantiate(uiSecondBoardGOList[secondboardInfo.order], mapRootInfo.selectBoardPosi);
        uiSecondBoard uis = secondBoard.GetComponent<uiSecondBoard>();
        uis.EnterInit(secondboardInfo);
        return secondBoard;
    }
    public void exitSecondBoard()
    {
        Destroy(secondBoard);
    }
    //商店
    [HideInInspector]
    public GameObject shopBoard;
    public void instanShopBoard()
    {
        shopBoard = Instantiate(shopUiGO, mapRootInfo.secondBoardPosi);
        uiShopBoard uiShop = shopBoard.GetComponent<uiShopBoard>();
        uiShop.Init();
    }

    //展示卡牌
    public void instanShowCard(playerCard card, int i)
    {
        GameObject cardg = Instantiate(cardGO, mapRootInfo.showCardPosi);
        realCard rc = cardg.transform.GetChild(0).GetComponent<realCard>();
        rc.Init(card, RealCardState.ShowCard);
        rc.ShowCard(i);
        rc.AlphaAnima(0);
        rc.AlphaAnima(1, 0.25f);
    }

    //战斗——抽卡
    public void instanDrawACard(card playercard)
    {
        GameObject card = Instantiate(cardGO, battleuiRoot.handCardControll);
        realCard realcard = card.GetComponentInChildren<realCard>();
        realcard.Init(playercard,RealCardState.RealCard);
        realcard.ShowDraw();
        battleuiRoot.handCardControll.GetComponent<handcardControll>().playerHandCards.Add(realcard);
    }
    //战斗——部件
    public realKnapsack instanBattleKnapsack(knapsack _knapsack)
    {
        //创建纸板
        knapscak = Instantiate(knapsackGO, battleuiRoot.cardBoardPosition);
        realKnapsack rk = knapscak.GetComponent<realKnapsack>();
        rk.Init(_knapsack,GameState.BattleSence);
        return rk;
    }
    //战斗——生成怪物
    public void instanMonster(monsterInfo moninfo,out realEnemy realEnemy)
    {
        GameObject monster = Instantiate(MonsterAll[moninfo.Id], battleEnvRoot.monsterPosi);
        realEnemy realenemy = monster.GetComponent<realEnemy>();
        realenemy.healthslider = battleuiRoot.healthSlider;
        realenemy.healthslider.SetMonsterHead(moninfo.Id);
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
    //战斗——生成特效
    public void instanPerformEffect(PerformEffect perform)
    {
        GameObject effectGO = perform.effect;
        GameObject neweffect = Instantiate(effectGO);
        switch (perform.kind)
        {
            case 0://主角位置
                neweffect.transform.position = gameManager.Instance.battlemanager.realplayer.transform.position;
                break;
            case 1://怪物位置
                neweffect.transform.position = gameManager.Instance.battlemanager.realenemy.transform.position;
                break;
            case 2://主》》怪
                neweffect.transform.position = gameManager.Instance.battlemanager.realplayer.transform.position;
                break;
            case 3://怪》》主
                neweffect.transform.position = gameManager.Instance.battlemanager.realenemy.transform.position;
                break;
            case 4://主角法杖位置
                neweffect.transform.position = gameManager.Instance.battlemanager.realplayer.transform.position;
                break;
        }
        neweffect.AddComponent<realEffect>().Init(perform.kind,perform.speed,perform.lasttime);
    }
}
