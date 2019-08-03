using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realgrid : MonoBehaviour
{
    public grid thisgrig;
    private MeshRenderer _renderer;
    public Material part_black;
    public Material part_write;
    public Material part_gray;
    public Material part_cyan;
    void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }
    public void Init()
    {
        _renderer = GetComponent<MeshRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeMaterial()
    {
        if (thisgrig.IsOpen)
        {
            if(thisgrig.Power)
            {
                if (thisgrig.Selected)
                {
                    _renderer.material = part_cyan;
                }
                else
                {
                    _renderer.material = part_write;
                }
            }
            else
            {
                _renderer.material = part_gray;
            }
        }
        else
        {
            _renderer.material = part_black;
        }
    }
}
