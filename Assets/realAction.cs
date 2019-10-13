using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class realAction : MonoBehaviour
{
    public Text num;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetNum(string str)
    {
        if (num)
        {
            num.text = str;
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
