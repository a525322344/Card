using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ROUND
{
    PlayerRound,
    EnemyRound
}



public class gameManager : MonoBehaviour
{
    static gameManager _instance;
    public static gameManager Instance
    {
        get
        {
            // 不需要再检查变量是否为null
            return _instance;
        }
    }

    public playerAsset playerAsset;

    public ROUND GameRound;
    public enemybase EmenyClass;
    public enemybase PlayerClass;

    public GameObject cardMode;
    public Transform handCardControll;
    private int drawCardQuantityInit=6;
    private int drawCardQuantityLast;
    private int drawCardQuantityAjust;

    //public playerCard playerCard = new playerCard(1, "法力冲击", CardKind.PlayerCard, new CardEffect(AllAsset.effectAsset.giveDemage));

    void Awake()
    {
        _instance = this;
        playerAsset = GetComponent<playerAsset>();
    }
    void Start()
    {
        GameRound = ROUND.PlayerRound;
        startBattale();
        playerAsset.playerDickInGame = ListOperation.Shufle<playerCard>(playerAsset.playerDickInGame);
        StartRound();

    }
    public int check;
    public int min = 0;
    public int max = 5;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            check = Random.Range(0, max);
            
        }
        if (Input.GetMouseButtonDown(1))
        {
            check = Random.Range(min, max);
        }
    }
    public void startBattale()
    {
        playerAsset.playerDickInGame = playerAsset.playerDick;
    }
    public void StartRound()
    {
        drawCardQuantityLast = drawCardQuantityInit + drawCardQuantityAjust;
        for(int i = 0; i < drawCardQuantityLast; i++)
        {
            GameObject card = Instantiate(cardMode, handCardControll);
            card.GetComponent<realCard>().SetThiscard(playerAsset.playerDickInGame[i]);
            handCardControll.GetComponent<handcardControll>().playerHandCards.Add(card.GetComponent<realCard>());
        }
    }

}
