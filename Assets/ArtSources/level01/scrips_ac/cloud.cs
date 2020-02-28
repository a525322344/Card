using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloud : MonoBehaviour
{
    private Transform cloudTrans;
    private float x_02;
    private float z_02;
    

    void Start()
    {
        cloudTrans = gameObject.GetComponent<Transform>();
        z_02 = cloudTrans.position.z;
       
        
    }

    
    void Update()
    {
        cloudTrans.Translate(new Vector3(-1,0,0)*0.01f,Space.World);
        x_02 = cloudTrans.position.x;
        cloudTrans.position = new Vector3(x_02,-0.01f*x_02*x_02+0.06f*x_02+5,z_02);
        //cloudTrans.Translate(new Vector3(0, -1, 0)*0.01f, Space.World);
        //y_02 = 0.01f * x_02 * x_02 + 0.06f * x_02 + 8;
        
        if (x_02 <= -25.5f)
        {
            cloudTrans.position = new Vector3(16.0f, Random.Range(4.2f,8.0f), z_02);
            Debug.Log("reset");
        }
        
    }
}
