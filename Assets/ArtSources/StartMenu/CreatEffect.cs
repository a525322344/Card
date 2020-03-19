using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatEffect : MonoBehaviour
{
    public GameObject smokEffct;
    private Transform HeroTrans;
    private GameObject Hero;

    void Start()
    {

        HeroTrans = gameObject.GetComponent<Transform>();
        Hero = gameObject;

    }

    private void SmokEffect()
    {
        Vector3 position = HeroTrans.position;
        GameObject.Destroy(Hero);
        GameObject.Instantiate(smokEffct, position, Quaternion.identity);
        Invoke("CreateIcon", 1.0f);
        
    }
   

}
