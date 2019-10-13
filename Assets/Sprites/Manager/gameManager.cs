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

    public Camera Encamera;
    public Camera UIcamera;

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
        a inserta = new a("插入");
        a a1 = new a("1");
        ass.Add(new a("0"));
        ass.Add(a1);
        ass.Add(new a("2"));
        ListOperation.InsertItemAfter(ass, a1, inserta);
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

    public List<a> ass = new List<a>();
    

}
//test
[System.Serializable]
public class a
{
    public string st = "a";
    public a(string stt)
    {
        st = stt;
    }
}