using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constant;

public class realgrid : MonoBehaviour
{
    public grid thisgrig;
    private MeshRenderer _renderer;
    public Material mr_black;
    public Material mr_write;
    public Material mr_gray;
    public Material mr_cyan;

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
}
