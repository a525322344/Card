using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//latice 栅格
public class latice
{
    public latice(int xposi,int yposi,bool isexploit)
    {
        position = new Vector2(xposi, yposi);
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

    public Dictionary<Vector2, latice> laticePairs = new Dictionary<Vector2, latice>();

    public void Init()
    {
        //laticePairs
    }
}
