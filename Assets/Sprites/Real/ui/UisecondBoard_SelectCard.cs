using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public delegate void onSelectCards<T>(List<T> a);
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
    public onSelectCards<playerCard> onSelectCards;
    public spriteButton CancelButton;
    //滑块设置
    public Transform cardscale;
    public Transform cardFolder;
    public GameObject sliderFolder;
    public Transform curpos;
    public Transform sliderUpPosi;
    public Transform sliderDownPosi;
    public bool canmoveslider;
    public float slideralllength;
    public float upz;
    public float downz;

    public float allLength;                //总长
    public float present;           //占比
    public float screenLength = 16; //
    public float canmoveLength;
    public float cardlength = 5.9f;

    private void Update()
    {
        if (canmoveslider)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)//下
            {
                present += 0.4f;
                if (present > 1)
                {
                    present = 1;
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)//上
            {
                present -= 0.4f;
                if (present < 0)
                {
                    present = 0;
                }
            }
            canmoveLength = allLength - screenLength;
            cardFolder.transform.DOLocalMoveY(present * canmoveLength, 0.2f);
            curpos.DOLocalMoveY(upz - present * slideralllength, 0.2f);
        }
    }

    public void Init(List<playerCard> cardlist,int selectnum,int i=0)
    {
        gameManager.Instance.mapmanager.EventWindow(true, 0.8f);
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
            if (i != 0)
            {
                rc.orderCost();
            }
            rc.cardselects = cardsToSelect;
            h++;
            if (h >= numHor)
            {
                h = 0;
                v++;
            }
        }
        //计算总长
        if (h == 0){
            v--;
        }
        allLength = v * -distanceVer * cardscale.localScale.y + cardlength;
        if (screenLength < allLength)
        {
            sliderFolder.SetActive(true);
            canmoveLength = allLength - screenLength;
            upz = sliderUpPosi.position.y;
            downz = sliderDownPosi.position.y;
            slideralllength = upz - downz;
            canmoveslider = true;
        }
        else
        {
            sliderFolder.SetActive(false);
            canmoveslider = false;
        }


        CancelButton.AddListener(() =>
        {
            gameManager.Instance.mapmanager.EventWindow(false, 1);
        });
    }
    public override void Exit()
    {
        base.Exit();
        onSelectCards(selectCardList);
        gameManager.Instance.mapmanager.EventWindow(false, 1);
        //退出这个页面
        Destroy(gameObject);
    }
}
