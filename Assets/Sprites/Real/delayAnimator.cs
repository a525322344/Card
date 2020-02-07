using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delayAnimator : MonoBehaviour
{
    public float delayTime;

    private Animator[] animators;
    void Start()
    {
        animators = GetComponentsInChildren<Animator>();
        if (animators.Length>0)
        {
            for(int i = 0; i < animators.Length; i++)
            {
                animators[i].speed = 0;
            }
            StartCoroutine(delayToAnima());
        }
    }

    IEnumerator delayToAnima()
    {
        yield return new WaitForSeconds(delayTime);
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].speed = 1;
        }
    }
}
