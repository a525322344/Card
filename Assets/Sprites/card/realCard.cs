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
    public float enterPosiY;
    public float scaleF = 1.6f;
    public float enterFloatUp;
    ///时间
    public float handswayTime = 1;
    public float cardrotateTime = 0.5f;
    public float scalechangeTime = 0.3f;
    public float floatupTime = 1;

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
                transform.DOScale(Vector3.one * scaleF, scalechangeTime);
                transform.DOMoveY(enterPosiY + enterFloatUp, floatupTime);
                break;
            case HandCardState.Select:
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
        enterPosiY = handcardControll.enterPosiY;
        scaleF = handcardControll.scaleF;
        enterFloatUp = handcardControll.enterFloatUp;
        handswayTime = handcardControll.handswayTime;
        cardrotateTime = handcardControll.cardrotateTime;
        scalechangeTime = handcardControll.scalechangeTime;
        floatupTime = handcardControll.floatupTime;
    }
    private void OnMouseEnter()
    {
        handCardState = HandCardState.Enter;
        transform.position = new Vector3(transform.position.x, enterPosiY, transform.position.z); 
    }
    private void OnMouseExit()
    {
        handCardState = HandCardState.Freedom;
    }
    private void OnMouseDown()
    {
    }

}
