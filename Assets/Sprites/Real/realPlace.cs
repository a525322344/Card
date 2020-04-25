using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class realPlace : MonoBehaviour
{
    public PlaceNode placeNode;
    public place thisplace;

    public SpriteRenderer spriteRenderer;
    public Transform spriteTran;
    public Color NormalColor;
    public Color ToGoColor;
    public Color PressedColor;
    public Color DisableColor;
    public float colorChangeTime;
    public float overside = 1.3f;
    public float sizeChangeTime;
    public bool b_mouseOver;

    private void Start()
    {
        //placeNode = new PlaceNode();
    }

    public void Update()
    {
        switch (placeNode.placeState)
        {
            case PlaceState.DenseFog:
                DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, NormalColor, colorChangeTime);
                if (b_mouseOver)
                {
                    spriteTran.DOScale(Vector3.one * overside, sizeChangeTime);
                }
                else
                {
                    spriteTran.DOScale(Vector3.one, sizeChangeTime);
                }
                break;
            case PlaceState.NowOn:
                DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, NormalColor, colorChangeTime);
                if (b_mouseOver)
                {
                    spriteTran.DOScale(Vector3.one * overside, sizeChangeTime);
                }
                else
                {
                    spriteTran.DOScale(Vector3.one, sizeChangeTime);
                }
                break;
            case PlaceState.ToGo:
            case PlaceState.ToGoOut:
                DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, ToGoColor, colorChangeTime);
                if (b_mouseOver)
                {
                    spriteTran.DOScale(Vector3.one * overside, sizeChangeTime);
                }
                else
                {
                    spriteTran.DOScale(Vector3.one, sizeChangeTime);
                }
                break;
            case PlaceState.Used:
                DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, DisableColor, colorChangeTime);
                if (b_mouseOver)
                {
                    spriteTran.DOScale(Vector3.one * overside, sizeChangeTime);
                }
                else
                {
                    spriteTran.DOScale(Vector3.one, sizeChangeTime);
                }
                break;
        }
    }

    public void Init(PlaceNode _placenode)
    {
        thisplace = _placenode.thisplace;
        placeNode = _placenode;
        spriteRenderer.sprite = gameManager.Instance.instantiatemanager.mapPlaceSprites[thisplace.imageorder];
    }
    public void Init(place _placenode)
    {
        thisplace = _placenode;
        spriteRenderer.sprite = gameManager.Instance.instantiatemanager.mapPlaceSprites[thisplace.imageorder];
    }

    public void OnMouseDown()
    {
        thisplace.onclick();
    }
    public void OnMouseEnter()
    {
        b_mouseOver = true;
    }
    public void OnMouseExit()
    {
        b_mouseOver = false;
    }
}
