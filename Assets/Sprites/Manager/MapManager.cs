using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AllAsset;

public enum MapState
{
    MainMap,        //在主地图页面，主要可选行为：选择路径点
    EventWindow,    //在事件子窗口上
}


public class MapManager : MonoBehaviour
{
    public MapState mapState = MapState.MainMap;
    public List<realPlace> realplaceList = new List<realPlace>();
    //public Dictionary<Vector2, PlaceNode> placeNodeDic = new Dictionary<Vector2, PlaceNode>();
    private MapRootInfo maprootinfo;

    //地点平均距离
    public Vector2 placedistanceRange;
    //总层数
    public int allStoreyNum = 12;
    //每层的地点数（初设）
    public int placeNumPerStorey = 4;
    public int minPlaceNumPerStorey = 2;
    //新增路线地点数范围
    public Vector2 outPlaceNumRange;
    //删除节点数范围
    public Vector2 deleteNodeNumRange;
    //添加路线数范围
    public Vector2 addLinklineNumRange;
    //事件战斗比例(百分比)
    public float befallBattlePrent;
    //精英怪层
    public int hardEnemyStorey = 5;
    //宝箱层
    public int treasureStorey = 7;
    public int hardEnemyExtraNum = 2;

    Dictionary<int, List<PlaceNode>> intListDic = new Dictionary<int, List<PlaceNode>>();
    List<PlaceNode> placeNodeList = new List<PlaceNode>();
    Dictionary<Vector2, PlaceNode> placeNodeDic = new Dictionary<Vector2, PlaceNode>();

