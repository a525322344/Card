using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class realKnapsack : MonoBehaviour
{
    //创建5X5预备背包格，初始开放6格，中心点为3，3
    public GameObject LaticeGO;
    public Transform pointtran;
    public float distance;

    public knapsack thisknapsack;
    public GameState KnapsackState;

    public Dictionary<Vector2, realLatice> laticePairs = new Dictionary<Vector2, realLatice>();
#region 用于中转
    private Dictionary<Vector2, latice> lactices = new Dictionary<Vector2, latice>();
#endregion


    public void Init(knapsack _knapsack,GameState state)
    {
        KnapsackState = state;
        thisknapsack = _knapsack;
        if (KnapsackState == GameState.MapSence)
        {
            //生成实例Latice
            for (int i = 0; i < 25; i++)
            {
                //0 1 2 3 4
                int posx = i % 5;
                int posy = i / 5;
                Vector2 posi = new Vector2(posx, posy);
                latice newLatice = new latice(posi, thisknapsack.isexploits[i]);
                lactices.Add(posi, newLatice);
            }
            foreach (var pl in lactices)
            {
                GameObject realLaticeGO = Instantiate(LaticeGO, pointtran);
                //realLaticeGO.name = "realLatice";
                Vector2 posi = pl.Key;
                realLaticeGO.transform.localPosition = new Vector3(posi.x - 2, posi.y - 2, 0) * distance;
                realLatice realLatice = realLaticeGO.GetComponent<realLatice>();
                realLatice.Init(pl.Value, this,GameState.MapSence);

                laticePairs.Add(posi, realLatice);
            }
            //初始化部件数据
            InitInstallPart();
        }
        else if (KnapsackState == GameState.BattleSence)
        {
            //生成实例Latice
            for (int i = 0; i < 25; i++)
            {
                //0 1 2 3 4
                int posx = i % 5;
                int posy = i / 5;
                Vector2 posi = new Vector2(posx, posy);
                latice newLatice = new latice(posi, thisknapsack.isexploits[i]);
                lactices.Add(posi, newLatice);
            }
            foreach (var pl in lactices)
            {
                GameObject realLaticeGO = Instantiate(LaticeGO, pointtran);
                //realLaticeGO.name = "realLatice";
                Vector2 posi = pl.Key;
                realLaticeGO.transform.localPosition = new Vector3(posi.x - 2, posi.y - 2, 0) * distance;
                realLatice realLatice = realLaticeGO.GetComponent<realLatice>();
                realLatice.Init(pl.Value, this,GameState.BattleSence);

                laticePairs.Add(posi, realLatice);
            }
            //初始化部件
        }
    }
    private void InitInstallPart()
    {
        foreach (var centerPart in thisknapsack.installParts)
        {
            foreach (var vecGrid in thisknapsack.installParts[centerPart.Key].Vector2GridRotate)
            {
                if (vecGrid.Value.Opening)
                {
                    laticePairs[vecGrid.Key + centerPart.Key].grid = vecGrid.Value;
                    laticePairs[vecGrid.Key + centerPart.Key].thislatice.state = LaticeState.Install;
                }
            }
        }
    }


    private List<realLatice> onLatices;
    public bool CanInstallPart(MagicPart magicPart,Vector2 center)
    {
        Debug.Log("Can");
        onLatices = new List<realLatice>();
        bool result = true;
        foreach(var vg in magicPart.Vector2GridRotate)
        {
            if (vg.Value.Opening)
            {
                if (laticePairs.ContainsKey(center + vg.Key))
                {
                    realLatice rl = laticePairs[center + vg.Key];
                    if (rl.thislatice.state != LaticeState.Exploit)
                    {
                        result = false;
                        break;
                    }
                    else
                    {
                        onLatices.Add(rl);
                    }
                }
                else
                {
                    result = false;
                    break;
                }
            }
        }
        //设置反应颜色
        if (result)
        {
            foreach(realLatice rl in onLatices)
            {
                rl.thislatice.state = LaticeState.CanInstall;
                rl.changeColor();
            }
        }
        return result;
    }
    public void ExitCanInstall()
    {
        Debug.Log("ExitCanInstall");

        foreach (realLatice rl in onLatices)
        {
            rl.thislatice.state = LaticeState.Exploit;
            rl.changeColor();
        }
    }
    public void ExitInstall(Vector2 center)
    {
        Debug.Log("ExitInstall");
        foreach(var g in thisknapsack.installParts[center].Vector2GridRotate)
        {
            if (g.Value.Opening)
            {
                laticePairs[g.Key + center].grid = null;
                laticePairs[g.Key + center].thislatice.state = LaticeState.Exploit;
            }
        }
        thisknapsack.installParts.Remove(center);
    }
    public void InstallPart(MagicPart magicPart,Vector2 center,out Transform positionTran)
    {
        Debug.Log("Install");
        ExitCanInstall();
        
        Dictionary<realLatice, grid> rLGPairs = new Dictionary<realLatice, grid>();
        bool caninstall = true;
        //检测能否安装
        foreach(var vg in magicPart.Vector2GridRotate)
        {
            if (vg.Value.Opening)
            {
                if (laticePairs.ContainsKey(center + vg.Key))
                {
                    realLatice rl = laticePairs[center + vg.Key];
                    if (rl.thislatice.state != LaticeState.Exploit)
                    {
                        caninstall = false;
                        break;
                    }
                    else//可以装
                    {
                        rLGPairs.Add(rl, vg.Value);
                    }
                }
                else
                {
                    caninstall = false;
                    break;
                }
            }
        }
        if (caninstall)
        {
            thisknapsack.installParts.Add(center, magicPart);
            foreach (var rlg in rLGPairs)
            {
                //将grid储存在对应的latice上
                rlg.Key.grid = rlg.Value;
                //设置状态
                rlg.Key.thislatice.state = LaticeState.Install;
            }
            positionTran = laticePairs[center].transform;
        }
        else
        {
            Debug.Log("错误：部件安装失败");
            positionTran = null;
        }
    }

}
