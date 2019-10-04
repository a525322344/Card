using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realPlace : MonoBehaviour
{
    public place thisplace;

    // Start is called before the first frame update
    void Start()
    {
        thisplace = new battlePlace();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        thisplace.onclick();
    }
}