    public void InitMap()
    {
        Debug.Log("init map");
        maprootinfo = gameManager.Instance.instantiatemanager.mapRootInfo;

        place newplace;
        //战斗地点
        newplace = new battlePlace(new monInfo_Cat(), 3);      
        realplaceList.Add(instantiatePlace(newplace));
        //事件地点
        secondBoardInfo secondboard = new secondBoardInfo(0,"部件配置");
        //befallinfo secondbefall = new befallinfo("整装待发", -1, null, new Button_OverSortPart());
        befallinfo newbefallinfo = new befallinfo("整装待发", 0, "英雄征途的第一步：整理背包",
            new Button_ExitBefall("直接出发"),new Button_SecondBoard(secondboard));
        newplace = new befallPlace(newbefallinfo);
        realplaceList.Add(instantiatePlace(newplace));
        //商店地点
        newplace = new shopPlace();
        realplaceList.Add(instantiatePlace(newplace));
        /////////////////////////////分割线//////////////////////////////


        PlaceNode startPlaceNode = new PlaceNode(new startPlace(), new Vector2(2.5f, 0));
        intListDic.Add(0, new List<PlaceNode>() { startPlaceNode });
        placeNodeDic.Add(startPlaceNode.PointPosi, startPlaceNode);
        PlaceNode endPlaceNode = new PlaceNode(new battlePlace(new monInfo_Cat(), 0,3), new Vector2(2.5f, allStoreyNum));
        intListDic.Add(allStoreyNum, new List<PlaceNode>() { endPlaceNode });
        placeNodeDic.Add(endPlaceNode.PointPosi, endPlaceNode);
        //普通战斗
        place battleplace = new battlePlace(new monInfo_Cat(), 3,1);
        //普通事件
        place befallplace = new befallPlace(new befallinfo("普通事件", 0, "普通的事件"));
        //空白
        place spaceplace = new spacePlace();
        for(int storey = 1; storey < allStoreyNum; storey++)
        {
            List<PlaceNode> list = new List<PlaceNode>();
            intListDic.Add(storey, list);
            for(int order = 1; order <= placeNumPerStorey; order++)
            {
                PlaceNode placeNode = new PlaceNode(spaceplace, new Vector2(order, storey));
                placeNodeDic.Add(new Vector2(order, storey), placeNode);
                list.Add(placeNode);
                placeNodeList.Add(placeNode);
            }
        }
        //连线
        //start
        for (int order = 1; order <= placeNumPerStorey; order++)
        {
            startPlaceNode.LinkNode(placeNodeDic[new Vector2(order, 1)]);
        }
        for (int storey = 1; storey < allStoreyNum - 1; storey++)
        {
            for (int order = 1; order <= placeNumPerStorey; order++)
            {
                placeNodeDic[new Vector2(order, storey)].LinkNode(placeNodeDic[new Vector2(order, storey + 1)]);
            }
        }
        for (int order = 1; order <= placeNumPerStorey; order++)
        {
            placeNodeDic[new Vector2(order, allStoreyNum - 1)].LinkNode(endPlaceNode);
        }

        //改变节点
        //添加边路
        int outplacenum = (int)Random.Range(outPlaceNumRange.x, outPlaceNumRange.y+1);
        //左边
        int startstorey = (int)Random.Range(2, allStoreyNum - 2 - outplacenum);
        for(int i = 0; i < outplacenum;i++)
        {
            Vector2 posi=new Vector2(0, startstorey +i);
            PlaceNode placeNode = new PlaceNode(spaceplace, posi);
            placeNodeDic.Add(posi, placeNode);
            intListDic[startstorey + i].Add(placeNode);
            placeNodeList.Add(placeNode);
        }
        //链接
        PlaceNode outStartNode = placeNodeDic[new Vector2(0, startstorey)];
        PlaceNode outEndNode = placeNodeDic[new Vector2(0, startstorey + outplacenum - 1)];
        placeNodeDic[new Vector2(1, startstorey - 1)].LinkNode(outStartNode);
        outEndNode.LinkNode(placeNodeDic[new Vector2(1, startstorey + outplacenum)]);
        for(int i = 0; i < outplacenum - 1; i++)
        {
            Vector2 posi = new Vector2(0, startstorey + i);
            placeNodeDic[posi].LinkNode(placeNodeDic[posi + Vector2.up]);
        }
        //右边
        int lastoutnum = outplacenum;
        outplacenum = (int)Random.Range(outPlaceNumRange.x, outPlaceNumRange.y + 1);
        if (startstorey + lastoutnum / 2 < allStoreyNum / 2)
        {
            startstorey = (int)Random.Range(startstorey+lastoutnum+2, allStoreyNum - 2 - outplacenum);
        }
        else
        {
            startstorey = (int)Random.Range(2, startstorey-2);
        }
        for (int i = 0; i < outplacenum; i++)
        {
            Vector2 posi = new Vector2(placeNumPerStorey+1, startstorey + i);
            PlaceNode placeNode = new PlaceNode(spaceplace, posi);
            placeNodeDic.Add(posi, placeNode);
            intListDic[startstorey + i].Add(placeNode);
            placeNodeList.Add(placeNode);
        }
        //链接
        outStartNode = placeNodeDic[new Vector2(placeNumPerStorey + 1, startstorey)];
        outEndNode = placeNodeDic[new Vector2(placeNumPerStorey + 1, startstorey + outplacenum - 1)];
        placeNodeDic[new Vector2(placeNumPerStorey, startstorey - 1)].LinkNode(outStartNode);
        outEndNode.LinkNode(placeNodeDic[new Vector2(placeNumPerStorey, startstorey + outplacenum)]);
        for (int i = 0; i < outplacenum - 1; i++)
        {
            Vector2 posi = new Vector2(placeNumPerStorey + 1, startstorey + i);
            placeNodeDic[posi].LinkNode(placeNodeDic[posi + Vector2.up]);
        }
        //添加路线
        int addLinklineNun = (int)Random.Range(addLinklineNumRange.x, addLinklineNumRange.y + 1);
        for (int i = 0; i < addLinklineNun;)
        {
            PlaceNode toaddLineNode = ListOperation.RandomValue<PlaceNode>(placeNodeList);
            if (CanAddLinkline(toaddLineNode))
            {
                List<Vector2> tolinkVectors = new List<Vector2>
                {
                    toaddLineNode.PointPosi+new Vector2(-1,1),
                    toaddLineNode.PointPosi+new Vector2(1,1),
                };
                Vector2 tolinkV = new Vector2(1, 1);
                bool whilet = false;
                do
                {
                    if (tolinkVectors.Count == 0)
                    {
                        break;
                    }
                    tolinkV = ListOperation.RandomValue<Vector2>(tolinkVectors);
                    tolinkVectors.Remove(tolinkV);
                    if (placeNodeDic.ContainsKey(tolinkV))
                    {
                        //已经连接了，不行
                        if (toaddLineNode.nextNodeList.Contains(placeNodeDic[tolinkV]))
                        {
                            whilet = false;
                        }
                        else
                        {
                            //准备连接的地点
                            if (placeNodeDic[tolinkV].lastNodeList.Count >= 2)
                            {
                                whilet = false;
                            }
                            else
                            {
                                if (placeNodeDic.ContainsKey(new Vector2(tolinkV.x, toaddLineNode.PointPosi.y)) && placeNodeDic.ContainsKey(new Vector2(toaddLineNode.PointPosi.x, tolinkV.y)))
                                {
                                    if (placeNodeDic[new Vector2(tolinkV.x, toaddLineNode.PointPosi.y)].nextNodeList.Contains(placeNodeDic[new Vector2(toaddLineNode.PointPosi.x, tolinkV.y)]))
                                    {
                                        whilet = false;
                                    }
                                    else
                                    {
                                        whilet = true;
                                    }
                                }
                                else
                                {
                                    whilet = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        whilet = false;
                    }
                }
                while (!whilet);

                toaddLineNode.LinkNode(placeNodeDic[tolinkV]);
                i++;
            }
        }
        //删除节点
        int deleteNodeNum = (int)Random.Range(deleteNodeNumRange.x, deleteNodeNumRange.y + 1);
        for(int i = 0; i < deleteNodeNum;)
        {
            PlaceNode todeleteNode = ListOperation.RandomValue<PlaceNode>(placeNodeList);

            if (CanDeleteNode(todeleteNode))                                 
            {
                //之前
                foreach(PlaceNode lastnode in todeleteNode.lastNodeList)
                {
                    lastnode.nextNodeList.Remove(todeleteNode);
                    if (lastnode.nextNodeList.Count == 0)
                    {
                        List<Vector2> nextvectors = new List<Vector2>();
                        nextvectors.Add(lastnode.PointPosi + new Vector2(-1, 1));
                        nextvectors.Add(lastnode.PointPosi + new Vector2(0, 1));
                        nextvectors.Add(lastnode.PointPosi + new Vector2(1, 1));
                        nextvectors.Remove(todeleteNode.PointPosi);
                        Vector2 tolinknextV=new Vector2(1,1);
                        do
                        {
                            if (nextvectors.Count == 0)
                            {
                                break;
                            }
                            tolinknextV = ListOperation.RandomValue<Vector2>(nextvectors);
                            nextvectors.Remove(tolinknextV);
                        }
                        while (!placeNodeDic.ContainsKey(tolinknextV));
                        lastnode.LinkNode(placeNodeDic[tolinknextV]);
                    }
                }
                //之后
                Debug.Log(todeleteNode.nextNodeList.Count);
                foreach(PlaceNode nextnode in todeleteNode.nextNodeList)
                {
                    nextnode.lastNodeList.Remove(todeleteNode);
                    if (nextnode.lastNodeList.Count == 0)
                    {
                        List<Vector2> lastvectors = new List<Vector2>();
                        lastvectors.Add(nextnode.PointPosi + new Vector2(-1, -1));
                        lastvectors.Add(nextnode.PointPosi + new Vector2(0, -1));
                        lastvectors.Add(nextnode.PointPosi + new Vector2(1, -1));
                        lastvectors.Remove(todeleteNode.PointPosi);
                        Vector2 tolinklastV = new Vector2(1, 1);
                        do
                        {
                            if (lastvectors.Count == 0)
                            {
                                break;
                            }
                            tolinklastV = ListOperation.RandomValue<Vector2>(lastvectors);
                            //Debug.Log(nextnode.PointPosi+""+(tolinklastV- nextnode.PointPosi));
                            lastvectors.Remove(tolinklastV);
                        }
                        while (!placeNodeDic.ContainsKey(tolinklastV));
                        placeNodeDic[tolinklastV].LinkNode(nextnode);
                    }

                }
                placeNodeDic.Remove(todeleteNode.PointPosi);
                intListDic[(int)todeleteNode.PointPosi.y].Remove(todeleteNode);
                i++;
            }
        }

        foreach (var intlist in intListDic)
        {
            intlist.Value.Sort();
        }
        //生成节点        
        foreach (var vp in placeNodeDic)
        {
            instantiatePlace(vp.Value);
        }
        //生成路线        
        foreach (var vp in placeNodeDic)
        {
            foreach (PlaceNode placeNode in vp.Value.nextNodeList)
            {
                GameObject line = Instantiate(
                    gameManager.Instance.instantiatemanager.loadGO,
                    vp.Value.realplaceTran);
                LineRenderer linerender = line.GetComponent<LineRenderer>();
                linerender.SetPosition(0, new Vector3(vp.Value.realplaceTran.position.x, vp.Value.realplaceTran.position.y, -0.5f));
                linerender.SetPosition(1, new Vector3(placeNode.realplaceTran.position.x, placeNode.realplaceTran.position.y, -0.5f));         
            }
        }


    }
    bool CanDeleteNode(PlaceNode placeNode)
    {
        //不是新加得边线才可以删
        if (placeNode.PointPosi.x == 0 || placeNode.PointPosi.x == placeNumPerStorey + 1)
        {
            return false;
        }
        //太靠前或太靠后
        if(placeNode.PointPosi.y < 3 || placeNode.PointPosi.y > allStoreyNum - 2)
        {
            return false;
        }
        //每层地点数量
        if (intListDic[(int)placeNode.PointPosi.y].Count <= minPlaceNumPerStorey)
        {
            return false;
        }
        //不能是外边得起始点或终止点
        foreach(PlaceNode node in placeNode.nextNodeList)
        {
            if (node.PointPosi.x == 0 | node.PointPosi.x == placeNumPerStorey + 1)
            {
                return false;
            }
        }
        foreach(PlaceNode node in placeNode.lastNodeList)
        {
            if(node.PointPosi.x == 0 | node.PointPosi.x == placeNumPerStorey + 1)
            {
                return false;
            }
        }
        //前后不能已经有删掉的点
        if (!placeNodeDic.ContainsKey(placeNode.PointPosi + new Vector2(0, 1))){
            return false;
        }
        if (!placeNodeDic.ContainsKey(placeNode.PointPosi + new Vector2(0, -1))){
            return false;
        }

        return true;
    }
    bool CanAddLinkline(PlaceNode placeNode)
    {
        //开始和结尾就不添加了
        if (placeNode.PointPosi.y == 1 || placeNode.PointPosi.y == allStoreyNum - 1)
        {
            return false;
        }
        //已经有两条线以上的就不添加了
        if (placeNode.nextNodeList.Count >= 2)
        {
            return false;
        }
        //前路没有可链接的点了
        int nextnodeNum = 0;
        //if(placeNodeDic.ContainsKey(placeNode.PointPosi+new Vector2(0, 1)))
        //{
        //    nextnodeNum++;
        //}
        if (placeNodeDic.ContainsKey(placeNode.PointPosi + new Vector2(-1, 1)))
        {
            nextnodeNum++;
        }
        if (placeNodeDic.ContainsKey(placeNode.PointPosi + new Vector2(1, 1)))
        {
            nextnodeNum++;
        }
        if (nextnodeNum == 1)
        {
            return false;
        }
        return true;
    }
    //地点位置数据
    public List<Vector3> vector3list = new List<Vector3>() { new Vector3(7.44f, 4.11f), new Vector3(0.28f, -1.18f), new Vector3(-8.67f, -3.65f), new Vector3(1.01f, -8.83f) ,new Vector3(-13.68f, 4.02f),new Vector3(-3.61f, 5.29f) };

    //实例生成地点
    private realPlace instantiatePlace(place place)
    {
        Vector3 vec = ListOperation.RandomValue<Vector3>(vector3list);
        GameObject placego = Instantiate(
            gameManager.Instance.instantiatemanager.placeGO,
            new Vector3(vec.x,vec.y, maprootinfo.placeBeginPosi.position.z),
            Quaternion.identity,
            maprootinfo.placefolder
        );
        
        vector3list.Remove(vec);
        realPlace result = placego.GetComponent<realPlace>();
        result.Init(place);
        

        return result;
    }
    private realPlace instantiatePlace(PlaceNode placenode)
    {
        float x = Random.Range(-0.2f, 0.2f);
        float y = Random.Range(-0.2f, 0.2f);
        //float x = 0;
        //float y = 0;
        int placenumper = intListDic[(int)placenode.PointPosi.y].Count;
        int placenum = 0;
        foreach(PlaceNode pn in intListDic[(int)placenode.PointPosi.y])
        {
            if (pn == placenode)
            {
                break;
            }
            placenum++;
        }
        float leftstart = -(float)(placenumper - 1) / 2;
        float pointx = leftstart + placenum;
        GameObject placego = Instantiate(
            gameManager.Instance.instantiatemanager.placeGO,
            new Vector3(
                maprootinfo.placeBeginPosi.position.x+ (pointx + x)* Random.Range(placedistanceRange.x, placedistanceRange.y),
                maprootinfo.placeBeginPosi.position.y+ (placenode.PointPosi.y + y)*placedistanceRange.y,
                maprootinfo.placeBeginPosi.position.z
            ) ,//maprootinfo.placeBeginPosi.position + new Vector3(pointx+x,placenode.PointPosi.y+y)*Random.Range(placedistanceRange.x,placedistanceRange.y),
            Quaternion.identity,
            maprootinfo.placefolder
        );
        realPlace result = placego.GetComponent<realPlace>();
        placenode.realplaceTran = result.transform;
        result.Init(placenode.thisplace);
        return result;
    }
    //private void 
    public void EnterBattle(battlePlace battle)
    {
        AsyncOperation _asyncOperation = SceneManager.LoadSceneAsync(AllAsset.MapAsset.GetSceneStr(battle.sceneId),LoadSceneMode.Additive);
        gameManager.Instance.battleScene = SceneManager.GetSceneByName(AllAsset.MapAsset.GetSceneStr(battle.sceneId));
        StartCoroutine(IEenterBattle(_asyncOperation, battle.monsterInfo));
    }
    IEnumerator IEenterBattle(AsyncOperation asyncOperation,monsterInfo monster)
    {
        yield return new WaitUntil(() => {
            return asyncOperation.isDone;
        });
        gameManager.Instance.battleManagerInit();
        gameManager.Instance.SwitchScene(true);
        gameManager.Instance.battlemanager.InitBattlemanaget();
        gameManager.Instance.battlemanager.BattleStartEnemySet(monster);
        gameManager.Instance.battlemanager.startBattale();
    }
}
