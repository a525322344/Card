using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//latice 栅格
[System.Serializable]
public class latice
{
    public latice(int xposi,int yposi,bool isexploit)
    {
        position = new Vector2(xposi, yposi);
        b_exploit = isexploit;
    }
    public latice(Vector2 posi, bool isexploit)
    {
        position = posi;
        b_exploit = isexploit;
    }
    //储存位置
    public Vector2 position;
    //决定上限，是否开发
    public bool b_exploit;
    //实际使用，由部件激活
    public bool b_use;
}
public class realKnapsack : MonoBehaviour
{
    //创建5X5预备背包格，初始开放6格，中心点为3，3
    public GameObject LaticeGO;
    public Transform pointtran;
    public float distance;

    bool[] ise = new bool[25];

    public Dictionary<Vector2, realLatice> laticePairs = new Dictionary<Vector2, realLatice>();
    private Dictionary<Vector2, latice> lactices = new Dictionary<Vector2, latice>();


    public void Init(bool[] isexploits)
    {
        for(int i = 0; i < 25; i++)
        {
            int posx = i % 5;
            int posy = i / 5;
            Vector2 posi = new Vector2(posx, posy);
            latice newLatice = new latice(posi, isexploits[i]);
            lactices.Add(posi, newLatice);
        }
        foreach (var pl in lactices)
        {
            GameObject realLaticeGO = Instantiate(LaticeGO, pointtran);
            //realLaticeGO.name = "realLatice";
            Vector2 posi = pl.Key;
            realLaticeGO.transform.localPosition = new Vector3(posi.x - 2, posi.y - 2, 0) * distance;
            realLatice realLatice = realLaticeGO.GetComponent<realLatice>();
            realLatice.Init(pl.Value);

            laticePairs.Add(posi, realLatice);
        }
    }
}
