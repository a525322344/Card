using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battleUIRoot : MonoBehaviour
{
    public Canvas uiCanvas;
    public Button RoundEndButton;
    //手牌的位置
    public Transform handCardControll;
    public Transform bookFolderTran;
    //牌库的位置
    public Transform dicktran;
    //纸板的位置
    public Transform cardBoardPosition;
    //意图的位置
    public Transform actionTran;

    //弃卡选择框的位置
    public Transform waitSelectCard;
    //胜利结算面板
    public uiVectorBoard uiVectorBoard;
}
