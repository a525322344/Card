using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiSecondBoard_SortPart : uiSecondBoard
{
    public GameObject partGO;
    public GameObject knapsackGO;
    public Transform partPosition;
    public Transform knapsackPosition;
    public Button exitbutton;
    public float partDistance;
    public override void EnterInit(secondBoardInfo secondInfo)
    {
        exitbutton.onClick.AddListener(Exit);
        //初始化背包
        instanSortPart(gameManager.Instance.playerinfo.MagicPartDick, gameManager.Instance.playerinfo.playerKnapsack);
        base.EnterInit(secondInfo);
    }
    public override void Exit()
    {
        base.Exit();
        Destroy(gameObject);
    }
    public void instanSortPart(List<MagicPart> magicParts, knapsack _knapsack)
    {
        //外部生成还没有安装的部件
        int j = 0;
        for (int i = 0; i < magicParts.Count; i++)
        {
            if (!_knapsack.installParts.ContainsValue(magicParts[i]))
            {
                GameObject part = Instantiate(partGO, partPosition);
                part.transform.localPosition = Vector3.right * partDistance * j;
                realpart rp = part.GetComponent<realpart>();
                rp.Init(magicParts[i], RealPartState.Sort, partPosition);
                rp.meshTran.gameObject.SetActive(true);
                j++;
            }
        }
        //把背包生成
        GameObject knapscak = Instantiate(knapsackGO, knapsackPosition);
        realKnapsack rk = knapscak.GetComponent<realKnapsack>();
        rk.Init(_knapsack, GameState.MapSence);
        //生成安装了的部件
        foreach (var i in _knapsack.installParts)
        {
            GameObject part = Instantiate(partGO, rk.laticePairs[i.Key].transform.parent);
            realpart rp = part.GetComponent<realpart>();
            rp.meshTran.gameObject.SetActive(false);
            rp.InitInstall(rk.laticePairs[i.Key].transform);
            //lastRealLatice.InstallPart(thisMagicPart, out installPosiTran);
            rp.Init(i.Value, RealPartState.Sort, partPosition);
        }

    }
}
