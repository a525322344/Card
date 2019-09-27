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
    SelectOut,
    Other
}
public class realCard : MonoBehaviour
{
    public handcardControll handcardControll;
    public realCost realcost;
    public card thisCard;
    //UI
    //public RectTransform realCardMesh;
    public Text nameText;
    public Text describeText;

    // 旋转节点
    Transform father;
    Transform cardmesh;
    // 根节点旋转角度
    private float adjustAngle;
    private int handorder;
    private const float deviationZ = 0.1f;
    //

    ///进入状态设置的值
    private float enter_cardPosiYSet;
    private float enter_cardScaleMultiple = 1.6f;
    private float enter_cardPosiYFloatUp;

    private float free_positionZ;
    ///时间
    private float handswayTime = 1;
    private float cardrotateTime = 0.5f;
    private float scalechangeTime = 0.3f;
    private float floatupTime = 1;

    private float init_cardLocalPosiY;

    /// 记录初始信息
    private Vector3 localpositionStart;

    public HandCardState handCardState = HandCardState.Freedom;

    private Vector3 startmeshsalce;

    void Start()
    {
        father = transform.parent;
        cardmesh = transform.GetChild(0);
        //realCardMesh = GetComponent<RectTransform>();
        localpositionStart = transform.localPosition;
        handcardControll = father.parent.GetComponent<handcardControll>();
        startmeshsalce = cardmesh.localScale;
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
                //transform.localPosition = localpositionStart + Vector3.forward * handorder * deviationZ;
                transform.DOLocalMove(localpositionStart+Vector3.forward*handorder*deviationZ, handswayTime);

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
                transform.DOMove(mouseposition+Vector3.back*1,0.1f);
                transform.DOScale(Vector3.one, 0.1f);

                cardmesh.localPosition = Vector3.zero;
                cardmesh.localScale = startmeshsalce;
                if (IsOutOfHandPlace())
                {
                    handCardState = HandCardState.SelectOut;

                    cardmesh.SetParent(cardmesh.parent.parent,true);
                    cardmesh.DOMove(handcardControll.showCardPosition.position, 0.1f);
                    cardmesh.DOScale(startmeshsalce * 2, 0.1f);
                    realcost.gameObject.SetActive(true);

                    handcardControll.SelectCardOut();
                }

                break;
            case HandCardState.SelectOut:
                mouseposition = Input.mousePosition;
                mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(mouseposition.x, mouseposition.y, instantiateManager.instance.uiCanvas.planeDistance));
                transform.DOMove(mouseposition + Vector3.back * 1, 0);
                transform.DOScale(Vector3.one*1.1f, 0.1f);

                if (IsOutOfHandPlace() == false)
                {
                    handCardState = HandCardState.Select;

                    cardmesh.SetParent(transform,true); 
                    cardmesh.localPosition = Vector3.zero;
                    cardmesh.localScale = startmeshsalce;
                    realcost.gameObject.SetActive(false);
                }
                break;
        }
    }
    //供创建时使用
    public void SetThiscard(card playerCard)
    {
        thisCard = playerCard;
        nameText.text = playerCard.Name;
        describeText.text = playerCard.Describe;

        realcost.setCost(thisCard);
        realcost.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置手牌转动角度及次序
    /// </summary>
    /// <param name="angle"></param>
    public void SetCardMoveNum(float angle,int order)
    {
        adjustAngle = angle;
        handorder = order;
    }

    /// <summary>
    /// 接受handcardControll手牌飘动设置信息
    /// 为方便调整，统一将数据设置在了handcardControll里【最终确定后，可以优化】
    /// </summary>
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

    //供handcardControll调用
    public void StateSelect_Freedom()
    {
        handCardState = HandCardState.Other;

        transform.position = cardmesh.position;
        handCardState = HandCardState.Freedom;
        cardmesh.SetParent(transform, true);
        cardmesh.localPosition = Vector3.zero;
        cardmesh.localScale = startmeshsalce;
        realcost.gameObject.SetActive(false);

    }

    private bool IsOutOfHandPlace()
    {
        if (Vector3.Distance(transform.position, handcardControll.handPlace.position) > handcardControll.handPlace.localScale.x / 2 * handcardControll.handPlace.parent.parent.localScale.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnMouseEnter()
    {
        
        switch (handCardState)
        {
            case HandCardState.Enter:
                break;
            case HandCardState.Freedom:
                if (handcardControll.selectedCard == null)
                {
                    handCardState = HandCardState.Enter;
                    transform.position = new Vector3(transform.position.x, init_cardLocalPosiY + enter_cardPosiYSet, transform.position.z);
                }               
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
                if (handcardControll.selectedCard == null)
                {
                    handCardState = HandCardState.Freedom;
                }               
                break;
            case HandCardState.Select:

                break;
        }
    }
    private void OnMouseDown()
    {
        switch (handCardState)
        {
            case HandCardState.Enter:
                handCardState = HandCardState.Select;
                handcardControll.SetSelectCard(this);
                break;
            case HandCardState.Freedom:
                
                break;
            case HandCardState.Select:

                break;
        }
    }
}
