using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LlightShining : MonoBehaviour
{
    private Light spotLight;
    
    void Start()
    {
        spotLight = gameObject.GetComponent<Light>();
        
    }

    
    void Update()
    {
        ShanShuo();
    }
    /// <summary>
    /// 随着时间控制灯光强度
    /// </summary>
    void ShanShuo()
    {
        if (Time.time <= 0.3f)
        {
            spotLight.intensity = 5.0f;           
        }
        if (Time.time > 0.3f&& Time.time<0.5f)
        {
            spotLight.intensity = 15.0f;
          
        }
        if (Time.time >= 0.5f&& Time.time<0.7f)
        {
            spotLight.intensity = 5.0f;
        }
        if (Time.time >= 0.7f && Time.time < 1.0f)
        {
            spotLight.intensity = 13.5f;
        }
        if (Time.time >= 1.0f&& Time.time<1.6f)
        {
            float intens = Mathf.Lerp(0.5f,17.0f,Random.Range(0.5f,0.9f));
            spotLight.intensity = intens;
        }
        
        if (Time.time >= 1.6f)
        {
            spotLight.intensity = 12.18f;           
        }

    }
}
