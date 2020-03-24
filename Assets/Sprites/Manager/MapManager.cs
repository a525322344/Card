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
    private MapRootInfo maprootinfo;

    float width = 15;
    float height = 8;
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
        
        befallinfo newbefallinfo1 = new befallinfo("神秘山泉", 0, "有点甜的神秘泉水",
            new Button_ExitBefall("喝（回复15点血量）"), new Button_ExitBefall("洗涤身体"));
        newplace = new befallPlace(newbefallinfo1);
        realplaceList.Add(instantiatePlace(newplace));

        befallinfo newbefallinfo2 = new befallinfo("微笑的果农", 0, "“哎呀呀年轻人，你想尝尝哪种水果呢？”",
            new Button_ExitBefall("梨子（最大生命值+5））"), new Button_ExitBefall("苹果（回复15点血量）"), new Button_ExitBefall("西瓜（获得卡牌“西瓜种子”）"));
        newplace = new befallPlace(newbefallinfo2);
        realplaceList.Add(instantiatePlace(newplace));
    }
    //实例生成地点
    private realPlace instantiatePlace(place place)
    {
        Vector3[] a = new Vector3[] { new Vector3(7.44f, 4.11f), new Vector3(0.28f, -1.18f), new Vector3(-8.67f, -3.65f), new Vector3(1.01f, -8.83f) };
        List<Vector3> list = new List<Vector3>(a);
        Vector3 vec = ListOperation.RandomValue<Vector3>(list);
        GameObject placego = Instantiate(
            gameManager.Instance.instantiatemanager.placeGO,
            vec,
            Quaternion.identity,
            maprootinfo.placefolder
        );
        realPlace result = placego.GetComponent<realPlace>();
        result.Init(place);
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
        gameManager.Instance.battlemanager.BattleStartEnemySet(monster);
        gameManager.Instance.battlemanager.startBattale();
    }
}
