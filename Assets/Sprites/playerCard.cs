using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public delegate void CardEffect(int num);


[System.Serializable]
public class playerCard : card
{
    //public GameObject cardObject;
    public CardEffect cardEffect;

    public playerCard(int id,string name,CardKind kind,CardEffect effect)
    {
        Id = id;
        Name = name;
        Kind = kind;
        switch (kind)
        {
            case CardKind.CurseCard:
            case CardKind.StateCard:
                SetCanPlay(false);
                break;
            case CardKind.PlayerCard:
                SetCanPlay(true);
                break;
        }
        cardEffect = effect;
    }

    public void debug()
    {
        Debug.Log("ID:" + Id);
        Debug.Log("Name:" + Name);
        Debug.Log("Kind:" + Kind);
    }
}

