using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatEffect : MonoBehaviour
{
    public GameObject smokEffct;
    public GameObject hero;

    private void SmokEffect()
    {
        Vector3 position = transform.position;
        hero.SetActive(false);
        GameObject.Instantiate(smokEffct, position, Quaternion.identity);
        //Invoke("CreateIcon", 1.0f);
        
    }
    private void MapEnter()
    {
        Debug.Log("mapEnter");
        if (gameManager.Instance.uimanager.startMuneControll)
        {
            gameManager.Instance.uimanager.startMuneControll.b_animaOver = true;
        }
    }

}
