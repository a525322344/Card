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
            List<playerCard> selectcardList = player.playerDeck;
            secondBoardInfo second = new secondBoardInfo(1);
            secondcardGO = Instantiate(instantiateManager.instance.uiSecondBoardGOList[second.order], instantiateManager.instance.mapRootInfo.selectBoardPosi);
            UisecondBoard_SelectCard uis = secondcardGO.GetComponent<UisecondBoard_SelectCard>();
            uis.EnterInit(second);
            uis.Init(selectcardList, -1);
            uis.describeText.text = "牌库";
            uis.CancelButton.AddListener(() =>
            {
                gameManager.Instance.mapmanager.mapState = laststate;
                Destroy(secondcardGO);
            });
            uis.onSelectCards = (cardlist) =>
            {

            };

        });
        partui.AddListener(() =>
        {
            laststate = gameManager.Instance.mapmanager.mapState;
            gameManager.Instance.mapmanager.mapState = MapState.EventWindow;
            secondBoardInfo secondBoard = new secondBoardInfo(0);
            GameObject sbui = gameManager.Instance.instantiatemanager.instanSecondBoard(secondBoard);
            sbui.GetComponent<uiSecondBoard>().exitToDo += () =>
            {
                gameManager.Instance.mapmanager.mapState = laststate;
            };
        });
        SetMoney();
    }

    public void SetMoney()
    {
        moneytext.text = "" + gameManager.Instance.playerinfo.money;
    }
}
