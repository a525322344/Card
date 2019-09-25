using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constant;

public class realgrid : MonoBehaviour
{
    public realpart fatherPart;
    public grid thisgrig;
    private Part part;
    private MeshRenderer _renderer;
    private Material mr_black;
    private Material mr_write;
    private Material mr_gray;
    private Material mr_cyan;


    public card downtheCard;
    private bool b_DownCard;
    public void SetDownCard(card _selectcard)
    {
        if (_selectcard == null)
        {
            downtheCard = null;
            b_DownCard = false;
        }
        else
        {
            downtheCard = _selectcard;
            b_DownCard = true;
        }
    }

    public void setThisGrid(grid _grid)
    {
        thisgrig = _grid;
    }
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        
    }
    public void Init()
    {
        _renderer = GetComponent<MeshRenderer>();
        mr_black = Resources.Load<Material>(Path.GRID_MATERIAL_BLACK);
        mr_cyan = Resources.Load<Material>(Path.GRID_MATERIAL_CYAN);
        mr_gray = Resources.Load<Material>(Path.GRID_MATERIAL_GRAY);
        mr_write = Resources.Load<Material>(Path.GRID_MATERIAL_WRITE);
    }
    // Update is called once per frame
    void Update()
    {
        if (b_DownCard)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(downtheCard.Describe);
            }
        }
    }
    public void changeMaterial()
    {
        if (thisgrig.Opening)
        {
            _renderer.material = mr_write;
        }
        else
        {
            _renderer.material = mr_black;
        }
    }

    public bool CanOverCostPlay(card selectcard)
    {
        return fatherPart.CanCostPlay(thisgrig, selectcard);
    }
}
