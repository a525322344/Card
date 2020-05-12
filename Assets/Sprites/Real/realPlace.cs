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
    public SpriteRenderer spriteDown;
    public Transform spriteTran;
    public Color NormalColor;
    public Color ToGoColor;
    public Color PressedColor;
    public Color DisableColor;
    public float colorChangeTime;
    public float overside = 1.3f;
    public float oversideTogo = 1.5f;
    public float sizeChangeTime;
    public bool b_mouseOver;

    public AnimationCurve curve;
    public AnimationCurve alphacurve;
    public float runtime=1;
    [Range(0,1)]
    private float puntime;
    private bool isfront = true;

    public Image biaoji;
    public void Update()
    {
        switch (placeNode.placeState)
        {
            case PlaceState.Cannot:
                //if (b_mouseOver)
                //{
                //    spriteTran.DOScale(Vector3.one * overside, sizeChangeTime);
                //    Color color = new Color(NormalColor.r, NormalColor.g, NormalColor.b, 1);
                //    DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, color, colorChangeTime);
                //}
                //else
                //{
                //    DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, NormalColor, colorChangeTime);
                //    spriteTran.DOScale(Vector3.one, sizeChangeTime);
                //}
                DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, NormalColor, colorChangeTime);
                spriteTran.DOScale(Vector3.one, sizeChangeTime);
                break;
            case PlaceState.NowOn:
                DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, NormalColor, colorChangeTime);
                if (b_mouseOver)
                {
                    spriteTran.DOScale(Vector3.one, sizeChangeTime);
                }
                else
                {
                    spriteTran.DOScale(Vector3.one, sizeChangeTime);
                }
                break;
            case PlaceState.ToGo:
                if (b_mouseOver)
                {
                    if (isfront)
                    {
                        puntime += Time.deltaTime / runtime;
                        if (puntime > 1)
                        {
                            puntime = 1;
                            isfront = false;
                        }
                    }
                    else
                    {
                        puntime -= Time.deltaTime / runtime;
                        if (puntime < 0)
                        {
                            puntime = 0;
                            isfront = true;
                        }
                    }
                    float alpha = alphacurve.Evaluate(1);
                    Color color = new Color(ToGoColor.r, ToGoColor.g, ToGoColor.b, alpha);
                    DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, color, colorChangeTime);
                    spriteTran.DOScale(Vector3.one * oversideTogo, sizeChangeTime);
                }
                else
                {
                    if (isfront)
                    {
                        puntime += Time.deltaTime / runtime;
                        if (puntime > 1)
                        {
                            puntime = 1;
                            isfront = false;
                        }
                    }
                    else
                    {
                        puntime -= Time.deltaTime / runtime;
                        if (puntime < 0)
                        {
                            puntime = 0;
                            isfront = true;
                        }
                    }
                    float scale = curve.Evaluate(puntime);
                    scale = 1 + (oversideTogo - 1) * scale;
                    spriteTran.DOScale(Vector3.one * scale, sizeChangeTime);
                    float alpha = alphacurve.Evaluate(puntime);
                    Color color = new Color(ToGoColor.r, ToGoColor.g, ToGoColor.b, alpha);
                    DOTween.To(() => spriteRenderer.color, x => spriteRenderer.color = x, color, colorChangeTime);
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
        spriteDown.sprite = gameManager.Instance.instantiatemanager.mapPlaceDiSprites[thisplace.imageorder];
    }
    public void Init(place _placenode)
    {
        thisplace = _placenode;
        spriteRenderer.sprite = gameManager.Instance.instantiatemanager.mapPlaceSprites[thisplace.imageorder];
    }

    public void OnMouseDown()
    {
        if (gameManager.Instance.mapmanager.MapPlaceOpen)
        {
            if (gameManager.Instance.mapmanager.mapState == MapState.MainMap)
            {
                StartCoroutine(IEenterPlace());
            }
        }
        else
        {
            switch (placeNode.placeState)
            {
                case PlaceState.Cannot:
                    break;
                case PlaceState.NowOn:
                    break;
                case PlaceState.ToGo:
                    if (gameManager.Instance.mapmanager.mapState == MapState.MainMap)
                    {
                        StartCoroutine(IEenterPlace());
                    }
                    break;
                case PlaceState.Used:
                    break;
            }
        }
    }
    public void OnMouseEnter()
    {
        switch (placeNode.placeState)
        {
            case PlaceState.Cannot:
                break;
            case PlaceState.NowOn:
                break;
            case PlaceState.ToGo:
                break;
            case PlaceState.Used:
                break;
        }
        b_mouseOver = true;
    }
    public void OnMouseExit()
    {
        switch (placeNode.placeState)
        {
            case PlaceState.Cannot:
                break;
            case PlaceState.NowOn:
                break;
            case PlaceState.ToGo:
                break;
            case PlaceState.Used:
                break;
        }
        b_mouseOver = false;
    }
    IEnumerator IEenterPlace()
    {
        //画标记
        biaoji.gameObject.SetActive(true);
        //玩家token移动
        DOTween.To(() => biaoji.fillAmount, x => biaoji.fillAmount = x, 1, 0.25f);
        yield return new WaitForSeconds(0.15f);
        gameManager.Instance.mapmanager.mapplayer.MoveTo(placeNode);
        yield return new WaitForSeconds(0.8f);
        gameManager.Instance.mapmanager.SetNowPlace(placeNode);
        thisplace.onclick();
    }
}
