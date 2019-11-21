using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UImanager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
