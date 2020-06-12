using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoBattleTest : MonoBehaviour
{
    public GameObject gamemanager;
    private monsterInfo monsterInfo;
    // Start is called before the first frame update
    void Start()
    {
        if (!gameManager.Instance)
        {
            GameObject gameManagerTest = Instantiate(gamemanager);
            gameManager.Instance.GameStartInit();
            monsterInfo = new monInfo_MoNv();
            gameManager.Instance.gameState = GameState.BattleSence;
            gameManager.Instance.battleManagerInit();
            gameManager.Instance.battlemanager.InitBattlemanaget();
            gameManager.Instance.battlemanager.BattleStartEnemySet(monsterInfo);
            gameManager.Instance.battlemanager.startBattale();
        }
    }
}
