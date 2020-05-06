using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class spriteButton : MonoBehaviour
{
    Vector3 initScale;
    public bool isOn = true;
    private toDo onclick = () => { };
    private void OnMouseDown()
    {
        if (isOn)
        {
            onclick();
        }
    }
    public void AddListener(toDo todo)
    {
        onclick += todo;
    }
    private void Awake()
    {
        initScale = transform.localScale;
    }
    private void OnMouseEnter()
    {
        if (isOn)
        {
            transform.DOScale(initScale * 1.1f, 0.1f);
        }
    }
    private void OnMouseExit()
    {
        if (isOn)
        {
            transform.DOScale(initScale, 0.1f);
        }

    }
}
