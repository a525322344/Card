using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class addlistenerTest : MonoBehaviour
{
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(aa);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void aa()
    {
        Debug.Log("ga!ga!");
    }
}
