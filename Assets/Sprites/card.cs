﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CardKind
{
    PlayerCard,
    StateCard,
    CurseCard
}
[System.Serializable]
public class card:object
{
    public int Id;
    public string Name;
    public CardKind Kind;
    private bool canplay;
    public void SetCanPlay(bool b)
    {
        canplay = b;
    }
    public bool GetCanPlay()
    {
        return canplay;
    }
}
