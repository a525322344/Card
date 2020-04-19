using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realPlayer : MonoBehaviour
{
    //public playerInfo playerinfo;
    public pawnbase playerpawn;
    public healthSlider healthslider;
    public Animator animator;
    public GameObject realStateGO;
    public GameObject damageshow;
    public Transform damagePosi;

    List<GameObject> statego = new List<GameObject>();

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

    public void StateUpdtae()
    {
        for (int i = statego.Count - 1; i >= 0; i--)
        {
            Destroy(statego[i]);
        }
        statego.Clear();
        int a = 0;
        foreach (var state in playerpawn.nameStatePairs)
        {
            GameObject stateg = Instantiate(realStateGO, healthslider.statePosi);
            statego.Add(stateg);
            stateg.transform.localPosition = stateg.transform.localPosition + Vector3.right * healthslider.statedistance * a;
            stateg.GetComponent<realState>().Init(state.Value);
            a++;
        }
    }
}
