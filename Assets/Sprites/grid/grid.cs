using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class grid 
{
    public bool IsOpen;
    public bool Power;
    public bool Selected;
    public grid(bool open)
    {
        IsOpen = open;
        if (open)
        {
            Power = true;
        }
        else
        {
            Power = false;
        }
    }
}
