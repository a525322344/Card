using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraDown : MonoBehaviour
{
    private Animator camDown;

    void Start()
    {
        camDown = gameObject.GetComponent<Animator>();
    }


    void Update()
    {
        //按左键摄像机往下拉
        if (Input.GetMouseButton(0))
        {
            Debug.Log("按下左键");
            camDown.SetBool("down", true);
        }
    }
}

