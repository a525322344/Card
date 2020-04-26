using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInfoFrame : MonoBehaviour
{
    public Text text;
    public float outWidth;
    private Image Image;
    // Start is called before the first frame update
    public void Init(stateAbstarct state)
    {
        Image = GetComponent<Image>();
        text.text = state.DescribeState();
        Image.rectTransform.sizeDelta = new Vector2(Image.rectTransform.sizeDelta.x, text.rectTransform.sizeDelta.y * text.rectTransform.localScale.x + outWidth);
    }


    void Start()
    {
        Image = GetComponent<Image>();
    }
    //Update is called once per frame
    void Update()
    {
        Image.rectTransform.sizeDelta = new Vector2(Image.rectTransform.sizeDelta.x, text.rectTransform.sizeDelta.y * text.rectTransform.localScale.x + outWidth);
    }
}
