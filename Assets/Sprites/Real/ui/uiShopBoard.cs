using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllAsset;
using TMPro;
class saleInfo
{
    public Transform thingTran;
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

    public thingToSelect<playerCard> cardsToBuy;
    public thingToSelect<MagicPart> partsToBuy;
    public uiSecondBoard ToDeleteCardBoard;

    Dictionary<playerCard, saleInfo> cardSaleDic = new Dictionary<playerCard, saleInfo>();
    Dictionary<MagicPart, saleInfo> partSaleDic = new Dictionary<MagicPart, saleInfo>();
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
                saleInfo saleinfo = new saleInfo();
                saleinfo.price = sale;
                saleinfo.thingTran = cardTrans[i];
                GameObject cardGO = Instantiate(instantiateManager.instance.cardGO, cardTrans[i]);
                realCard realcard = cardGO.transform.GetChild(0).GetComponent<realCard>();
                realcard.Init(card, RealCardState.SelectCard);
                realcard.cardselects = cardsToBuy;

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
            saleInfo saleinfo = cardSaleDic[_card];
            playerInfo playerinfo = gameManager.Instance.playerinfo;
            if (playerinfo.money >= saleinfo.price)
            {
                playerinfo.money -= saleinfo.price;
                playerinfo.AddNewCard(_card);
                //
                Destroy(saleinfo.thingGO);
                Destroy(saleinfo.priceGO);
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
            if (part.rank == -1)
            {

            }
            else
            {
                saleInfo saleInfo = new saleInfo();
                saleInfo.price = sale;
                saleInfo.thingTran = partTrans[i];
                GameObject partGO = Instantiate(instantiateManager.instance.partGO, partTrans[i].GetChild(0));
                realpart rp = partGO.GetComponent<realpart>();
                rp.Init(part, RealPartState.Select);
                rp.partToSelect = partsToBuy;
                saleInfo.priceGO = Instantiate(saleMoney, saleInfo.thingTran);
                saleInfo.thingGO = partGO;
                saleInfo.priceGO.transform.GetChild(0).GetComponent<TextMeshPro>().text = "" + saleInfo.price;
                partSaleDic.Add(part, saleInfo);
                i++;
            }
        }
        secondBoardInfo PartSortBoardInfo = new secondBoardInfo(0);
        PartSortBoardInfo.onEnter = () =>
        {
            gameObject.SetActive(false);
        };
        PartSortBoardInfo.onExit = () =>
        {
            gameObject.SetActive(true);
        };
        partsToBuy.onSelectcard += (_part) =>
        {
            saleInfo saleinfo = partSaleDic[_part];
            playerInfo playerinfo = gameManager.Instance.playerinfo;
            if (playerinfo.money >= saleinfo.price)
            {
                playerinfo.money -= saleinfo.price;
                playerinfo.AddNewPart(_part);

                Destroy(saleinfo.thingGO);
                Destroy(saleinfo.priceGO);
                //打开部件配置页面
                instantiateManager.instance.instanSecondBoard(PartSortBoardInfo);
            }
        };
        //注册退出按钮
        exitbutton.AddListener(() =>
        {
            gameManager.Instance.mapmanager.mapState = MapState.MainMap;
            Destroy(gameObject);
        });
    }
}
