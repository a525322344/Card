using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constant;
using DG.Tweening;

public class realgrid : MonoBehaviour
{
    public realpart fatherPart;
    public grid thisgrid;
    public GridState gridState = GridState.NotActive;
    //private MeshRenderer _renderer;

    public SpriteRenderer costRenderer;
    public SpriteRenderer diwenRenderer;
    public Sprite[] diwenSprites;

    public Color normalColor;
    public Color canColor;
    public Color useColor;

    public AnimationCurve alphacurve;
    public float runtime = 1;
    [Range(0, 1)]
    private float puntime;
    private bool isfront = true;

    public CardOutlineShaderCS gridOutlineCS;
    //public Material mr_black;
    //public Material mr_write;
    //public Material mr_gray;
    //public Material mr_cyan;

    public void Init(grid _thisgrid,realpart fatherpart)
    {
        thisgrid = _thisgrid;
        fatherPart = fatherpart;
        diwenRenderer.sprite = diwenSprites[fatherpart.thisMagicPart.diWenNUm];
        costRenderer.color = normalColor;
        if (!fatherPart.b_ShowOutlineInMap)
        {
            gridOutlineCS.gameObject.SetActive(false);
        }

        //_renderer = GetComponent<MeshRenderer>();
        if (thisgrid.Opening)
        {
            gridState = GridState.Power;
        }
        else
        {
            gridState = GridState.NotActive;
            gameObject.SetActive(false);
        }
        changeMaterial();
        puntime = Random.Range(0, 1);
    }

    public void Update()
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
        float alpha = alphacurve.Evaluate(puntime);
        Color color = new Color(useColor.r, useColor.g, useColor.b, alpha);
        diwenRenderer.color = color;
    }

    public void changeMaterial()
    {
        switch (gridState)
        {
            case GridState.Power:
                DOTween.To(() => costRenderer.color, x => costRenderer.color = x, normalColor, 0.5f);
                //spriteRenderer.color = normalColor;
                break;
            case GridState.Can:
                DOTween.To(() => costRenderer.color, x => costRenderer.color = x, canColor, 0.2f);
                break;
            case GridState.Used:
                DOTween.To(() => costRenderer.color, x => costRenderer.color = x, useColor, 0.2f);
                //_renderer.material = mr_gray;
                //spriteRenderer.color = useColor;
                break;
        }
    }
    public void changeMaterial(GridState state)
    {
        gridState = state;
        switch (gridState)
        {
            case GridState.Power:
                DOTween.To(() => costRenderer.color, x => costRenderer.color = x, normalColor, 0.5f);
                break;
            case GridState.Can:
                DOTween.To(() => costRenderer.color, x => costRenderer.color = x, canColor, 0.2f);
                break;
            case GridState.Used:
                DOTween.To(() => costRenderer.color, x => costRenderer.color = x, useColor, 0.2f);
                break;
        }
    }
    private void OnMouseDown()
    {
        fatherPart.OnMouseDown();
    }
    private void OnMouseEnter()
    {
        fatherPart.OnMouseEnter();
    }
    private void OnMouseExit()
    {
        fatherPart.OnMouseExit();
    }
    private void OnMouseUp()
    {
        fatherPart.OnMouseUp();
    }
}
