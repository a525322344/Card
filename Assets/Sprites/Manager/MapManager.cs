using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AllAsset;
using DG.Tweening;

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
    public bool MapPlaceOpen;
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
    public Vector2 hardEnemyNumRange;
    public int perLineMaxHardEnemyNum;
    public float sleepCloseHardEnemyPresent;
    //商店
    public Vector2 shopNumRange;
    public int perLineMaxShopNum;
    //休息
    public Vector2 sleepNumRange;
    public int perSleepMaxNum;
    //宝箱层
    public int treasureStorey = 7;
    public int hardEnemyExtraNum = 2;

    Dictionary<int, List<PlaceNode>> intListDic = new Dictionary<int, List<PlaceNode>>();
    List<PlaceNode> placeNodeList = new List<PlaceNode>();
    Dictionary<Vector2, PlaceNode> placeNodeDic = new Dictionary<Vector2, PlaceNode>();

    float positionZ;
    public float positionUp;
    public float positionDown;
    public float mapCameraMoveSpeed;
    public void Update()
    {
        if (maprootinfo)
        {
            if (gameManager.Instance.gameState == GameState.MapSence && mapState == MapState.MainMap)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0)//下
                {
                    positionZ -= mapCameraMoveSpeed;
                    if (positionZ < positionDown)
                    {
                        positionZ = positionDown;
                    }
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0)//上
                {
                    positionZ += mapCameraMoveSpeed;
                    if (positionZ > positionUp)
                    {
                        positionZ = positionUp;
                    }
                }
                maprootinfo.cameraMove.DOLocalMoveY(positionZ, 0.7f);
            }
        }
    }

    public void InitMap()
    {
        Debug.Log("init map");
        maprootinfo = gameManager.Instance.instantiatemanager.mapRootInfo;
        positionZ = maprootinfo.cameraMove.localPosition.y;
        /////////////////////////////分割线//////////////////////////////
        RandomList();
        RandomGenerateMap();
    }
    //生成随机表单
    void RandomList()
    {
        normalEnemyLevel1 = ListOperation.RandomValueList<monsterInfo>(AllAsset.MapAsset.nMonster1s, 3);
        normalEnemyLevel2 = ListOperation.RandomValueList<monsterInfo>(AllAsset.MapAsset.nMonster2s, 3);
        normalEnemyLevel3 = ListOperation.RandomValueList<monsterInfo>(AllAsset.MapAsset.nMonster3s, 3);
        hardEnemyLevel4 = ListOperation.RandomValueList<monsterInfo>(AllAsset.MapAsset.hMonster1s, 2);
        hardEnemyLevel5 = ListOperation.RandomValueList<monsterInfo>(AllAsset.MapAsset.hMonster2s, 2);
        bossList = ListOperation.RandomValueList<monsterInfo>(AllAsset.MapAsset.bossLists, 1);

        befallList = ListOperation.RandomValueList<befallinfo>(AllAsset.MapAsset.AllBefallInfos, 2);
    }
    List<monsterInfo> normalEnemyLevel1 = new List<monsterInfo>();
    List<monsterInfo> normalEnemyLevel2 = new List<monsterInfo>();
    List<monsterInfo> normalEnemyLevel3 = new List<monsterInfo>();
    List<monsterInfo> hardEnemyLevel4 = new List<monsterInfo>();
    List<monsterInfo> hardEnemyLevel5 = new List<monsterInfo>();
    List<monsterInfo> bossList = new List<monsterInfo>();
    //
    List<befallinfo> befallList = new List<befallinfo>();

    public PlaceNode NowPlace;
    public void SetNowPlace(PlaceNode placeNode)
    {
        NowPlace.placeState = PlaceState.Used;
        foreach(PlaceNode p in NowPlace.nextNodeList)
        {
            p.placeState = PlaceState.Cannot;
        }
        placeNode.placeState = PlaceState.NowOn;
        foreach (PlaceNode p in placeNode.nextNodeList)
        {
            p.placeState = PlaceState.ToGo;
        }
    }
    private void RandomGenerateMap()
    {
        PlaceNode startPlaceNode = new PlaceNode(new startPlace(), new Vector2(2.5f, 0));
        intListDic.Add(0, new List<PlaceNode>() { startPlaceNode });
        placeNodeDic.Add(startPlaceNode.PointPosi, startPlaceNode);


        PlaceNode endPlaceNode = new PlaceNode(new battlePlace(3,12,0), new Vector2(2.5f, allStoreyNum));
        intListDic.Add(allStoreyNum, new List<PlaceNode>() { endPlaceNode });
        placeNodeDic.Add(endPlaceNode.PointPosi, endPlaceNode);
        //普通战斗
        place battleplace;
        place befallplace = new befallPlace();
        //空白
        place spaceplace = new spacePlace();
        for (int storey = 1; storey < allStoreyNum; storey++)
        {
            List<PlaceNode> list = new List<PlaceNode>();
            intListDic.Add(storey, list);
            for (int order = 1; order <= placeNumPerStorey; order++)
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
        int outplacenum = (int)Random.Range(outPlaceNumRange.x, outPlaceNumRange.y + 1);
        //左边
        int startstorey = (int)Random.Range(2, allStoreyNum - 2 - outplacenum);
        for (int i = 0; i < outplacenum; i++)
        {
            Vector2 posi = new Vector2(0, startstorey + i);
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
        for (int i = 0; i < outplacenum - 1; i++)
        {
            Vector2 posi = new Vector2(0, startstorey + i);
            placeNodeDic[posi].LinkNode(placeNodeDic[posi + Vector2.up]);
        }
        //右边
        int lastoutnum = outplacenum;
        outplacenum = (int)Random.Range(outPlaceNumRange.x, outPlaceNumRange.y + 1);
        if (startstorey + lastoutnum / 2 < allStoreyNum / 2)
        {
            startstorey = (int)Random.Range(startstorey + lastoutnum + 2, allStoreyNum - 2 - outplacenum);
        }
        else
        {
            startstorey = (int)Random.Range(2, startstorey - 2);
        }
        for (int i = 0; i < outplacenum; i++)
        {
            Vector2 posi = new Vector2(placeNumPerStorey + 1, startstorey + i);
            PlaceNode placeNode = new PlaceNode(spaceplace, posi);
            placeNodeDic.Add(posi, placeNode);
            intListDic[startstorey + i].Add(placeNode);
            placeNodeList.Add(placeNode);
        }
        //链接
        outStartNode = placeNodeDic[new Vector2(placeNumPerStorey + 1, startstorey)];
        outEndNode = placeNodeDic[new Vector2(placeNumPerStorey + 1, startstorey + outplacenum - 1)];
        placeNodeDic[new Vector2(placeNumPerStorey, startstorey - 1)].LinkNode(outStartNode);
        if(placeNodeDic.ContainsKey(new Vector2(placeNumPerStorey, startstorey + outplacenum)))
        {
            outEndNode.LinkNode(placeNodeDic[new Vector2(placeNumPerStorey, startstorey + outplacenum)]);
        }
        else
        {
            Debug.Log("bug");
        }

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
        for (int i = 0; i < deleteNodeNum;)
        {
            PlaceNode todeleteNode = ListOperation.RandomValue<PlaceNode>(placeNodeList);

            if (CanDeleteNode(todeleteNode))
            {
                //之前
                foreach (PlaceNode lastnode in todeleteNode.lastNodeList)
                {
                    lastnode.nextNodeList.Remove(todeleteNode);
                    if (lastnode.nextNodeList.Count == 0)
                    {
                        List<Vector2> nextvectors = new List<Vector2>();
                        nextvectors.Add(lastnode.PointPosi + new Vector2(-1, 1));
                        nextvectors.Add(lastnode.PointPosi + new Vector2(0, 1));
                        nextvectors.Add(lastnode.PointPosi + new Vector2(1, 1));
                        nextvectors.Remove(todeleteNode.PointPosi);
                        Vector2 tolinknextV = new Vector2(1, 1);
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
                foreach (PlaceNode nextnode in todeleteNode.nextNodeList)
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
        //排序，为生成做准备
        foreach (var intlist in intListDic)
        {
            intlist.Value.Sort();
        }
        //依概率生成普通怪物或事件
        foreach (var vp in placeNodeDic)
        {
            if (Random.Range(0, 100) > befallBattlePrent)
            {
                place placebefall = new befallPlace();
                vp.Value.thisplace = placebefall;
            }
            else
            {
                place battlebefall = new battlePlace();
                battleplace = new battlePlace(1, enemyLevelFormStorey(false, (int)vp.Value.PointPosi.y), 0);
                vp.Value.thisplace = battleplace;
            }
        }
        //精英怪
        int hardenemyNum =(int) Random.Range(hardEnemyNumRange.x, hardEnemyNumRange.y + 1);
        for(int i = 0; i < hardenemyNum;)
        {
            PlaceNode nowplacenode = ListOperation.RandomValue<PlaceNode>(placeNodeList);
            if (CanBeHardEnemy(nowplacenode)){
                battleplace = new battlePlace(2, enemyLevelFormStorey(false, (int)nowplacenode.PointPosi.y), 0);
                nowplacenode.thisplace = battleplace;
                //在附近添加火堆
                if (Random.Range(0, 100) < sleepCloseHardEnemyPresent)
                {
                    if (Random.Range(0, 100) > 50)
                    {
                        foreach(PlaceNode p in nowplacenode.nextNodeList)
                        {
                            if (p.thisplace.imageorder == 1 | p.thisplace.imageorder == 4)
                            {
                                p.thisplace = new sleepPlace();
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (PlaceNode p in nowplacenode.lastNodeList)
                        {
                            if (p.thisplace.imageorder == 1 | p.thisplace.imageorder == 4)
                            {
                                p.thisplace = new sleepPlace();
                                break;
                            }
                        }
                    }
                }
                i++;
            }
        }
        //商店
        int shopNum = (int)Random.Range(shopNumRange.x, shopNumRange.y + 1);
        for (int i = 0; i < shopNum;)
        {
            PlaceNode nowplacenode = ListOperation.RandomValue<PlaceNode>(placeNodeList);
            if (CanBeShop(nowplacenode))
            {
                nowplacenode.thisplace = new shopPlace();
                i++;
            }
        }
        //boss前一层是休息
        foreach (PlaceNode placenode in intListDic[allStoreyNum - 1])
        {
            place sleepPlace = new sleepPlace();
            placenode.thisplace = sleepPlace;
        }
        int sleepNum = (int)Random.Range(sleepNumRange.x, sleepNumRange.y + 1);
        for (int i = 0; i < sleepNum;)
        {
            PlaceNode nowplacenode = ListOperation.RandomValue<PlaceNode>(placeNodeList);
            if (CanBeSleep(nowplacenode))
            {
                nowplacenode.thisplace = new sleepPlace();
                i++;
            }
        }
        //第零层是休息
        startPlaceNode.thisplace = new sleepPlace();
        //第一层都是战斗
        foreach (PlaceNode placenode in intListDic[1])
        {
            battleplace = new battlePlace(1, enemyLevelFormStorey(false, (int)placenode.PointPosi.y), 0);

            place Place = battleplace;
            placenode.thisplace = Place;
        }
        //第7层都是宝箱
        foreach (PlaceNode placenode in intListDic[7])
        {
            place treasurePlace = new treasurePlace();
            placenode.thisplace = treasurePlace;
        }

        //最后是boss层
        endPlaceNode.thisplace = new battlePlace(3, 12, 0);
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

        //设置起始点
        NowPlace = startPlaceNode;
        NowPlace.placeState = PlaceState.NowOn;
        foreach(PlaceNode p in NowPlace.nextNodeList)
        {
            p.placeState = PlaceState.ToGo;
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
    bool CanBeHardEnemy(PlaceNode placeNode)
    {
        if (placeNode.PointPosi.y<=2|placeNode.PointPosi.y>=allStoreyNum-3|placeNode.PointPosi.y==7)
        {
            return false;
        }
        if (LinePlaceNum(placeNode, 2) >= perLineMaxHardEnemyNum)
        {
            return false;
        }
        //附近有不行
        foreach(PlaceNode p in placeNode.nextNodeList)
        {
            if (p.thisplace.imageorder == 2)
            {
                return false;
            }
        }
        foreach (PlaceNode p in placeNode.lastNodeList)
        {
            if (p.thisplace.imageorder == 2)
            {
                return false;
            }
        }
        return true;
    }
    bool CanBeShop(PlaceNode placeNode)
    {
        if (placeNode.PointPosi.y <= 1 | placeNode.PointPosi.y >= allStoreyNum - 1 | placeNode.PointPosi.y == 7)
        {
            return false;
        }
        if (LinePlaceNum(placeNode, 5) >= perLineMaxShopNum)
        {
            return false;
        }
        //不会占用
        if (placeNode.thisplace.imageorder == 2)
        {
            return false;
        }
        //附近有相同不行
        foreach (PlaceNode p in placeNode.nextNodeList)
        {
            if (p.thisplace.imageorder ==5)
            {
                return false;
            }
        }
        foreach (PlaceNode p in placeNode.lastNodeList)
        {
            if (p.thisplace.imageorder == 5)
            {
                return false;
            }
        }
        return true;
    }
    bool CanBeSleep(PlaceNode placeNode)
    {
        if (placeNode.PointPosi.y <= 4 | placeNode.PointPosi.y >= allStoreyNum - 2 | placeNode.PointPosi.y == 7)
        {
            return false;
        }
        if (LinePlaceNum(placeNode, 0) >= perSleepMaxNum)
        {
            return false;
        }
        //不会占用
        if (placeNode.thisplace.imageorder == 2| placeNode.thisplace.imageorder==5)
        {
            return false;
        }
        //附近有相同不行
        foreach (PlaceNode p in placeNode.nextNodeList)
        {
            if (p.thisplace.imageorder == 0)
            {
                return false;
            }
        }
        foreach (PlaceNode p in placeNode.lastNodeList)
        {
            if (p.thisplace.imageorder == 0)
            {
                return false;
            }
        }
        return true;
    }
    int LinePlaceNum(PlaceNode placeNode,int kind)
    {
        int lastnum = 0;
        foreach(PlaceNode lastnode in placeNode.lastNodeList)
        {
            int thislinenum = lastPlaceNum(lastnode, kind);
            if (thislinenum > lastnum)
            {
                lastnum = thislinenum;
            }
        }
        int nextnum = 0;
        foreach(PlaceNode nextnode in placeNode.nextNodeList)
        {
            int thislinenum = nextPlaceNum(nextnode, kind);
            if (thislinenum > nextnum)
            {
                nextnum = thislinenum;
            }
        }
        return lastnum + nextnum;
    }
    int lastPlaceNum(PlaceNode placeNode,int kind)
    {
        int thisnum = 0;
        if (placeNode.thisplace.imageorder == kind)
        {
            thisnum = 1;
        }
        int lastnum = 0;
        foreach (PlaceNode lastnode in placeNode.lastNodeList)
        {
            int thislinenum = lastPlaceNum(lastnode, kind);
            if ( thislinenum> lastnum)
            {
                lastnum = thislinenum;
            }
        }
        return thisnum + lastnum;
    }
    int nextPlaceNum(PlaceNode placeNode, int kind)
    {
        int thisnum = 0;
        if (placeNode.thisplace.imageorder == kind)
        {
            thisnum = 1;
        }
        int nextnum = 0;
        foreach (PlaceNode nextnode in placeNode.nextNodeList)
        {
            int thislinenum = nextPlaceNum(nextnode, kind);
            if (thislinenum > nextnum)
            {
                nextnum = thislinenum;
            }
        }
        return thisnum + nextnum;
    }
    int enemyLevelFormStorey(bool ishard,int storey)
    {
        if (ishard)
        {
            switch (storey)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    return 4;
                case 8:
                case 9:
                case 10:
                    return 5;
            }
        }
        else
        {
            switch (storey)
            {
                case 1:
                case 2:
                case 3:
                    return 1;
                case 4:
                case 5:
                case 6:
                    return 2;
                case 8:
                case 9:
                case 10:
                    return 3;
            }
        }
        return 1;
    }
    //地点位置数据
    public List<Vector3> vector3list = new List<Vector3>() { new Vector3(-14, 8), new Vector3(-14, 6), new Vector3(-14, 4), new Vector3(-14, 2) };

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
        placenode.realPlace = result;
        result.Init(placenode);
        return result;
    }
    //private void 
    public void EnterBattle(battlePlace battle)
    {
        AsyncOperation _asyncOperation = SceneManager.LoadSceneAsync(AllAsset.MapAsset.GetSceneStr(battle.sceneId),LoadSceneMode.Additive);
        gameManager.Instance.battleScene = SceneManager.GetSceneByName(AllAsset.MapAsset.GetSceneStr(battle.sceneId));
        monsterInfo monster = normalEnemyLevel1[0];
        if (battle.imageorder == 1)
        {
            if (battle.storey == 1)
            {
                monster = normalEnemyLevel1[0];
                normalEnemyLevel1.Remove(monster);
            }
            else if (battle.storey == 2)
            {
                monster = normalEnemyLevel2[0];
                normalEnemyLevel2.Remove(monster);
            }
            else if (battle.storey == 3)
            {
                monster = normalEnemyLevel3[0];
                normalEnemyLevel3.Remove(monster);
            }
        }
        else if (battle.imageorder == 2)
        {
            if (battle.storey == 1)
            {
                monster = hardEnemyLevel4[0];
                hardEnemyLevel4.Remove(monster);
            }
            else if (battle.storey == 2)
            {
                monster = hardEnemyLevel5[0];
                hardEnemyLevel5.Remove(monster);
            }
        }
        else if (battle.imageorder == 3)
        {
            monster = bossList[0];
            bossList.Remove(monster);
        }
        else
        {
            Debug.Log("错误");
            monster = normalEnemyLevel1[0];
        }
        StartCoroutine(IEenterBattle(_asyncOperation, monster));
    }
    
    public void EnterTreasure()
    {
        mapState = MapState.EventWindow;
        befallinfo befallinfo = AllAsset.MapAsset.mapSystemBefall[0];
        gameManager.Instance.uimanager.uiBefallBoard.EnterEventBoard(befallinfo);
    }
    public void EnterSleep()
    {
        mapState = MapState.EventWindow;
        befallinfo befallinfo = AllAsset.MapAsset.mapSystemBefall[1];
        gameManager.Instance.uimanager.uiBefallBoard.EnterEventBoard(befallinfo);
    }
    public void EnterBefall()
    {
        befallinfo befallinfo = ListOperation.RandomValue<befallinfo>(befallList);
        //打开二级事件窗口
        gameManager.Instance.uimanager.uiBefallBoard.EnterEventBoard(befallinfo);
        gameManager.Instance.mapmanager.mapState = MapState.EventWindow;
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
