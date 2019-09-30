using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showcontroll : MonoBehaviour
{
    public Slider healthslider;
    public enemybase enemybase;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        healthslider.value = enemybase.healthnow;
    }
    public void init()
    {
        enemybase = GetComponent<enemyControll>().pikaqiu;
        healthslider.maxValue = enemybase.healthmax;
        healthslider.minValue = 0;
    }
}
