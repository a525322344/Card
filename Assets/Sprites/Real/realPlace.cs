using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realPlace : MonoBehaviour
{
    public place thisplace;


    private void OnMouseDown()
    {
        thisplace.onclick();

    }

    private void OnMouseOver()
    {
        thisplace.onover();
    }
}
