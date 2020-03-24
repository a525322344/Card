using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [HideInInspector]
    public Button roundEndButton;
    [HideInInspector]
    public uiEventBoard uiBefallBoard;
    public startMuneControll startMuneControll;
    //战斗场景回合结束按钮
    public void EndRound()
    {
        gameManager.Instance.battlemanager.b_toEndRound = true;
    }

    //加载地图场景时加载
    public void InitMapUI()
    {
        uiBefallBoard = gameManager.Instance.instantiatemanager.mapRootInfo.uieventBoard;
        uiBefallBoard.gameObject.SetActive(false);
    }
}
