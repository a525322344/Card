using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    public Button roundEndButton;
    //战斗场景回合结束按钮
    public void EndRound()
    {
        Debug.Log("endclick");
        gameManager.Instance.battlemanager.b_toEndRound = true;
    }

    public void InitEnterMap()
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
