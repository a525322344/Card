using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class spriteButton : MonoBehaviour
{
    Vector3 initScale;
    public Color sleepColor;
    
    public bool IsActive = true;
    public bool isMoseOn = true;
    private toDo onclick = () => { };
    private void OnMouseDown()
    {
        if (isMoseOn)
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
        if (isMoseOn)
        {
            transform.DOScale(initScale * 1.1f, 0.1f);
        }
    }
    private void OnMouseExit()
    {
        if (isMoseOn)
        {
            transform.DOScale(initScale, 0.1f);
        }

    }

    public void SetActive(bool a)
    {
        if (a)
        {
            IsActive = true;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            IsActive = false;
            GetComponent<SpriteRenderer>().color = sleepColor;
        }
    }
}
