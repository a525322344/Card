using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllAsset;
using TMPro;
class saleCardInfo
{
    public Transform cardTran;
    public GameObject priceGO;
    public GameObject cardGO;
    public int price;
}

public class uiShopBoard : MonoBehaviour
{
    public GameObject saleMoney;
    public List<Transform> cardTrans;
    public cardsToSelect cardsToSelect;
    public List<playerCard> saleCardList = new List<playerCard>();
    Dictionary<playerCard, saleCardInfo> cardSaleDic = new Dictionary<playerCard, saleCardInfo>();
    public void Init()
    {
        List<playerCard> allcards = new List<playerCard>(cardAsset.AllIdCards);
        cardTrans = cardsToSelect.cardTrans;
        //随机选择5张牌
        for (int i = 0; i < 5;)
        {
            playerCard card = ListOperation.RandomValue<playerCard>(allcards);
            allcards.Remove(card);
            int sale=10;
            if (card.Rank == 0)
            {

            }
            else
            {
                saleCardList.Add(card);
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
                saleCardInfo saleinfo = new saleCardInfo();
                saleinfo.price = sale;
                saleinfo.cardTran = cardTrans[i];
                cardSaleDic.Add(card, saleinfo);
                i++;
            }
        }
        List<realCard> realcards;
        cardsToSelect.Init(saleCardList,out realcards);

        foreach(realCard rc in realcards)
        {
            saleCardInfo saleinfo = cardSaleDic[rc.thisCard as playerCard];
            saleinfo.priceGO = Instantiate(saleMoney,saleinfo.cardTran);
            saleinfo.cardGO = rc.transform.parent.gameObject;
            saleinfo.priceGO.transform.GetChild(0).GetComponent<TextMeshPro>().text = "" + saleinfo.price;
        }

        cardsToSelect.onSelectcard += (_card) =>
        {
            saleCardInfo saleinfo = cardSaleDic[_card];
            playerInfo playerinfo = gameManager.Instance.playerinfo;
            if (playerinfo.money >= saleinfo.price)
            {
                playerinfo.money -= saleinfo.price;
                playerinfo.AddNewCard(_card);
                //
                Destroy(saleinfo.cardGO);
                Destroy(saleinfo.priceGO);
            }
        };
    }
}
