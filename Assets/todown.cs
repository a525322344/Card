using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class todown : MonoBehaviour
{
    public float speed;
    public bool b_todown;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (b_todown)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
    }
}
