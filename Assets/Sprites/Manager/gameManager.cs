using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public enum GameState
{
    StartMenu,
    MapSence,
    BattleSence,
    BattleTest,
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
    //[HideInInspector]
    public playerInfo playerinfo;
    //数据加载类
    public InitData initdata;
    //战斗管理器
    [HideInInspector]
    public battleManager battlemanager;
    //实例管理器
    [HideInInspector]
    public instantiateManager instantiatemanager;
    //地图管理器
    [HideInInspector]
    public MapManager mapmanager;
    [HideInInspector]
    public UImanager uimanager;
    public InitControllBoard InitControllBoard;
    public CardEditorBoard CardEditorBoard;

    public Scene battleScene;
    public Scene mapScene;
    //public Camera Encamera;
    //public Camera UIcamera;

    void Awake()
    {
        _instance = this;
        //待定
        instantiatemanager = GetComponent<instantiateManager>();
        uimanager = GetComponent<UImanager>();
        DontDestroyOnLoad(gameObject);
        //游戏数据
        initdata = new InitData();
        initdata.Awake();
    }
    void Start()
    {
        //游戏开始
        GameStartInit();
    }

    public void SwitchScene(bool tobattle)
    {
        if (tobattle)
        {
            SceneManager.SetActiveScene(battleScene);
            instantiatemanager.mapRootInfo.mapCamera.GetComponent<AudioListener>().enabled = false;
            instantiatemanager.mapRootInfo.mapCamera.enabled = false;
            instantiatemanager.mapRootInfo.maplight.enabled = false;
            instantiatemanager.mapRootInfo.transform.gameObject.SetActive(false);
        }
        else
        {
            SceneManager.SetActiveScene(mapScene);
            instantiatemanager.mapRootInfo.mapCamera.GetComponent<AudioListener>().enabled = true;
            instantiatemanager.mapRootInfo.mapCamera.enabled = true;
            instantiatemanager.mapRootInfo.maplight.enabled = true;
            instantiatemanager.mapRootInfo.transform.gameObject.SetActive(true);
        }
    }

    public void GameStartInit()
    {
        //初始化玩家数据
        //playerinfo = new playerInfo();
        playerinfo.PlayerDickInit(InitControllBoard.carddeckInit);
        playerinfo.MagicPartDickInit();
        playerinfo.KnapSackInit(InitControllBoard.knapsackLaticInit);
    }

    public void mapManagerInit()
    {
        mapmanager = gameObject.GetComponent<MapManager>();
        instantiatemanager.mapRootInfo= GameObject.Find("root").GetComponent<MapRootInfo>();
        gameState = GameState.MapSence;
        mapmanager.InitMap();
        uimanager.InitMapUI();
    }

    public void battleManagerInit()
    {
        battlemanager = gameObject.AddComponent<battleManager>();
        instantiatemanager.battleuiRoot = GameObject.Find("CameraUI").GetComponent<battleUIRoot>();
        instantiatemanager.battleEnvRoot = GameObject.Find("Environment").GetComponent<BattleEnvRoot>();
        uimanager.uiVectorBoard = instantiatemanager.battleuiRoot.uiVectorBoard;
        uimanager.roundEndButton = instantiatemanager.battleuiRoot.RoundEndButton;
        uimanager.roundEndButton.onClick.AddListener(()=> { uimanager.EndRound(); });
    }
    public void exitBattlescene()
    {
        gameManager.Instance.SwitchScene(false);
        playerinfo.playerHealth = battlemanager.battleInfo.Player.healthnow;
        Destroy(battlemanager);
        SceneManager.UnloadSceneAsync(battleScene);
    }
}
