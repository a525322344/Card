using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showcontroll : MonoBehaviour
{
    public void Awake()
    {
        Debug.Log("sin(pi/2)=" + (int)Mathf.Sin(Mathf.PI*2));
        Debug.Log("sin(pi)="+(int)Mathf.Sin(Mathf.PI)+"???");
    }
}
