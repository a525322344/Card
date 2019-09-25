using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class bookFolderControll : MonoBehaviour
{
    public Transform positionStart;
    public Transform positionShow;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.Instance.battlemanager.b_isSelectCard)
        {
            transform.DOMove(positionShow.position,time);
            transform.DORotateQuaternion(positionShow.rotation,time);
            transform.DOScale(positionShow.localScale, time);
        }
        else
        {
            transform.DOMove(positionStart.position, time);
            transform.DORotateQuaternion(positionStart.rotation, time);
            transform.DOScale(positionStart.localScale, time);
        }
    }
}
