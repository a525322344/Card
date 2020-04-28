using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public delegate void onSelectCards(List<playerCard> a);
public class UisecondBoard_SelectCard : uiSecondBoard
{
    public Transform cardZeroPosition;
    public TextMeshPro describeText;
    public int numHor;
    public float distanceHor;
    public float distanceVer;

    //数据上
    public thingToSelect<playerCard> cardsToSelect;
    public int selectNum = 1;
    public List<playerCard> freeCardList;
    public List<playerCard> selectCardList = new List<playerCard>();
    //外部设置
    public onSelectCards onSelectCards;

    public override void EnterInit(secondBoardInfo secondInfo)
    {
        base.EnterInit(secondInfo);

    }

    public void Init(List<playerCard> cardlist,int selectnum)
    {
        freeCardList = new List<playerCard>(cardlist);
        selectNum = selectnum;
        cardsToSelect = new thingToSelect<playerCard>();
        cardsToSelect.onSelectcard += (_card) =>
        {
            bool once = true;
            if (freeCardList.Contains(_card)&&once)
            {
                once = false;
                freeCardList.Remove(_card);
                selectCardList.Add(_card);
            }
            else if (selectCardList.Contains(_card) && once)
            {
                once = false;
                selectCardList.Remove(_card);
                freeCardList.Add(_card);
            }
            if (selectCardList.Count == selectNum)
            {
                Exit();
            }
        };

        int h = 0;
        int v = 0;
        foreach(playerCard card in cardlist)
        {
            GameObject cardGO = Instantiate(instantiateManager.instance.cardGO, cardZeroPosition);
            cardGO.transform.localPosition = new Vector3(h * distanceHor, v * distanceVer, 0);
            realCard rc = cardGO.transform.GetChild(0).GetComponent<realCard>();
            rc.Init(card, RealCardState.SelectCard);
            rc.cardselects = cardsToSelect;
            h++;
            if (h >= numHor)
            {
                h = 0;
                v++;
            }
        }
    }
    public override void Exit()
    {
        base.Exit();
        onSelectCards(selectCardList);
        //退出这个页面
        Destroy(gameObject);
    }
}
