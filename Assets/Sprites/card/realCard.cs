using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public enum HandCardState
{
    Freedom,
    Enter,
    Select,
}
public class realCard : MonoBehaviour
{
    public handcardControll handcardControll;
    public card thisCard;
    //UI
    //public RectTransform realCardMesh;
    public Text nameText;
    public Text describeText;

    // 旋转节点
    Transform father;
    // 根节点旋转角度
    private float adjustAngle;




    ///进入状态设置的值
    public float enter_cardPosiYSet;
    public float enter_cardScaleMultiple = 1.6f;
    public float enter_cardPosiYFloatUp;
    ///时间
    public float handswayTime = 1;
    public float cardrotateTime = 0.5f;
    public float scalechangeTime = 0.3f;
    public float floatupTime = 1;

    public float init_cardLocalPosiY;

    /// 记录初始信息
    private Vector3 localpositionStart;

    public HandCardState handCardState = HandCardState.Freedom;

    void Start()
    {
        father = transform.parent;
        //realCardMesh = GetComponent<RectTransform>();
        localpositionStart = transform.localPosition;
        handcardControll = father.parent.GetComponent<handcardControll>();
    }

    // Update is called once per frame
    void Update()
    {
        recesiveInfo();
        switch (handCardState)
        {
            case HandCardState.Freedom:
                father.DORotate(new Vector3(0, 0, adjustAngle), handswayTime);
                transform.DORotate(new Vector3(0, 0, adjustAngle), cardrotateTime);
                transform.DOScale(Vector3.one, scalechangeTime);
                transform.DOLocalMove(localpositionStart, handswayTime);
                break;
            case HandCardState.Enter:
                father.DORotate(new Vector3(0, 0, adjustAngle), handswayTime);
                transform.DORotate(Vector3.zero, cardrotateTime);
                transform.DOScale(Vector3.one * enter_cardScaleMultiple, scalechangeTime);
                transform.DOMoveY(init_cardLocalPosiY + enter_cardPosiYSet + enter_cardPosiYFloatUp, floatupTime);
                break;
            case HandCardState.Select:
                Vector3 mouseposition = Input.mousePosition; 
                mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(mouseposition.x,mouseposition.y,instantiateManager.instance.uiCanvas.planeDistance));
                transform.DOMove(mouseposition,0.1f);
                transform.DOScale(Vector3.one*1.1f, 0.1f);
                break;
        }
    }

    public void SetThiscard(card playerCard)
    {
        thisCard = playerCard;
        nameText.text = playerCard.Name;
        describeText.text = playerCard.Describe;
    }
    /// <summary>
    /// 设置手牌转动角度
    /// </summary>
    /// <param name="angle"></param>
    public void SetCardMoveNum(float angle)
    {
        adjustAngle = angle;
    }
    private void recesiveInfo()
    {
        enter_cardPosiYSet = handcardControll.enter_cardPosiYSet;
        enter_cardScaleMultiple = handcardControll.enter_cardScaleMultiple;
        enter_cardPosiYFloatUp = handcardControll.enter_cardPosiYFloatUp;
        handswayTime = handcardControll.handswayTime;
        cardrotateTime = handcardControll.cardrotateTime;
        scalechangeTime = handcardControll.scalechangeTime;
        floatupTime = handcardControll.floatupTime;
        init_cardLocalPosiY = handcardControll.init_cardLocalPosiY;

        localpositionStart = transform.parent.localPosition + Vector3.up * handcardControll.radiues;
    }

    public void StateSelect_Freedom()
    {
        handCardState = HandCardState.Freedom;
    }
    private void OnMouseEnter()
    {
        switch (handCardState)
        {
            case HandCardState.Enter:
                break;
            case HandCardState.Freedom:
                handCardState = HandCardState.Enter;
                transform.position = new Vector3(transform.position.x, init_cardLocalPosiY + enter_cardPosiYSet, transform.position.z);
                break;
            case HandCardState.Select:

                break;
        }
         
    }
    private void OnMouseExit()
    {
        switch (handCardState)
        {
            case HandCardState.Enter:
            case HandCardState.Freedom:
                handCardState = HandCardState.Freedom;
                break;
            case HandCardState.Select:

                break;
        }
    }
    private void OnMouseDown()
    {
        handCardState = HandCardState.Select;
        handcardControll.SetSelectCard(this);
        Debug.Log(Camera.main);
    }

}
