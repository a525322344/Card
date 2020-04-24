using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class realKnapsack : MonoBehaviour
{
    //创建5X5预备背包格，初始开放6格，中心点为3，3
    public GameObject LaticeGO;
    public Transform pointtran;
    public GameObject meshFolder;
    //public Transform BoardMesh;
    public Transform overcubeFolder;
    public float distance;

    public knapsack thisknapsack;
    public GameState KnapsackState;

    public Dictionary<Vector2, realLatice> laticePairs = new Dictionary<Vector2, realLatice>();
    public Dictionary<Vector2, realLatice> usedLaticePairs = new Dictionary<Vector2, realLatice>();
    public List<realpart> realparts = new List<realpart>();
    public List<Transform> overcubes = new List<Transform>();






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
            foreach (KeyValuePair<Vector2,latice> pl in lactices)
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
            InitInstallPart(GameState.MapSence);
        }
        else if (KnapsackState == GameState.BattleSence)
        {
            meshFolder.gameObject.SetActive(false);
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

            Debug.Log(usedLaticePairs.Count);
            //初始化部件
            InitInstallPart(GameState.BattleSence);

            foreach (var pl in laticePairs)
            {
                if (pl.Value.thislatice.state == LaticeState.Install)
                {
                    usedLaticePairs.Add(pl.Key, pl.Value);
                }
            }
            selectPart = nullpart;
        }

        for(int i = 0; i < thisknapsack.isexploits.Length; i++)
        {
            overcubes.Add(overcubeFolder.GetChild(i));
            overcubes[i].gameObject.SetActive(!thisknapsack.isexploits[i]);
        }
    }
    private void InitInstallPart(GameState state)
    {
        if (state == GameState.MapSence)
        {
            foreach (var centerPart in thisknapsack.installParts)
            {
                foreach (var vecGrid in thisknapsack.installParts[centerPart.Key].Vector2GridRotate)
                {
                    if (vecGrid.Value.Opening)
                    {
                        //laticePairs[vecGrid.Key + centerPart.Key].grid = vecGrid.Value;
                        laticePairs[vecGrid.Key + centerPart.Key].thislatice.state = LaticeState.Install;
                        
                    }
                }
            }
        }
        else if (state == GameState.BattleSence)
        {
            foreach (var centerPart in thisknapsack.installParts)
            {
                foreach (var vecGrid in thisknapsack.installParts[centerPart.Key].Vector2GridRotate)
                {
                    if (vecGrid.Value.Opening)
                    {
                        //laticePairs[vecGrid.Key + centerPart.Key].grid = vecGrid.Value;
                        //laticePairs[vecGrid.Key+centerPart.Key].realpart=
                        laticePairs[vecGrid.Key + centerPart.Key].thislatice.state = LaticeState.Install;
                        laticePairs[vecGrid.Key + centerPart.Key].Init(laticePairs[vecGrid.Key + centerPart.Key].thislatice, this, GameState.BattleSence);
                    }
                }
            }
            //生成安装了的部件
            instantiateManager instantiate = gameManager.Instance.instantiatemanager;
            foreach (var centerPart in thisknapsack.installParts)
            {
                GameObject part = Instantiate(instantiate.partGO, laticePairs[centerPart.Key].transform);
                realpart rp = part.GetComponent<realpart>();
                rp.meshTran.gameObject.SetActive(false);
                //rp.InitInstall(rk.laticePairs[centerPart.Key].transform);
                //lastRealLatice.InstallPart(thisMagicPart, out installPosiTran);
                rp.Init(centerPart.Value, GameState.BattleSence,null);
                realparts.Add(rp);
                foreach (var vecGrid in thisknapsack.installParts[centerPart.Key].Vector2GridRotate)
                {
                    if (vecGrid.Value.Opening)
                    {
                        laticePairs[vecGrid.Key + centerPart.Key].realpart = rp;
                        laticePairs[vecGrid.Key + centerPart.Key].realgrid = rp.gridRealgridPairs[vecGrid.Value];
                        laticePairs[vecGrid.Key + centerPart.Key].thislatice.state = LaticeState.Install;
                        laticePairs[vecGrid.Key + centerPart.Key].Init(laticePairs[vecGrid.Key + centerPart.Key].thislatice, this, GameState.BattleSence);
                    }
                }
            }
        }
    }
    private void Update()
    {
        switch (KnapsackState)
        {
            case GameState.BattleSence:
                if (b_readyToPlayCard)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        b_readyToPlayCard = false;
                        gameManager.Instance.battlemanager.battleInfo.currentPos = currentPos;
                        gameManager.Instance.battlemanager.PlayCard();
                    }
                }
                break;
        }
    }
    //地图操作
    private List<realLatice> onLatices;
    public bool CanInstallPart(MagicPart magicPart,Vector2 center)
    {
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
                //laticePairs[g.Key + center].grid = null;
                laticePairs[g.Key + center].thislatice.state = LaticeState.Exploit;
            }
        }
        thisknapsack.installParts.Remove(center);
    }
    public void InstallPart(MagicPart magicPart,Vector2 center,out Transform positionTran)
    {
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
                //rlg.Key.grid = rlg.Value;
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



    //战斗操作
    public bool b_readyToPlayCard;
    public card selectCard;
    MagicPart nullpart = new MagicPart();
    public MagicPart selectPart;
    List<realLatice> selectLatices = new List<realLatice>();
    List<realpart> selecRealParts = new List<realpart>();

    //当前打出卡牌位置信息
    public List<Vector2> currentPos = new List<Vector2>();
    //可用位置信息
    //public List<Vector2> canUsePos = new List<Vector2>();


    public void SetinPart()
    {
        foreach(var v in thisknapsack.installParts)
        {
            v.Value.SetinReactions();
        }
    }


    public bool CanCostPlay(Vector2 center, Dictionary<Vector2, int> vectorInts)
    {
        //储存费用坐标
        currentPos.Clear();
        foreach(var vi in vectorInts)
        {
            if (vi.Value == 1)
            {
                currentPos.Add(center + vi.Key);
            }
        }

        selectLatices.Clear();
        selecRealParts.Clear();
        bool result = true;
        bool b_samePart = true;
        int cost=0;
        //selectPart = laticePairs[center].realpart.thisMagicPart;
        foreach(var vecint in vectorInts)
        {
            cost += vecint.Value;
            if (vecint.Value == 1)//cost块符合
            {
                Vector2 position = center + vecint.Key;
                if (usedLaticePairs.ContainsKey(position))//纸板上有
                {
                    if (usedLaticePairs[position].gridState == GridState.Power | usedLaticePairs[position].gridState == GridState.Can)
                    {
                        //储存用到的latice
                        selectLatices.Add(usedLaticePairs[position]);
                        //储存部件
                        if (b_samePart)
                        {
                            //第一次
                            if (selectPart == nullpart)
                            {
                                if (usedLaticePairs[position].realpart.thisMagicPart != null)
                                {
                                    selectPart = usedLaticePairs[position].realpart.thisMagicPart;
                                    if (!selecRealParts.Contains(usedLaticePairs[position].realpart))
                                    {
                                        selecRealParts.Add(usedLaticePairs[position].realpart);
                                    }
                                }
                                else
                                {
                                    selectPart = nullpart;
                                    b_samePart = false;
                                    selecRealParts.Clear();
                                }
                            }
                            else
                            {
                                if (selectPart != usedLaticePairs[position].realpart.thisMagicPart)
                                {
                                    selectPart = nullpart;
                                    b_samePart = false;
                                    selecRealParts.Clear();
                                }
                                else
                                {
                                    if (!selecRealParts.Contains(usedLaticePairs[position].realpart))
                                    {
                                        selecRealParts.Add(usedLaticePairs[position].realpart);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        result = false;
                        break;
                    }
                }
                else
                {
                    result = false;
                    break;
                }
            }
        }
        //额外判断零费牌触发的部件效果
        
        if (cost == 0)
        {
            if (usedLaticePairs[center].gridState == GridState.Power | usedLaticePairs[center].gridState == GridState.Can| usedLaticePairs[center].gridState == GridState.Used)
            {
                if (usedLaticePairs[center].realpart.thisMagicPart != null)
                {
                    selectPart = usedLaticePairs[center].realpart.thisMagicPart;
                    selecRealParts.Add(usedLaticePairs[center].realpart);
                }
            }
        }
        b_readyToPlayCard = result;
        return result;
    }


    public void ToSetPart(card _selectcard)
    {
        if (_selectcard == null)
        {
            for (int i = 0; i < selectLatices.Count; i++)
            {
                selectLatices[i].BackSetLitice(false);
            }
            selectLatices.Clear();
            b_readyToPlayCard = false;
            selectCard = null;
            selectPart = nullpart;
            foreach (realpart rp in selecRealParts)
            {
                rp.OutlineShow(0);
            }
            gameManager.Instance.battlemanager.SetSelectPart(null);
        }
        else
        {
            for(int i = 0; i < selectLatices.Count; i++)
            {
                selectLatices[i].BackSetLitice(true);
            }
            b_readyToPlayCard = true;
            selectCard = _selectcard;

            gameManager.Instance.battlemanager.SetSelectPart(selectPart);
            foreach(realpart rp in selecRealParts)
            {
                rp.OutlineShow(1);
            }
            //部件的激活与睡眠转移到了CardEvent中
        }
    }


    public void UseSelectLatices()
    {
        for (int i = 0; i < selectLatices.Count; i++)
        {
            selectLatices[i].changeColor(GridState.Used);
            selectLatices[i].realgrid.changeMaterial(GridState.Used);
        }
    }
    public void PowerLatices()
    {
        foreach(var v in laticePairs)
        {
            if (v.Value.gridState == GridState.Used || v.Value.gridState == GridState.Power)
            {
                v.Value.changeColor(GridState.Power);
                v.Value.realgrid.changeMaterial(GridState.Power);
            }
        }
    }
}
