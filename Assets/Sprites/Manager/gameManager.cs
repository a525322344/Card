using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



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
    //[HideInInspector]
    public playerInfo playerinfo;
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
        float pi = Mathf.PI;
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

    private void GameStartInit()
    {
        //初始化玩家数据
        playerinfo = new playerInfo();
        playerinfo.PlayerDickInit(InitControllBoard.carddeckInit);
        playerinfo.MagicPartDickInit();
        playerinfo.KnapSackInit(InitControllBoard.knapsackLaticInit);
    }

    public void mapManagerInit()
    {
        mapmanager = gameObject.AddComponent<MapManager>();
        instantiatemanager.mapRootInfo= GameObject.Find("root").GetComponent<MapRootInfo>();
        mapmanager.InitMap();
        uimanager.InitMapUI();
    }

    public void battleManagerInit()
    {
        SwitchScene(true);
        battlemanager = gameObject.AddComponent<battleManager>();
        instantiatemanager.battleuiRoot = GameObject.Find("CameraUI").GetComponent<battleUIRoot>();
        instantiatemanager.battleEnvRoot = GameObject.Find("Environment").GetComponent<BattleEnvRoot>();
        uimanager.uiVectorBoard = instantiatemanager.battleuiRoot.uiVectorBoard;
        uimanager.roundEndButton = instantiatemanager.battleuiRoot.RoundEndButton;
        uimanager.roundEndButton.onClick.AddListener(()=> { uimanager.EndRound(); });
    }
    public void exitBattlescene()
    {
        SceneManager.UnloadSceneAsync(battleScene);
    }
}
