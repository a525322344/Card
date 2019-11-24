﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum GameState
{
    StartMenu,
    MapSence,
    BattleSence
}

public class gameManager : MonoBehaviour
{
    //外部静态访问
    static gameManager _instance;
    public static gameManager Instance
    {
        get
        {
            // 不需要再检查变量是否为null
            return _instance;
        }
    }
    public GameState gameState;
    //玩家信息管理记录类
    public playerInfo playerinfo;
    //战斗管理器
    //[HideInInspector]
    public battleManager battlemanager;
    //实例管理器
    //[HideInInspector]
    public instantiateManager instantiatemanager;
    //地图管理器
    public MapManager mapmanager;
    public UImanager uimanager;

    //public Camera Encamera;
    //public Camera UIcamera;

    void Awake()
    {
        _instance = this;
        //待定
        instantiatemanager = GetComponent<instantiateManager>();
        uimanager = GetComponent<UImanager>();
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        //游戏开始
        GameStartInit();
        //默认进入第一场战斗
        //  /这里直接获得，正式时要先实例，赋予数据
        //battlemanager = GetComponent<battleManager>();
        //battlemanager.startBattale();
    }

    void Update()
    {
        
    }

    private void GameStartInit()
    {
        //初始化玩家数据
        playerinfo = new playerInfo();
        playerinfo.PlayerDickInit();
        playerinfo.MagicPartDickInit();
    }

    public void mapManagerInit()
    {
        mapmanager = gameObject.AddComponent<MapManager>();
        instantiatemanager.mapRootInfo= GameObject.Find("root").GetComponent<MapRootInfo>();
        mapmanager.InitMap();
    }

    public void battleManagerInit()
    {
        battlemanager = gameObject.AddComponent<battleManager>();
        instantiatemanager.battleuiRoot = GameObject.Find("CameraUI").GetComponent<battleUIRoot>();
        battlemanager.startBattale();
    }
}
