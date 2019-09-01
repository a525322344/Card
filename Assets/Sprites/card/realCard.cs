using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class realCard : MonoBehaviour
{
    //生成时，传递给该手牌类的数据
    public int thisid;
    //
    public card thisCard;
    public RectTransform realCardMesh;
    public Text nameText;
    public Text describeText;

    private Vector3 initLocakscale;
    private Vector3 handPosition;
    public Vector3 targetPosition;

    private float maxMouseOnDistance;
    private float adjustMouseOnDistance;
    private float upfloatSpeed;
    private float moveSpeed;

    private bool _b_mouseEnter = false;
    private bool _b_selected = false;
    // Start is called before the first frame update
    void Start()
    {
        realCardMesh = GetComponent<RectTransform>();
        handPosition = transform.position;
        initLocakscale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (_b_mouseEnter)
        {
            realCardMesh.position = Vector3.MoveTowards(transform.position,handPosition , upfloatSpeed * Time.deltaTime);
        }
        else
        {
            realCardMesh.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        if (_b_selected)
        {

        }
    }

    public void SetThiscard(card playerCard)
    {
        thisCard = playerCard;
        nameText.text = playerCard.Name;
        describeText.text = playerCard.Describe;
    }
    /// <summary>
    /// 设置手牌移动效果参数
    /// </summary>
    /// <param name="_maxD">缓慢向上飘最大距离</param>
    /// <param name="_adjustD">放大的位置调整</param>
    /// <param name="_upSpeed">缓慢向上飘的速度</param>
    /// <param name="_moveSpeed">卡牌移动速度</param>
    public void SetCardMoveNum(float _maxD,float _adjustD,float _upSpeed,float _moveSpeed)
    {
        maxMouseOnDistance = _maxD;
        adjustMouseOnDistance = _adjustD;
        upfloatSpeed = _upSpeed;
        moveSpeed = _moveSpeed;
    }
    private void OnMouseEnter()
    {
        _b_mouseEnter = true;
        realCardMesh.position = targetPosition + Vector3.up * adjustMouseOnDistance;
        handPosition = targetPosition + Vector3.up * maxMouseOnDistance;
        realCardMesh.localScale = initLocakscale * 1.5f;
    }
    private void OnMouseExit()
    {
        _b_mouseEnter = false;
        realCardMesh.localScale = initLocakscale;
        realCardMesh.position = targetPosition;
    }
    private void OnMouseDown()
    {
        playthiscardTest();
        if (_b_selected)
        {
            //进入选择，
        }
        else
        {
            _b_selected = true;
        }        
    }

    void playthiscardTest()
    {

    }
}
