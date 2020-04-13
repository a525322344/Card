using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realPlayer : MonoBehaviour
{
    //public playerInfo playerinfo;
    public pawnbase playerpawn;
    public healthSlider healthslider;
    public Animator animator;

    public void Init(pawnbase pawn)
    {
        Debug.Log(transform.GetChild(0).name);
        playerpawn = pawn;
        healthslider.Init(pawn);
    }

    public void changeHealthAndArmor(float armor, float heath)
    {
        healthslider.SetSlider(armor, heath);
    }
    public void changeAnimation(int i)
    {
        animator.SetInteger("animaInt", i);
    }
    public void changeAnimation()
    {
        animator.SetInteger("animaInt", 1);
    }
}
