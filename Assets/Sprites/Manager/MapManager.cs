using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MAPSTAGE
{
    Shop,
    Event,
    Deck,
    Battle
}


public class MapManager : MonoBehaviour
{


    //敌人信息
    public List<enemybase> enemies = new List<enemybase>();
    //事件信息
    public List<mapEvent> events = new List<mapEvent>();


    //地点按钮
    public place Place;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnterBattle()
    {

    }
}
