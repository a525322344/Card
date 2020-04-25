using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRootInfo : MonoBehaviour
{
    public Transform placefolder;

    public Transform sortPartPosition;
    public float sortPartDistance;
    public Transform knapsackPosition;
    public Transform UI3D;
    public uiEventBoard uieventBoard;
    public Light maplight;
    public Transform placeBeginPosi;
    public Transform secondBoardPosi;

    public Camera mapCamera;

    public float sortPositionZ()
    {
        //Debug.Log(sortPartPosition.localPosition.z);
        //Debug.Log(UI3D.localScale.x);
        return sortPartPosition.localPosition.z * UI3D.localScale.x;
    }
}
