using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void UseCard(playerCard card);
public class cardsToSelect : MonoBehaviour
{
    public List<Transform> cardTrans = new List<Transform>();
    List<playerCard> CardList = new List<playerCard>();
    public UseCard onSelectcard;

    public void Init(List<playerCard> cardlist,out List<realCard> realcardList)
    {
        CardList = cardlist;
        int i = 0;
        int TransLength = cardTrans.Count;
        List<realCard> realcards = new List<realCard>();
        foreach(playerCard card in CardList)
        {
            if (i < TransLength)
            {
                GameObject cardGo = Instantiate(gameManager.Instance.instantiatemanager.cardGO,cardTrans[i]);
                realCard realcard = cardGo.transform.GetChild(0).GetComponent<realCard>();
                realcards.Add(realcard);
                realcard.Init(card, RealCardState.SelectCard);
                realcard.cardselects = this;
                i++;
            }
        }
        realcardList = realcards;
    }

    public void selecThisCard(playerCard card)
    {
        onSelectcard(card);
    }
}
