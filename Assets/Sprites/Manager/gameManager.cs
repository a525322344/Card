using System.Collections;
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

    //玩家信息管理记录类
    public playerInfo playerinfo;
    //战斗管理器
    public battleManager battlemanager;
    //实例管理器
    public instantiateManager instantiatemanager;
    //地图管理器
    public MapManager mapmanager;



    void Awake()
    {
        _instance = this;

        //待定
        instantiatemanager = GetComponent<instantiateManager>();
    }
    void Start()
    {
        //游戏开始
        GameStartInit();
        //默认进入第一场战斗
        //  /这里直接获得，正式时要先实例，赋予数据
        battlemanager = GetComponent<battleManager>();
        battlemanager.startBattale();
        //battlemanager.startRound();
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



}
