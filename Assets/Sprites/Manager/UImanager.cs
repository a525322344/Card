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

    //开始游戏按钮
    public void buttonEnterMap()
    {
        //加载地图
        AsyncOperation _asyncOperation = SceneManager.LoadSceneAsync("Map");
        StartCoroutine(IEenterMap(_asyncOperation));
    }
    IEnumerator IEenterMap(AsyncOperation _asyncOperation)
    {
        yield return new WaitUntil(() => {
            return _asyncOperation.isDone;
        });
        gameManager.Instance.mapManagerInit();
    }

}
