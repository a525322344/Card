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
    public ROUND GameRound;

    private int drawCardQuantityInit=5;
    private int drawCardQuantityLast;
    private int drawCardQuantityAjust;

    public playerCard playerCard = new playerCard(1, "法力冲击", CardKind.PlayerCard, new CardEffect(AllAsset.effectAsset.addDemage));
    // Start is called before the first frame update
    void Start()
    {
        GameRound = ROUND.PlayerRound;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerCard.debug();
        }
        if (Input.GetMouseButtonDown(1))
        {
            playerCard.cardEffect(5);
        }
    }

    public void StartRound()
    {
        drawCardQuantityLast = drawCardQuantityInit + drawCardQuantityAjust;
    }
    private void OnMouseDown()
    {
        
    }
}
