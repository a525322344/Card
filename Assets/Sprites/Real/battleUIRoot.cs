﻿using System.Collections;
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
    //储存部件的坐标
    public List<Vector3> partPositionList = new List<Vector3>();
    //意图的位置
    public Transform actionTran;
    public List<Transform> parttransforms = new List<Transform>();
    //弃卡选择框的位置
    public Transform waitSelectCard;
}
