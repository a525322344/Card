using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class realMapPlayer : MonoBehaviour
{
    public bool moveing = false;
    public healthSlider healthSlider;
    Vector3 targetplace;


    public void Init(PlaceNode nowplace)
    {
        transform.position = nowplace.realPlace.spriteRenderer.transform.position;
        targetplace = transform.position;
        healthSlider.isbattle = false;
        healthSlider.Init(gameManager.Instance.playerinfo);
    }
    public void MoveTo(PlaceNode placeNode)
    {
        targetplace = placeNode.realPlace.spriteRenderer.transform.position;
        //transform.DOMove(targetplace, 0.4f);
        transform.DOJump(targetplace, 0.5f, 3, 0.65f);
    }
}
