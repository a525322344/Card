using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startMuneControll : MonoBehaviour
{
    public float autoplayTime;
    private float time;
    private bool b_once = true;
    public bool b_autoOver = false;
    public bool b_animaOver = false;

    private bool b_NotOpen = true;
    public List<Animator> startToPlayAnimators = new List<Animator>();
    public List<Animator> waitLoadPlay = new List<Animator>();
    public List<Transform> startToDestorys = new List<Transform>();
    public AudioListener audioListener;
    public Camera startCamera;
    public todown todown;

    public float playeruptime;

    Scene startscene;
    // Update is called once per frame
    void Update()
    {
        if (b_once)
        {
            time += Time.deltaTime;
            if (time > autoplayTime)
            {
                b_once = false;
                b_autoOver = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AsyncOperation _asyncOperation = SceneManager.LoadSceneAsync("Map");
            gameManager.Instance.uimanager.ToStart(_asyncOperation);
        }
    }

    public void ToStart()
    {
        if (b_autoOver)
        {
            buttonEnterMap();
            Debug.Log("Startle!!");
        }
    }
    //开始游戏按钮
    public void buttonEnterMap()
    {
        if (b_NotOpen)
        {
            b_NotOpen = false;
            //加载地图
            AsyncOperation _asyncOperation = SceneManager.LoadSceneAsync("Map", LoadSceneMode.Additive);
            gameManager.Instance.mapScene = SceneManager.GetSceneByName("Map");
            //_asyncOperation.allowSceneActivation = false;
            StartCoroutine(IEenterMap(_asyncOperation));
        }
    }
    IEnumerator IEenterMap(AsyncOperation _asyncOperation)
    {
        //播放动画
        foreach (Animator animator in startToPlayAnimators)
        {
            animator.SetBool("start", true);
        }
        //后面地图加载好了
        yield return new WaitUntil(() => {
            return _asyncOperation.isDone;
        });
        startscene= SceneManager.GetActiveScene();
        todown.b_todown = true;
        audioListener.enabled = false;
        startCamera.clearFlags = CameraClearFlags.Depth;
        gameManager.Instance.mapManagerInit(playeruptime);

        //播放动画
        foreach (Animator animator in waitLoadPlay)
        {
            animator.SetBool("start", true);
        }

        foreach (Transform tran in startToDestorys)
        {
            tran.gameObject.SetActive(false);
        }
        //动画播放完了
        yield return new WaitUntil(() => {
            return b_animaOver;
        });
        yield return new WaitForSeconds(1);
        startscene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Map"));
        SceneManager.UnloadSceneAsync(startscene);
        //完全切换场景

    }
    public void UnloadStartScene()
    {
        SceneManager.UnloadSceneAsync(startscene);
    }
}
