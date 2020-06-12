using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class uiEventbutton : MonoBehaviour
{
    public bool ismouseOver;
    public Text buttondescribe;
    public string describe;

    public void Init(string des)
    {
        describe = des;
        buttondescribe.text = "· " + describe;
    }

    void Update()
    {
        if (ismouseOver)
        {
            transform.DOScale(Vector3.one * 1.2f, 0.2f);
        }
        else
        {
            transform.DOScale(Vector3.one, 0.2f);
        }
    }
    private void OnMouseEnter()
    {
        ismouseOver = true;
        buttondescribe.text = "> " + describe;
    }
    private void OnMouseExit()
    {
        ismouseOver = false;
        buttondescribe.text = "· " + describe;
    }
}
