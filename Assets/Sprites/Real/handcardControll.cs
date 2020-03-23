using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handcardControll : MonoBehaviour
{
    public static handcardControll instance
    {
        get
        {
            return _instance;
        }
    }
    private static handcardControll _instance;

    private RectTransform rectTransform;
    [HideInInspector]
    public List<realCard> playerHandCards = new List<realCard>();
    public Transform handcardspaceSphere;
    public Transform handPlace;
    public Transform showCardPosition;
    private Vector3 sphereCenter;
    [HideInInspector]
    public float radiues;
    //手牌数量
    public int cardnum;


    // 要向realcard传递的参数
    #region 卡牌移动的相关参数
    ///进入状态设置的值
    public float enter_cardPosiYSet;            //鼠标进入时，卡片瞬间上移距离
    public float enter_cardScaleMultiple = 1.6f;//鼠标进入时，卡片瞬间变大倍数
    public float enter_cardPosiYFloatUp;        //鼠标进入后，卡片上飘距离
    [HideInInspector]
    public float init_cardLocalPosiY;
    ///时间
    public float handswayTime = 1;
    public float cardrotateTime = 0.5f;
    public float scalechangeTime = 0.3f;
    public float floatupTime = 1;
    #endregion
    //
    public float allAngle;                  //手牌能占的最大的区域（用角度所得
    public float defuatAngle;               //手牌数少的时候，牌与牌之间默认角度
    public float abdicateAngle;             //微调的角度

    public realCard selectedCard;

    private void Awake()
    {
        _instance = this;
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
       
    }

    void Update()
    {
        GetHandcardspaceSphereInfo();       
        PlayRealCardManage();
        //按下鼠标右键，取消选择的卡牌
        if (Input.GetMouseButtonDown(1))
        {
            if (selectedCard)
            {
                selectedCard.StateSelect_Freedom();
                if (selectedCard.realcost.lastRealLatice)
                {
                    selectedCard.realcost.lastRealLatice.realknapsack.ToSetPart(null);
                    selectedCard.realcost.lastRealLatice = null;
                }

                gameManager.Instance.battlemanager.setCardDescribe(new MagicPart());
                selectedCard = null;
                gameManager.Instance.battlemanager.b_isSelectCard = false;            
            }
        }
    }
    /// <summary>
    /// 实时获得协助球体的信息【最终确定下来，可以优化这里】
    /// </summary>
    public void GetHandcardspaceSphereInfo()
    {
        sphereCenter = handcardspaceSphere.position;
        radiues = handcardspaceSphere.localScale.x / 2;

        init_cardLocalPosiY = sphereCenter.y + radiues*transform.parent.localScale.y;

        transform.position = handcardspaceSphere.position;
    }
    /// <summary>
    /// 计算卡牌的旋转位置
    /// </summary>
    private void PlayRealCardManage()
    {
        cardnum = playerHandCards.Count;
        float betweenAngle = allAngle / (playerHandCards.Count - 1);
        if (betweenAngle > defuatAngle)
        {
            betweenAngle = defuatAngle;
        }
        float angleIndex = (float)-(cardnum - 1) / 2;
        bool needAbdicate = false;

        for (int i = 0; i < playerHandCards.Count; i++)
        {
            float angle;
            if (playerHandCards[i].handCardState == HandCardState.Enter||playerHandCards[i].handCardState==HandCardState.WaitToSelectEnter)
            {
                needAbdicate = true;
                angle = (angleIndex + i) * betweenAngle;
                playerHandCards[i].SetCardMoveNum(-angle,i+1);
                for (int j = 0; j < i; j++)
                {
                    angle = (angleIndex + j) * betweenAngle - abdicateAngle;
                    playerHandCards[j].SetCardMoveNum(-angle,j+1);
                }
                for (int j = i + 1; j < playerHandCards.Count; j++)
                {
                    angle = (angleIndex + j) * betweenAngle + abdicateAngle;
                    playerHandCards[j].SetCardMoveNum(-angle,j+1);
                }
            }
        }
        if (!needAbdicate)
        {
            for (int i = 0; i < cardnum; i++)
            {
                float angle = (angleIndex + i) * betweenAngle;
                playerHandCards[i].SetCardMoveNum(-angle,i+1);
            }
        }
    }
    /// <summary>
    /// 设置选择的卡牌
    /// realCard调用;通知battleManager
    /// </summary>
    /// <param name="realcard"></param>
    public void SetSelectCard(realCard realcard)
    {
        selectedCard = realcard;
        gameManager.Instance.battlemanager.SetSelectedCard(realcard);
        if (realcard)
        {
            gameManager.Instance.battlemanager.b_isSelectCard = true;
        }      
    }
    public void SelectCardOut()
    {
        //gameManager.Instance.battlemanager.b_isSelectCard = true;
    }
}
