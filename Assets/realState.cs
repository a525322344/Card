using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class realState : MonoBehaviour
{
    public Image teximage;
    public Text num;
    public void Init(stateAbstarct state)
    {
        teximage.sprite = gameManager.Instance.instantiatemanager.stateSprites[state.texint];
        if (state.num == -999)
        {
            num.gameObject.SetActive(false);
        }
        else
        {
            num.gameObject.SetActive(true);
            num.text = "" + state.num;
        }
    }
}
