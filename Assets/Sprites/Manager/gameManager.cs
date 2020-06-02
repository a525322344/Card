using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public enum GameState
{
    StartMenu,
    MapSence,
    BattleSence,
    Failure,
    BattleTest,
}
[System.Serializable]
public class InitControll
{
    public InitControll()
    {
        knapsackLaticInit[6] = true;
        knapsackLaticInit[7] = true;
        knapsackLaticInit[8] = true;
        knapsackLaticInit[11] = true;
        knapsackLaticInit[12] = true;
        knapsackLaticInit[13] = true;
        //knapsackLaticInit[16] = true;
        //knapsackLaticInit[17] = true;
        //knapsackLaticInit[18] = true;
    }
    public List<int> carddeckInit = new List<int>() { 0, 0, 0, 0, 1, 1, 1, 1, 2, 3 };
    public bool[] knapsackLaticInit = new bool[25]
    {
        false,false,false,false,false,
        false,true,true,true,false,
        false,true,true,true,false,
        false,true,true,true,false,
        false,false,false,false,false
    };
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

    public bool useCSInit;
    private bool[] knapsackLaticInit = new bool[25]
    {
        false,false,false,false,false,
        false,true,true,true,false,
        false,true,true,true,false,
        false,true,true,true,false,
        false,false,false,false,false
    };
    private List<int> carddeckInit = new List<int>() { 0, 0, 0, 0, 1, 1, 1, 1, 2, 3 };

    //public InitControll InitControll = new InitControll();

    public CardEditorBoard CardEditorBoard;
    public bool testcard;
    public CardEditorBoard TestCardEditor;
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
        //游戏开始
        GameStartInit();
    }

    private void Update()
    {
        if (gameState == GameState.Failure)
        {
            if (Input.anyKeyDown)
            {
                Destroy(gameObject);
                SceneManager.LoadScene("ReStart");
            }
        }
    }

    public void SwitchScene(bool tobattle)
    {
        if (tobattle)
        {
            SceneManager.SetActiveScene(battleScene);
            instantiatemanager.mapRootInfo.mapCamera.GetComponent<AudioListener>().enabled = false;
            instantiatemanager.mapRootInfo.mapCamera.enabled = false;
            //instantiatemanager.mapRootInfo.maplight.enabled = false;
            instantiatemanager.mapRootInfo.transform.gameObject.SetActive(false);
        }
        else
        {
            SceneManager.SetActiveScene(mapScene);
            instantiatemanager.mapRootInfo.mapCamera.GetComponent<AudioListener>().enabled = true;
            instantiatemanager.mapRootInfo.mapCamera.enabled = true;
            //instantiatemanager.mapRootInfo.maplight.enabled = true;
            instantiatemanager.mapRootInfo.transform.gameObject.SetActive(true);
        }
    }

    public void GameStartInit()
    {
        //初始化玩家数据
        //playerinfo = new playerInfo();
        if (useCSInit)
        {
            playerinfo.PlayerDickInit(carddeckInit);
            playerinfo.MagicPartDickInit();
            playerinfo.KnapSackInit(knapsackLaticInit);
        }
        else
        {
            playerinfo.PlayerDickInit(InitControllBoard.carddeckInit);
            playerinfo.MagicPartDickInit();
            playerinfo.KnapSackInit(InitControllBoard.knapsackLaticInit);
        }

    }

    public void mapManagerInit()
    {
        mapmanager = gameObject.GetComponent<MapManager>();
        instantiatemanager.mapRootInfo= GameObject.Find("root").GetComponent<MapRootInfo>();
        gameState = GameState.MapSence;
        instantiatemanager.mapRootInfo.uiMapContrill.Init();
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
        mapmanager.mapplayer.healthSlider.SetSlider(0, playerinfo.playerHealth);
        Destroy(battlemanager);
        SceneManager.UnloadSceneAsync(battleScene);
    }
    public void FailScene()
    {
        gameState = GameState.Failure;
        Destroy(battlemanager);
        SceneManager.LoadScene("failure");
        //SceneManager.UnloadSceneAsync(battleScene);
        //SceneManager.UnloadSceneAsync(mapScene);
    }
}
