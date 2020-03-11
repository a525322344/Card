using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realLatice : MonoBehaviour
{
    public Material black_lock;
    public Material white_unlock;
    public latice thislatice;

    private MeshRenderer renderer;

    private void Awake()
    {
        renderer =transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    public void Init(latice l)
    {
        thislatice = l;
        changeColor();
    }

    public void changeColor()
    {
        if (thislatice.b_exploit)
        {
            renderer.material = white_unlock;
        }
        else
        {
            renderer.material = black_lock;
        }
    }
}
