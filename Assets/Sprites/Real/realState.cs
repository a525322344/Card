using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class realState : MonoBehaviour
{
    public stateAbstarct thisstate;
    public Image teximage;
    public Text num;
    public GameObject uiInfoFrameGo;
    private GameObject nowIFrame;
    public float uiInfoFrameGoDownOffset;
    public void Init(stateAbstarct state)
    {
        thisstate = state;
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
    
    public void OnMouseEnter()
    {
        if (!nowIFrame)
        {
            nowIFrame = Instantiate(uiInfoFrameGo, transform);
            nowIFrame.GetComponent<UiInfoFrame>().Init(thisstate);
            nowIFrame.transform.localPosition = Vector3.zero;
            nowIFrame.transform.position -= Vector3.up * ((nowIFrame.transform as RectTransform).rect.height / 2*0.0216f+uiInfoFrameGoDownOffset);
            Vector3 camerapos = Camera.main.WorldToViewportPoint(nowIFrame.transform.position + Vector3.right * (nowIFrame.transform as RectTransform).rect.width  * 0.0216f);
            Debug.Log(camerapos);
            if (camerapos.x < 1)
            {
                nowIFrame.transform.position += Vector3.right * (nowIFrame.transform as RectTransform).rect.width / 2 * 0.0216f;
            }
            else
            {
                nowIFrame.transform.position -= Vector3.right * (nowIFrame.transform as RectTransform).rect.width / 2 * 0.0216f;
            }

        }
    }
    public void OnMouseExit()
    {
        if (nowIFrame)
        {
            Destroy(nowIFrame);
        }
    }
}
