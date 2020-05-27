using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class uiMapContrill : MonoBehaviour
{
    public spriteButton partui;
    public spriteButton cardui;
    public spriteButton money;
    public healthSlider healthSlider;
    public TextMeshPro moneytext;

    private GameObject secondcardGO;
    private GameObject secondpartGO;
    private bool isopenOne;

    private MapState laststate;
    // Start is called before the first frame update
    public void Init()
    {
        playerInfo player = gameManager.Instance.playerinfo;
        healthSlider.isbattle = false;
        healthSlider.Init(player);
        cardui.AddListener(() =>
        {
            laststate = gameManager.Instance.mapmanager.mapState;
            gameManager.Instance.mapmanager.mapState = MapState.EventWindow;
            if (secondcardGO|isopenOne)
            {

            }
            else
            {
                if (gameManager.Instance.uimanager.IsEventOn)
                {
                    gameManager.Instance.uimanager.uiBefallBoard.SetActive(false);
                }

                isopenOne = true;
                List<playerCard> selectcardList = player.playerDeck;
                secondBoardInfo second = new secondBoardInfo(1);
                secondcardGO = Instantiate(instantiateManager.instance.uiSecondBoardGOList[second.order], instantiateManager.instance.mapRootInfo.selectBoardPosi);
                UisecondBoard_SelectCard uis = secondcardGO.GetComponent<UisecondBoard_SelectCard>();
                uis.EnterInit(second);
                uis.Init(selectcardList, -1);

                uis.describeText.text = "牌库";
                uis.CancelButton.AddListener(() =>
                {

                    if (gameManager.Instance.uimanager.IsEventOn)
                    {
                        gameManager.Instance.uimanager.uiBefallBoard.SetActive(true);
                        gameManager.Instance.mapmanager.EventWindow(true);
                    }
                    gameManager.Instance.mapmanager.mapState = laststate;
                    isopenOne = false;
                    Destroy(secondcardGO);
                });
                uis.onSelectCards = (cardlist) =>
                {

                };
            }
        });
        partui.AddListener(() =>
        {
            laststate = gameManager.Instance.mapmanager.mapState;
            gameManager.Instance.mapmanager.mapState = MapState.EventWindow;
            if (secondpartGO|isopenOne)
            {

            }
            else
            {
                isopenOne = true;
                //gameManager.Instance.mapmanager.EventWindow(true, 0.5f);
                secondBoardInfo secondBoard = new secondBoardInfo(0);
                GameObject sbui = gameManager.Instance.instantiatemanager.instanSecondBoard(secondBoard);
                sbui.GetComponent<uiSecondBoard>().exitToDo += () =>
                {
                    isopenOne = false;
                    //gameManager.Instance.mapmanager.EventWindow(false, 0.5f);
                    gameManager.Instance.mapmanager.mapState = laststate;
                };
            }
        });
        SetMoney();
    }

    public void SetMoney()
    {
        moneytext.text = "" + gameManager.Instance.playerinfo.money;
    }
}
