using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public enum RealCardState
{
    Other,
    RealCard,
    AwardCard,
    SelectCard,
    ShowCard,
}
public enum HandCardState
{
    Draw,
    DisCard,
    Freedom,
    Enter,                  //光标飘在卡上，卡片放大
    Select,                 //点选到
    SelectOut,
    WaitToSelectFree,       //等待点选到选卡区
    WaitToSelectEnter,      //等待点选到选卡区，光标漂浮
    InSelectBoard,          //处于选卡区
    Other
}
public class realCard : MonoBehaviour
{
    //外部引用
    public Transform costtran;
    public SpriteRenderer cardTexture;
    public SpriteRenderer cardBoard;
    //public SpriteRenderer cardKindIcon;
    //资源引用
    public Sprite[] cardBoardSprites;
    //public Sprite[] cardKindIconSprites;
    public GameObject[] costGO;
    public GameObject trailGO;
    private List<SpriteRenderer> costrenders = new List<SpriteRenderer>();

    public TextMeshPro nameTextPro;
    public TextMeshPro describeTextPro;
    //资源
    public int handorder;
    public handcardControll handcardControll;
    public realCost realcost;

    public card thisCard;
    public thingToSelect<playerCard> cardselects;

    #region 表现参数
    // 旋转节点
    Transform father;
    Transform cardmesh;

    Transform toSelectPointTran;
    public Selection selection;

    public float initdownmove = 60;
    // 根节点旋转角度
    private float adjustAngle;
    private const float deviationZ = 30;
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
    private Vector3 localpositionMesh;
    private Vector3 startmeshsalce;
    #endregion
    public RealCardState realCardState = RealCardState.Other;
    public HandCardState handCardState = HandCardState.Other;



    void Start()
    {
        father = transform.parent;
        cardmesh = transform.GetChild(0);
        localpositionStart = transform.localPosition;
        handcardControll = father.parent.GetComponent<handcardControll>();
        startmeshsalce = cardmesh.localScale;
        localpositionMesh = cardmesh.localPosition;
    }
    float timecount;
    // Update is called once per frame
    void Update()
    {
        switch (realCardState)
        {
            case RealCardState.RealCard:
                realcardUpdate();
                break;
            case RealCardState.AwardCard:
                break;
            case RealCardState.SelectCard:
                switch (handCardState)
                {
                    case HandCardState.Freedom:
                        transform.DOScale(Vector3.one, 0.1f);
                        break;
                    case HandCardState.Enter:
                        transform.DOScale(Vector3.one * 1.1f, 0.1f);
                        break;
                }
                break;
        }

    }

    void realcardUpdate()
    {
        recesiveInfo();
        switch (handCardState)
        {
            case HandCardState.Draw:
                break;
            case HandCardState.Freedom:
            case HandCardState.WaitToSelectFree:
                father.DORotate(new Vector3(0, 0, adjustAngle), handswayTime);
                transform.DORotate(new Vector3(0, 0, adjustAngle), cardrotateTime);
                transform.DOScale(Vector3.one, scalechangeTime);
                //transform.localPosition = localpositionStart + Vector3.forward * handorder * deviationZ;
                transform.DOLocalMove(localpositionStart + Vector3.forward * handorder * deviationZ, handswayTime);
                break;
            case HandCardState.Enter:
            case HandCardState.WaitToSelectEnter:
                father.DORotate(new Vector3(0, 0, adjustAngle), handswayTime);
                transform.DORotate(Vector3.zero, cardrotateTime);
                transform.DOScale(Vector3.one * enter_cardScaleMultiple, scalechangeTime);
                transform.DOMoveY(init_cardLocalPosiY + enter_cardPosiYSet + enter_cardPosiYFloatUp, floatupTime);
                break;
            case HandCardState.Select:
                Vector3 mouseposition = Input.mousePosition;
                mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(mouseposition.x, mouseposition.y, instantiateManager.instance.battleuiRoot.uiCanvas.planeDistance));
                transform.DOMove(mouseposition + Vector3.back * 1, 0);
                transform.DOScale(Vector3.one, 0.1f);

                cardmesh.localPosition = Vector3.zero;
                cardmesh.localScale = startmeshsalce;
                if (IsOutOfHandPlace())
                {
                    handCardState = HandCardState.SelectOut;

                    cardmesh.SetParent(cardmesh.parent.parent, true);
                    cardmesh.DOMove(handcardControll.showCardPosition.position, 0.1f);
                    cardmesh.DOScale(startmeshsalce * 2, 0.1f);
                    realcost.gameObject.SetActive(true);

                    handcardControll.SelectCardOut();
                }

                break;
            case HandCardState.InSelectBoard:
                //卡牌飘到选择区
                father.DORotate(new Vector3(0, 0, 0), 0.1f);
                transform.DORotate(new Vector3(0, 0, 0), 0.1f);
                transform.DOScale(Vector3.one, 0.1f);
                //transform.localPosition = localpositionStart + Vector3.forward * handorder * deviationZ;
                transform.DOMove(selection.PositionTran.position, 0.15f);
                break;
            case HandCardState.SelectOut:
                mouseposition = Input.mousePosition;
                mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(mouseposition.x, mouseposition.y, instantiateManager.instance.battleuiRoot.uiCanvas.planeDistance));
                transform.DOMove(mouseposition + Vector3.back * 1, 0);
                transform.DOScale(Vector3.one * 1.1f, 0.1f);

