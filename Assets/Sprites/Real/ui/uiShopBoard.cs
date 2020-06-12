using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllAsset;
using TMPro;
using DG.Tweening;
class saleInfo<T_real>
{
    public Transform thingTran;
    public T_real realCs;
    public GameObject priceGO;
    public GameObject thingGO;
    public int price;
}

public class uiShopBoard : MonoBehaviour
{
    public GameObject saleMoney;
    public List<Transform> cardTrans;
    public List<Transform> partTrans;
    public spriteButton exitbutton;
    public Transform DeleteButton;
    public Transform[] saleposis;
    public int deleteCardMoney;
    public float downmove;
    public float moveUpTime = 0.2f;

    public bool On = true;
    public thingToSelect<playerCard> cardsToBuy;
    public thingToSelect<MagicPart> partsToBuy;
    public uiSecondBoard ToDeleteCardBoard;

    Dictionary<playerCard, saleInfo<realCard>> cardSaleDic = new Dictionary<playerCard, saleInfo<realCard>>();
    Dictionary<MagicPart, saleInfo<realpart>> partSaleDic = new Dictionary<MagicPart, saleInfo<realpart>>();
    saleInfo<spriteButton> deleteSale;
    public void Init()
    {
        //初始化卡牌购买
        List<playerCard> allcards = new List<playerCard>(cardAsset.AllIdCards);
        cardsToBuy = new thingToSelect<playerCard>();
        //随机选择5张牌
        for (int i = 0; i < cardTrans.Count;)
        {
            playerCard card = ListOperation.RandomValue<playerCard>(allcards);
            allcards.Remove(card);
            int sale=10;
            if (card.Rank == 0)
            {

            }
            else
            {
                //saleCardList.Add(card);
                switch (card.Rank)
                {
                    case 1:
                        sale = 30;
                        break;
                    case 2:
                        sale = 50;
                        break;
                    case 3:
                        sale = 100;
                        break;
                }
                saleInfo<realCard> saleinfo = new saleInfo<realCard>();
                saleinfo.price = sale;
                saleinfo.thingTran = cardTrans[i];
                GameObject cardGO = Instantiate(instantiateManager.instance.cardGO, cardTrans[i]);
                realCard realcard = cardGO.transform.GetChild(0).GetComponent<realCard>();
                realcard.Init(card, RealCardState.SelectCard);
                realcard.cardselects = cardsToBuy;
                saleinfo.realCs = realcard;
                saleinfo.priceGO = Instantiate(saleMoney, saleinfo.thingTran);
                saleinfo.thingGO = cardGO;
                saleinfo.priceGO.transform.GetChild(0).GetComponent<TextMeshPro>().text = "" + saleinfo.price;
                cardSaleDic.Add(card, saleinfo);
                i++;
            }
        }
        //设置回调 
        cardsToBuy.onSelectcard += (_card) =>
        {
            saleInfo<realCard> saleinfo = cardSaleDic[_card];
            playerInfo playerinfo = gameManager.Instance.playerinfo;
            if (playerinfo.money >= saleinfo.price)
            {
                playerinfo.GetMoney(-saleinfo.price);
                playerinfo.AddNewCard(_card);
                //
                Destroy(saleinfo.thingGO);
                //Destroy(saleinfo.priceGO);
            }
        };

        //初始化部件购买
        List<MagicPart> allparts = new List<MagicPart>(magicpartAsset.AllMagicParts);
        partsToBuy = new thingToSelect<MagicPart>();
        for(int i = 0; i < partTrans.Count; i++)
        {
            MagicPart part = ListOperation.RandomValue<MagicPart>(allparts);
            allparts.Remove(part);
            int sale = 100;

            saleInfo<realpart> saleInfo = new saleInfo<realpart>();
            saleInfo.price = sale;
            saleInfo.thingTran = saleposis[i];
            GameObject partGO = Instantiate(instantiateManager.instance.partGO, partTrans[i].GetChild(0));
            realpart rp = partGO.GetComponent<realpart>();
            rp.Init(part, RealPartState.Select);
            rp.partToSelect = partsToBuy;
            saleInfo.realCs = rp;
            saleInfo.priceGO = Instantiate(saleMoney, saleInfo.thingTran);
            //saleInfo.priceGO.transform.position = rp.saleposi.position;
            saleInfo.thingGO = partGO;
            saleInfo.priceGO.transform.GetChild(0).GetComponent<TextMeshPro>().text = "" + saleInfo.price;
            partSaleDic.Add(part, saleInfo);
        }
        secondBoardInfo PartSortBoardInfo = new secondBoardInfo(0);
        PartSortBoardInfo.onEnter = () =>
        {
            gameObject.SetActive(false);
        };
        PartSortBoardInfo.onExit = () =>
        {
            gameObject.SetActive(true);
            TurnOn(true);
        };
        partsToBuy.onSelectcard += (_part) =>
        {
            saleInfo<realpart> saleinfo = partSaleDic[_part];
            playerInfo playerinfo = gameManager.Instance.playerinfo;
            if (playerinfo.money >= saleinfo.price)
            {
                playerinfo.GetMoney(-saleinfo.price);
                playerinfo.AddMagicPart(_part);

                Destroy(saleinfo.thingGO);
                Destroy(saleinfo.priceGO);
                //打开部件配置页面
                TurnOn(false);
                instantiateManager.instance.instanSecondBoard(PartSortBoardInfo);
            }
        };
        //注册退出按钮
        exitbutton.AddListener(() =>
        {
            gameManager.Instance.mapmanager.EventWindow(false); //mapState = MapState.MainMap;
            Destroy(gameObject);
        });
        //注册删卡按键
        deleteSale = new saleInfo<spriteButton>();
        deleteSale.price = deleteCardMoney;
        deleteSale.priceGO = Instantiate(saleMoney, DeleteButton);
        deleteSale.priceGO.transform.GetChild(0).GetComponent<TextMeshPro>().text = "" + deleteSale.price;
        deleteSale.thingGO = DeleteButton.GetChild(0).gameObject;
        deleteSale.thingTran = DeleteButton;
        deleteSale.realCs = DeleteButton.GetChild(0).GetComponent<spriteButton>();
        secondBoardInfo selectBoardInfo = new secondBoardInfo(1);
        DeleteButton.GetChild(0).GetComponent<spriteButton>().AddListener(() =>
        {
            playerInfo playerinfo = gameManager.Instance.playerinfo;
            if (playerinfo.money >= deleteSale.price)
            {
                //出现删卡面板
                TurnOn(false);
                GameObject selectcard = instantiateManager.instance.instanSecondBoard(selectBoardInfo);
                UisecondBoard_SelectCard uiselectboard = selectcard.GetComponent<UisecondBoard_SelectCard>();
                uiselectboard.EnterInit(selectBoardInfo);
                uiselectboard.Init(gameManager.Instance.playerinfo.playerDeck, 1);
                uiselectboard.describeText.text = "删除1张卡";
                uiselectboard.CancelButton.AddListener(() =>
                {
                    TurnOn(true);
                    gameManager.Instance.mapmanager.EventWindow(true);
                    Destroy(uiselectboard.gameObject);
                });
                uiselectboard.onSelectCards = (cardlist) =>
                {
                    foreach(playerCard card in cardlist)
                    {
                        gameManager.Instance.playerinfo.RemoveCard(card);
                    }
                    //删卡成功后
                    TurnOn(true);
                    playerinfo.GetMoney(-deleteSale.price);
                    gameManager.Instance.mapmanager.EventWindow(true);
                    Destroy(deleteSale.thingGO);
                    Destroy(deleteSale.priceGO);
                };
            }
        });

        MoveUp();
    }

    public void TurnOn(bool on)
    {
        On = on;
        if (On)
        {
            gameObject.SetActive(true);
            foreach (var d in cardSaleDic)
            {
                d.Value.realCs.handCardState = HandCardState.Freedom;
            }
            foreach (var d in partSaleDic)
            {
                d.Value.realCs.sortState = SortState.Free;
            }
            deleteSale.realCs.isMoseOn = true;
            exitbutton.isMoseOn = true;
        }
        else
        {
            gameObject.SetActive(false);
            foreach(var d in cardSaleDic)
            {
                d.Value.realCs.handCardState = HandCardState.Other;
            }
            foreach(var d in partSaleDic)
            {
                d.Value.realCs.sortState = SortState.Other;
            }
            deleteSale.realCs.isMoseOn = false;
            exitbutton.isMoseOn = false;
        }
    }
    public void MoveUp()
    {
        transform.localPosition = new Vector3(0, -downmove, 0);
        transform.DOLocalMoveY(0, moveUpTime);
    }
}
