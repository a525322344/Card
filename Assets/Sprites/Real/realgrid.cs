using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constant;

public class realgrid : MonoBehaviour
{
    public realpart fatherPart;
    public grid thisgrid;
    public GridState gridState = GridState.NotActive;
    private MeshRenderer _renderer;

    public CardOutlineShaderCS gridOutlineCS;
    public Material mr_black;
    public Material mr_write;
    public Material mr_gray;
    public Material mr_cyan;

    public void Init(grid _thisgrid,realpart fatherpart)
    {
        thisgrid = _thisgrid;
        fatherPart = fatherpart;

        if (!fatherPart.b_ShowOutlineInMap)
        {
            gridOutlineCS.gameObject.SetActive(false);
        }

        _renderer = GetComponent<MeshRenderer>();
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
    }

    public void changeMaterial()
    {
        switch (gridState)
        {
            case GridState.Power:
                _renderer.material = mr_write;
                break;
            case GridState.Can:
                _renderer.material = mr_cyan;
                break;
            case GridState.Used:
                _renderer.material = mr_gray;
                break;
        }
    }
    public void changeMaterial(GridState state)
    {
        gridState = state;
        switch (gridState)
        {
            case GridState.Power:
                _renderer.material = mr_write;
                break;
            case GridState.Can:
                _renderer.material = mr_cyan;
                break;
            case GridState.Used:
                _renderer.material = mr_gray;
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