                if (Input.GetAxis("Mouse ScrollWheel") < 0)//下
                {
                    realcost.RotateCost(-1);
                }
                if (Input.GetAxis("Mouse ScrollWheel") > 0)//上
                {
                    realcost.RotateCost(1);
                }

                if (IsOutOfHandPlace() == false)
                {
                    handCardState = HandCardState.Select;

                    cardmesh.SetParent(transform, true);
                    cardmesh.localPosition = Vector3.zero;
                    cardmesh.localScale = startmeshsalce;
                    realcost.gameObject.SetActive(false);
                }
                break;
        }
    }
    //供创建时使用

    public void Init(card playerCard,RealCardState _realCardState)
    {
        realCardState = _realCardState;
        thisCard = playerCard;
        foreach(EffectBase effect in (playerCard as playerCard).EffectPlayList)
        {
            effect.InitMixNum();
        }
        nameTextPro.text = playerCard.Name;
        describeTextPro.text = (playerCard as playerCard).CardDescribe();
        cardTexture.sprite = gameManager.Instance.instantiatemanager.cardSprites[playerCard.TextureId];
        Transform costTran = transform;
        switch (playerCard.Kind)
        {
            case CardKind.AttackCard:
                cardBoard.sprite = cardBoardSprites[0];
                costTran = Instantiate(costGO[playerCard.Cost], costtran).transform;
                for(int i = 0; i < costTran.childCount; i++)
                {
                    costrenders.Add(costTran.GetChild(i).GetComponent<SpriteRenderer>());
                }
                break;
            case CardKind.SkillCard:
                cardBoard.sprite = cardBoardSprites[1];
                costTran = Instantiate(costGO[playerCard.Cost], costtran).transform;
                for (int i = 0; i < costTran.childCount; i++)
                {
                    costrenders.Add(costTran.GetChild(i).GetComponent<SpriteRenderer>());
                }
                break;
            case CardKind.CurseCard:
                cardBoard.sprite = cardBoardSprites[0];
                //cardKindIcon.gameObject.SetActive(false);
                break;
        }

        realcost.Init(thisCard);
        realcost.gameObject.SetActive(false);
        if (realCardState == RealCardState.RealCard)
        {
            gameManager.Instance.battlemanager.setCardDescribe(this, new MagicPart());
        }
        else if (realCardState == RealCardState.AwardCard)
        {

        }
        else if (realCardState == RealCardState.SelectCard)
        {
            //cardKindIcon.GetComponent<SpriteRenderer>().sortingOrder = 0;
            handCardState = HandCardState.Freedom;
        }
        else if (realCardState == RealCardState.ShowCard)
        {
            //cardKindIcon.GetComponent<SpriteRenderer>().sortingOrder = 0;
            handCardState = HandCardState.Freedom;
        }
    }
    public void orderCost()
    {
        foreach(var sr in costrenders)
        {
            sr.sortingOrder = 1;
        }
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
        //cardKindIcon.GetComponent<SpriteRenderer>().sortingOrder = 0;
        transform.position = cardmesh.position;
        handCardState = HandCardState.Freedom;
        cardmesh.SetParent(transform, true);
        cardmesh.localPosition = localpositionMesh;
        cardmesh.localScale = startmeshsalce;
        realcost.gameObject.SetActive(false);

    }
    //牌从卡组抽出来、动画效果
    public void ShowDraw()
    {
        DOTween.To(() => timecount, a => timecount = a, 1, 0.1f).OnComplete(() =>
        {
            handCardState = HandCardState.Freedom;
        });
        transform.position = gameManager.Instance.instantiatemanager.battleuiRoot.dicktran.position;
        handCardState = HandCardState.Draw;
    }
    //alpha渐变
    public void AlphaAnima(float a,float time=0)
    {
        Color tocolor;
        tocolor=new Color(cardTexture.color.r, cardTexture.color.g, cardTexture.color.b, a);
        DOTween.To(() => cardTexture.color, x => cardTexture.color = x, tocolor, time);
        tocolor = new Color(cardBoard.color.r, cardBoard.color.g, cardBoard.color.b, a);
        DOTween.To(() => cardBoard.color, x => cardBoard.color = x, tocolor, time);
        foreach(SpriteRenderer sr in costrenders)
        {
            tocolor = new Color(sr.color.r, sr.color.g, sr.color.b, a);
            DOTween.To(() => sr.color, x => sr.color = x, tocolor, time);
        }
        tocolor = new Color(nameTextPro.color.r, nameTextPro.color.g, nameTextPro.color.b, a);
        DOTween.To(() => nameTextPro.color, x => nameTextPro.color = x, tocolor, time);
        tocolor = new Color(describeTextPro.color.r, describeTextPro.color.g, describeTextPro.color.b, a);
        DOTween.To(() => describeTextPro.color, x => describeTextPro.color = x, tocolor, time);
    }

    //用于进入等待选择状态
    public void EnterStateWaitSelect()
    {
        handCardState = HandCardState.WaitToSelectFree;
    }
    public void ExitStateWaitSelect()
    {
        handCardState = HandCardState.Freedom;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="i">1-得到、2-删除</param>
    public void ShowCard(int i)
    {
        if (i == 1)
        {
            transform.localPosition = new Vector3(0, -initdownmove, 0);
            transform.DOLocalMoveY(0, 0.25f).onComplete += () =>
            {
                GameObject t = Instantiate(trailGO, transform);
                AlphaAnima(0, 0.15f);
                t.GetComponent<TrailControll>().StartMove(instantiateManager.instance.mapRootInfo.uiMapContrill.cardui.transform);
                //t.transform.DOMove(instantiateManager.instance.mapRootInfo.uiMapContrill.cardui.transform.position, 1f);
            };
            Destroy(transform.parent.gameObject,1.5f);
        }
        else if (i == 2)
        {
            transform.localPosition = new Vector3(0, -initdownmove, 0);
            transform.DOLocalMoveY(0, 0.25f).onComplete += () =>
            {
                AlphaAnima(0, 0.8f);;
            };
            Destroy(transform.parent.gameObject, 1.5f);
        }
    }
    private void OnMouseEnter()
    {
        if (gameManager.Instance.gameState == GameState.BattleSence)
        {
            if (gameManager.Instance.battlemanager.BattleRound == BattleState.Vectory)
                return;
        }

        switch (realCardState)
        {
            case RealCardState.RealCard:
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
                    case HandCardState.WaitToSelectFree:
                        handCardState = HandCardState.WaitToSelectEnter;
                        transform.position = new Vector3(transform.position.x, init_cardLocalPosiY + enter_cardPosiYSet, transform.position.z);
                        break;
                    case HandCardState.Select:

                        break;
                }
                break;
            case RealCardState.AwardCard:
                break;
            case RealCardState.SelectCard:
                switch (handCardState)
                {
                    case HandCardState.Freedom:
                    case HandCardState.Enter:
                        handCardState = HandCardState.Enter;
                        break;
                }
                break;
        }
    }
    private void OnMouseExit()
    {
        if (gameManager.Instance.gameState == GameState.BattleSence)
        {
            if (gameManager.Instance.battlemanager.BattleRound == BattleState.Vectory)
                return;
        }
        switch (realCardState)
        {
            case RealCardState.RealCard:
                switch (handCardState)
                {
                    case HandCardState.Enter:
                    case HandCardState.Freedom:
                        if (handcardControll.selectedCard == null)
                        {
                            handCardState = HandCardState.Freedom;
                        }
                        break;
                    case HandCardState.WaitToSelectEnter:
                    case HandCardState.WaitToSelectFree:
                        handCardState = HandCardState.WaitToSelectFree;
                        break;
                    case HandCardState.Select:
                        break;
                }
                break;
            case RealCardState.AwardCard:
                break;
            case RealCardState.SelectCard:
                switch (handCardState)
                {
                    case HandCardState.Freedom:
                    case HandCardState.Enter:
                        handCardState = HandCardState.Freedom;
                        break;
                }
                break;
        }

    }
    private void OnMouseDown()
    {
        if (gameManager.Instance.gameState == GameState.BattleSence)
        {
            if (gameManager.Instance.battlemanager.BattleRound == BattleState.Vectory)
                return;
        }
        switch (realCardState)
        {
            case RealCardState.RealCard:
                switch (handCardState)
                {
                    case HandCardState.Enter:
                        if (thisCard.Kind != CardKind.CurseCard)
                        {
                            handCardState = HandCardState.Select;
                            //cardKindIcon.GetComponent<SpriteRenderer>().sortingOrder = 1;
                            handcardControll.SetSelectCard(this);
                        }
                        break;
                    case HandCardState.WaitToSelectEnter:
                        //被选到
                        if (gameManager.Instance.battlemanager.battleInfo.realWaitSelectCard.SelectOne(handorder - 1, out selection))
                        {
                            handCardState = HandCardState.InSelectBoard;
                        }
                        break;
                    case HandCardState.InSelectBoard:
                        //取消选择
                        selection.b_isNull = true;
                        selection.saveCardnum = -1;
                        selection = null;
                        handCardState = HandCardState.WaitToSelectFree;
                        break;
                    case HandCardState.Freedom:
                        break;
                    case HandCardState.Select:

                        break;
                }
                break;
            case RealCardState.AwardCard:
                break;
            case RealCardState.SelectCard:
                switch (handCardState)
                {
                    case HandCardState.Freedom:
                    case HandCardState.Enter:
                        cardselects.selecThisCard(thisCard as playerCard);
                        break;
                }
                break;
        }

    }
}
