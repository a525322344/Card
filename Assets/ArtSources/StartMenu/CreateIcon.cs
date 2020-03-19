using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateIcon : MonoBehaviour
{
    private Transform HeroTrans;
    private GameObject Hero;
    public GameObject icon;
    public GameObject end;
    private Transform endPosition;
    private Quaternion rotate;
    // Start is called before the first frame update
    void Start()
    {
        HeroTrans = gameObject.GetComponent<Transform>();
        Hero = gameObject;
        endPosition = end.GetComponent<Transform>();
        rotate = endPosition.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Icon()
    {
        Vector3 position = HeroTrans.position;
        GameObject.Instantiate(icon, position, rotate);
        Debug.Log("icon");
    }
}
