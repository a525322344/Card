using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToStart : MonoBehaviour
{
    private Animator start;
    void Start()
    {
        start = gameObject.GetComponent<Animator>();
    }

    
    void Update()
    {
        //按下鼠标左键将动画机中的start设置为true；
        if (Input.GetMouseButton(0))
        {
            Debug.Log("按下左键");
            start.SetBool("start",true);
        }
    }
}
