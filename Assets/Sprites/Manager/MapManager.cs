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
        newplace = new battlePlace(new monInfo_Cat(), 1);      
        realplaceList.Add(instantiatePlace(newplace));

        //事件地点
        befallinfo newbefallinfo = new befallinfo("休整片刻", 0, "时不时得整理下装备，或许可以在战斗中得优势",
            new Button_ExitBefall("现在只能选择不这么做"),new Button_SortPart());
        newplace = new befallPlace(newbefallinfo);
        realplaceList.Add(instantiatePlace(newplace));
    }

    //实例生成地点
    private realPlace instantiatePlace(place place)
    {
        float x = Random.Range(-width, width);
        float y = Random.Range(-height, height);
        GameObject placego = Instantiate(
            gameManager.Instance.instantiatemanager.placeGO,
            new Vector3(x, y, maprootinfo.placefolder.position.z),
            Quaternion.identity,
            maprootinfo.placefolder
        );
        realPlace result = placego.GetComponent<realPlace>();
        result.Init(place);
        return result;
    }

    public void EnterBattle(battlePlace battle)
    {
        AsyncOperation _asyncOperation = SceneManager.LoadSceneAsync(AllAsset.MapAsset.GetSceneStr(battle.sceneId));
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
