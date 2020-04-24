using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    //开始场景
    public startMuneControll startMuneControll;
    //地图场景
    [HideInInspector]
    public uiEventBoard uiBefallBoard;
    //战斗场景
    [HideInInspector]
    public Button roundEndButton;
    [HideInInspector]
    public uiVectorBoard uiVectorBoard;
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

    public void ToStart(AsyncOperation _asyncOperation)
    {
        StartCoroutine(IETestEnterMap(_asyncOperation));
    }

    IEnumerator IETestEnterMap(AsyncOperation _asyncOperation)
    {
        //后面地图加载好了
        yield return new WaitUntil(() => {
            return _asyncOperation.isDone;
        });
        gameManager.Instance.mapManagerInit();
        gameManager.Instance.mapScene = SceneManager.GetSceneByName("Map");

    }
}
