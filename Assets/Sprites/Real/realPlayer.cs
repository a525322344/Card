using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realPlayer : MonoBehaviour
{
    //public playerInfo playerinfo;
    public pawnbase playerpawn;
    public healthSlider healthslider;

    public void Init(pawnbase pawn)
    {
        playerpawn = pawn;
        healthslider.Init(pawn);
    }

    public void changeHealthAndArmor(float armor, float heath)
    {
        healthslider.SetSlider(armor, heath);
    }
}
