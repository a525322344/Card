using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firetest : MonoBehaviour
{
    public GameObject starPosition;//特效起始位置
    public GameObject endPosition;//特效结束位置
    public GameObject effect;//特效
    private Vector3 x;

   

    void Start()
    {
        starPosition = gameObject;
        endPosition = gameObject;
        effect = gameObject;
        x = endPosition.GetComponent<Transform>().position;

    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            GameObject.Instantiate(effect,starPosition.gameObject.GetComponent<Transform>().transform);
            effect.GetComponent<Transform>().Translate(x*1.0f);
        }
        if (effect.GetComponent<Transform>().position.x>endPosition.GetComponent<Transform>().position.x)
        {
            GameObject.Destroy(effect);
        }
    }
}
