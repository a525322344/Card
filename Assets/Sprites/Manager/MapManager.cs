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

    public float placedistance=5;

    int storeyNum = 12;
    int placePerNum = 4;

    public void InitMap()
    {
        Debug.Log("init map");
        maprootinfo = gameManager.Instance.instantiatemanager.mapRootInfo;

        place newplace;
        //战斗地点
        newplace = new battlePlace(new monInfo_Cat(), 3);      
        realplaceList.Add(instantiatePlace(newplace)); 
        //事件地点
        befallinfo secondbefall = new befallinfo("整装待发", -1, null, new Button_OverSortPart());
        befallinfo newbefallinfo = new befallinfo("整装待发", 0, "英雄征途的第一步：整理背包",
            new Button_ExitBefall("直接出发"),new Button_SortPart(secondbefall));
        newplace = new befallPlace(newbefallinfo);
        realplaceList.Add(instantiatePlace(newplace));
        
        Dictionary<Vector2, PlaceNode> placeNodeDic = new Dictionary<Vector2, PlaceNode>();

        PlaceNode startPlaceNode = new PlaceNode(new startPlace(), new Vector2(2.5f, 0));
        placeNodeDic.Add(startPlaceNode.PointPosi, startPlaceNode);
        PlaceNode endPlaceNode = new PlaceNode(new battlePlace(new monInfo_Cat(), 0), new Vector2(2.5f, storeyNum));
        placeNodeDic.Add(endPlaceNode.PointPosi, endPlaceNode);

        place battleplace = new battlePlace(new monInfo_Cat(), 3);
        for(int storey = 1; storey < storeyNum; storey++)
        {
            for(int order = 1; order <= placePerNum; order++)
            {
                PlaceNode placeNode = new PlaceNode(battleplace, new Vector2(order, storey));
                placeNodeDic.Add(new Vector2(order, storey), placeNode);
            }
        }
        //连线
        //start
        for(int order=1; order <= placePerNum; order++)
        {
            startPlaceNode.LinkNode(placeNodeDic[new Vector2(order, 1)]);
        }
        for (int storey = 1; storey < storeyNum-1; storey++)
        {
            for (int order = 1; order <= placePerNum; order++)
            {
                placeNodeDic[new Vector2(order, storey)].LinkNode(placeNodeDic[new Vector2(order, storey + 1)]);
            }
        }
        for (int order = 1; order <= placePerNum; order++)
        {
            placeNodeDic[new Vector2(order, storeyNum-1)].LinkNode(endPlaceNode);
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
    //    befallinfo newbefallinfo1 = new befallinfo("神秘山泉", 0, "有点甜的神秘泉水",
    //        new Button_ExitBefall("喝（回复15点血量）"), new Button_ExitBefall("洗涤身体"));
    //    newplace = new befallPlace(newbefallinfo1);
    //    realplaceList.Add(instantiatePlace(newplace));

    //    befallinfo newbefallinfo2 = new befallinfo("微笑的果农", 0, "“哎呀呀年轻人，你想尝尝哪种水果呢？”",
    //        new Button_ExitBefall("梨子（最大生命值+5））"), new Button_ExitBefall("苹果（回复15点血量）"), new Button_ExitBefall("西瓜（获得卡牌“西瓜种子”）"));
    //    newplace = new befallPlace(newbefallinfo2);
    //    realplaceList.Add(instantiatePlace(newplace));

    //地点位置数据
    public List<Vector3> vector3list = new List<Vector3>() { new Vector3(7.44f, 4.11f), new Vector3(0.28f, -1.18f), new Vector3(-8.67f, -3.65f), new Vector3(1.01f, -8.83f) ,new Vector3(-13.68f, 4.02f),new Vector3(-3.61f, 5.29f) };

    //实例生成地点
    private realPlace instantiatePlace(place place)
    {
        Vector3 vec = ListOperation.RandomValue<Vector3>(vector3list);
        GameObject placego = Instantiate(
            gameManager.Instance.instantiatemanager.placeGO,
            vec,
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
        float x = Random.Range(-0.3f, 0.3f);
        float y = Random.Range(-0.3f, 0.2f);
        GameObject placego = Instantiate(
            gameManager.Instance.instantiatemanager.placeGO,
            maprootinfo.placeBeginPosi.position + new Vector3(placenode.PointPosi.x+x,placenode.PointPosi.y+y)*placedistance,
            Quaternion.identity,
            maprootinfo.placefolder
        );
        realPlace result = placego.GetComponent<realPlace>();
        placenode.realplaceTran = result.transform;
        result.Init(placenode.thisplace);
        return result;
    }
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
