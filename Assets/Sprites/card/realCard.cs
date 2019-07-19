using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class realCard : MonoBehaviour
{
    //生成时，传递给该手牌类的数据
    public int thisid;
    //
    public playerCard thisCard;
    public RectTransform realCardMesh;
    public Text nameText;
    public Text describeText;

    private Vector3 initLocakscale;
    private Vector3 handPosition;
    private Vector3 handRotation;
    public Vector3 targetPosition;

    public float maxDistance;
    public float adjustDistance;

    public float upSpeed;
    public float moveSpeed;
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
            realCardMesh.position = Vector3.MoveTowards(transform.position,handPosition ,upSpeed*Time.deltaTime);
        }
        else
        {
            realCardMesh.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        if (_b_selected)
        {

        }
    }

    public void SetThiscard(playerCard playerCard)
    {
        thisCard = playerCard;
        nameText.text = playerCard.Name;
        describeText.text = playerCard.Describe;
    }
    private void OnMouseEnter()
    {
        _b_mouseEnter = true;
        realCardMesh.position = targetPosition + Vector3.up * adjustDistance;
        handPosition = targetPosition + Vector3.up * maxDistance;
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
        if (_b_selected)
        {
            //进入选择，
        }
        else
        {
            _b_selected = true;
        }        
    }
}
