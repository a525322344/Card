using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realPlace : MonoBehaviour
{
    public place thisplace;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(this.tag == "battlePlace")
        {
            thisplace = new battlePlace();
        }

        if (this.tag == "shopPlace")
        {
            thisplace = new shopPlace();
        }

        if (this.tag == "eventPlace")
        {
            thisplace = new eventPlace();
        }

        if (this.tag == "deckPlace")
        {
            thisplace = new deckPlace();
        }
    }

    private void OnMouseDown()
    {
        thisplace.onclick();
    }

    private void OnMouseOver()
    {
        thisplace.onover();
    }
}
