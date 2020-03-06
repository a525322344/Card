using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using AllAsset;

public class MapManager : MonoBehaviour
{
    public int allplacenum;
    public int battleplacenum;
    public int shopplacenum;
    public int ralexplacenum;

    public List<place> placeList = new List<place>();
    float width = 15;
    float height = 8;

    private MapRootInfo maprootinfo;

    public void InitMap()
    {
        //mapRootInfo = GameObject.Find("root").GetComponent<MapRootInfo>();
        maprootinfo = gameManager.Instance.instantiatemanager.mapRootInfo;

        float x = Random.Range(-width, width);
        float y = Random.Range(-height, height);
        GameObject place = Instantiate(
            gameManager.Instance.instantiatemanager.placeGOs[0],
            new Vector3(x,y, maprootinfo.placefolder.position.z),
            Quaternion.identity,
            maprootinfo.placefolder
        );
        place newplace = new battlePlace(new monInfo_Cat(), 0);
        place.GetComponent<realPlace>().thisplace = newplace;
        placeList.Add(newplace);
        Debug.Log("init map");
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
