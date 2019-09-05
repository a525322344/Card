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
    public List<realCard> playerHandCards = new List<realCard>();
    //手牌数量
    public int cardnum;


    // 要向realcard传递的参数
    #region 卡牌移动的相关参数
    ///进入状态设置的值
    public float enterPosiY;
    public float scaleF = 1.6f;
    public float enterFloatUp;
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


    private void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        cardnum = playerHandCards.Count;

        float betweenAngle = allAngle / (playerHandCards.Count - 1);
        if (betweenAngle > defuatAngle)
        {
            betweenAngle = defuatAngle;
        }
        float angleIndex = (float)-(cardnum - 1) / 2;
        bool needAbdicate = false;

        for(int i = 0; i < playerHandCards.Count; i++)
        {
            float angle;
            if (playerHandCards[i].handCardState == HandCardState.Enter)
            {
                needAbdicate = true;
                angle = (angleIndex + i) * betweenAngle;
                playerHandCards[i].SetCardMoveNum(-angle);
                for(int j = 0; j < i; j++)
                {
                    angle = (angleIndex + j) * betweenAngle - abdicateAngle;
                    playerHandCards[j].SetCardMoveNum(-angle);
                }
                for(int j = i + 1; j < playerHandCards.Count; j++)
                {
                    angle = (angleIndex + j) * betweenAngle + abdicateAngle;
                    playerHandCards[j].SetCardMoveNum(-angle);
                }
            }
        }
        if (!needAbdicate)
        {
            for (int i = 0; i < cardnum; i++)
            {
                float angle = (angleIndex + i) * betweenAngle;
                playerHandCards[i].SetCardMoveNum(-angle);
            }
        }
    }
}
