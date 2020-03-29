using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AllAsset;

public class uiVectorBoard : MonoBehaviour
{
    public Transform awardCard;
    public Transform[] cardPosition;
    List<playerCard> cards = new List<playerCard>();
    //List<GameObject> cardgo = new List<GameObject>();
    public GameObject d;
    private void Start()
    {
        //EnterVectorBoard();
    }
    void Update()
    {
        transform.DOScale(Vector3.one * 67, 0.2f);
    }
    public void EnterVectorBoard()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.one;

        for(int i = 0; i < 3; i++)
        {
            playerCard playerCard = ListOperation.RandomValue<playerCard>(AllAsset.cardAsset.AllIdCards);
            GameObject newcard = Instantiate(gameManager.Instance.instantiatemanager.cardGO,cardPosition[i]);
            //GameObject newcard = Instantiate(d, cardPosition[i]);
            realCard rc = newcard.GetComponentInChildren<realCard>();
            if (!rc)
            {
                Debug.Log("!");
            }
            rc.Init(playerCard, RealCardState.AwardCard);
            cards.Add(playerCard);
        }
    }
    public void ContinueToMap()
    {
        gameManager.Instance.SwitchScene(false);
        gameManager.Instance.exitBattlescene();
    }
    public void selectcard(int i)
    {
        //awardCard;
        gameManager.Instance.playerinfo.AddNewCard(cards[i]);
        awardCard.gameObject.SetActive(false);
    }
}
