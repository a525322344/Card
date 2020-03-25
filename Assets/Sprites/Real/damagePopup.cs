using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class damagePopup : MonoBehaviour
{
    public float value;
    public Vector2 speedrange;
    public float speed;
    public float ac = 10;
    public Text text;
    private float initscale=1;
    private float lefttime=0.5f;

    public float xrange;
    private float xspeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int num)
    {
        value = num;
        speed = Random.Range(speedrange.x, speedrange.y);
        text.text = "" + value;
        xspeed = Random.Range(-xrange, xrange);
    }
    // Update is called once per frame
    void Update()
    {
        speed -= ac * Time.deltaTime;
        transform.LookAt(instantiateManager.instance.battleEnvRoot.ENCamera.transform);
        transform.DOScale(Vector3.one * initscale * 1.1f, lefttime);
        transform.Translate(new Vector3(xspeed,speed,0) * Time.deltaTime);
    }
}
