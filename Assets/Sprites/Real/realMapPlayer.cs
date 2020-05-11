using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class realMapPlayer : MonoBehaviour
{
    public bool moveing = false;
    public healthSlider healthSlider;
    Vector3 targetplace;
    public float delayShowTime;

    public void Init(PlaceNode nowplace)
    {
        transform.position = nowplace.realPlace.spriteRenderer.transform.position;
        targetplace = transform.position;
        healthSlider.isbattle = false;
        healthSlider.Init(gameManager.Instance.playerinfo);
        transform.localScale = Vector3.zero;
        StartCoroutine(IEShow());
    }
    public void MoveTo(PlaceNode placeNode)
    {
        targetplace = placeNode.realPlace.spriteRenderer.transform.position;
        //transform.DOMove(targetplace, 0.4f);
        transform.DOJump(targetplace, 0.5f, 3, 0.65f);
    }
    IEnumerator IEShow()
    {
        yield return new WaitForSeconds(delayShowTime);
        transform.DOScale(Vector3.one, 0.5f);
    }
}
