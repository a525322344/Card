using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class bookFolderControll : MonoBehaviour
{
    public List<realpart> realparts = new List<realpart>();

    public Transform positionStart;
    public Transform positionShow;
    public float time;
    private bool b_toshow;
    private bool b_alreadyToShow = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //to update gamestate
        if (gameManager.Instance.battlemanager)
        {
            if (gameManager.Instance.battlemanager.b_isSelectCard)
            {
                if (b_alreadyToShow)
                {
                    b_alreadyToShow = false;
                    StopCoroutine("IEtoShow");
                }
                b_toshow = true;
            }
            else
            {
                if (!b_alreadyToShow)
                {
                    b_alreadyToShow = true;
                    StartCoroutine("IEtoShow");
                }
                //b_toshow = false;
            }
        }

        if (b_toshow)
        {
            transform.DOMove(positionShow.position, time);
            transform.DORotateQuaternion(positionShow.rotation, time);
            transform.DOScale(positionShow.localScale, time);
        }
        else
        {
            transform.DOMove(positionStart.position, time);
            transform.DORotateQuaternion(positionStart.rotation, time);
            transform.DOScale(positionStart.localScale, time);
        }
    }

    IEnumerator IEtoShow()
    {
        yield return new WaitForSeconds(1.5f);
        b_toshow = false;
    }
}
