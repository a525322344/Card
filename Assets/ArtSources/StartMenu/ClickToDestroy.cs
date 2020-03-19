using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToDestroy : MonoBehaviour
{

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            GameObject.Destroy(gameObject);
        }

    }
}
