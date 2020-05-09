using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AllAsset;

public class uiVectorBoard : MonoBehaviour
{
    bool isOn;
    public spriteButton selectcard;
    public spriteButton selectpart;
    public Transform secondpos;
    private GameObject selectcardGO;
    private GameObject selectpartGO;
    List<playerCard> cards = new List<playerCard>();
    //List<GameObject> cardgo = new List<GameObject>();
    public GameObject d;
    
    public void Init()
    {
        gameObject.SetActive(true);
        isOn = true;
        playerInfo player = gameManager.Instance.playerinfo;
        selectcard.AddListener(() =>
        {
            if (isOn)
            {
                if (selectcardGO)
                {
                    selectcardGO.SetActive(true);
                }
                else
                {
                    List<playerCard> selectcardList = ListOperation.RandomValueList<playerCard>(AllAsset.cardAsset.canGetCards, 3);
                    secondBoardInfo second = new secondBoardInfo(1);
                    selectcardGO = Instantiate(instantiateManager.instance.uiSecondBoardGOList[second.order], secondpos);
                    UisecondBoard_SelectCard uis = selectcardGO.GetComponent<UisecondBoard_SelectCard>();
                    uis.EnterInit(second);
                    uis.Init(selectcardList, 1);
                    uis.describeText.text = "选择1张卡牌";
                    uis.CancelButton.AddListener(() =>
                    {
                        selectcardGO.SetActive(false);
                        isOn = true;
                    });
                    uis.onSelectCards = (cardlist) =>
                      {
                          isOn = true;
                          foreach(playerCard card in cardlist)
                          {
                              player.AddNewCard(card);
                          }
                          Destroy(selectcardGO);
                          selectcard.gameObject.SetActive(false);
                      };
                }
            }
        });
        selectpart.AddListener(() =>
        {
            if (isOn)
            {
                if (selectpartGO)
                {
                    selectpartGO.SetActive(true);
                }
                else
                {
                    //随机选出可选部件
                    List<MagicPart> selectparts = ListOperation.RandomValueList<MagicPart>(AllAsset.magicpartAsset.AllMagicParts, player.treasureToSelectNum);
                    //打开选择部件面板
                    secondBoardInfo secondBoard = new secondBoardInfo(2);

                    selectpartGO = Instantiate(instantiateManager.instance.uiSecondBoardGOList[secondBoard.order], secondpos);
                    UisecondBoard_SelectPart uis = selectpartGO.GetComponent<UisecondBoard_SelectPart>();
                    uis.EnterInit(secondBoard);
                    uis.Init(selectparts, 1);
                    uis.describeText.text = "选择1个部件";
                    uis.CancelButton.AddListener(() =>
                    {
                        selectpartGO.SetActive(false);
                        isOn = true;
                    });
                    uis.onSelectParts = (partlist) =>
                    {
                        isOn = true;
                        foreach (MagicPart part in partlist)
                        {
                            player.AddMagicPart(part);
                        }
                        Destroy(selectpartGO);
                        selectpart.gameObject.SetActive(false);
                    };
                }
            }
        });
    }
    void Update()
    {
        transform.DOScale(Vector3.one * 67, 0.2f);
    }

    public void ContinueToMap()
    {
        if (isOn)
        {
            gameManager.Instance.exitBattlescene();
        }
    }
    //public void selectcard(int i)
    //{
    //    ////awardCard;
    //    //gameManager.Instance.playerinfo.AddNewCard(cards[i]);
    //    //awardCard.gameObject.SetActive(false);
    //}
}
