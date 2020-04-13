using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realEffect : MonoBehaviour
{
    Vector3 dir;
    float movespeed;
    int way;
    public void Init(int kind,float speed,float lasttime)
    {
        Destroy(gameObject, lasttime);
        dir = gameManager.Instance.battlemanager.realenemy.transform.position - gameManager.Instance.battlemanager.realplayer.transform.position;
        movespeed = speed;
        way = kind;
    }
    void Update()
    {
        switch (way)
        {
            case 2:
                transform.Translate(dir * movespeed * Time.deltaTime);
                break;
            case 3:
                transform.Translate(-dir * movespeed * Time.deltaTime);
                break;
        }
    }
}
