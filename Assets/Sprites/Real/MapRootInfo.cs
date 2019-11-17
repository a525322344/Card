using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRootInfo : MonoBehaviour
{
    private void Awake()
    {
        m = GameObject.Find("root").GetComponent<MapRootInfo>();
    }
    public MapRootInfo m;
    public Transform placefolder;
}
