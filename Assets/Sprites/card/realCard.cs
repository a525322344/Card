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
    private RectTransform transform;
    public Text nameText;
    public Text describeText;

    private Vector3 initLocakscale;
    public Vector3 handPosition;
    public Vector3 handRotation;

    public Vector3 targetPosition;
    public float maxDistance;
    public float adjustDistance;

    public float upSpeed;
    public float moveSpeed;
    private bool _b_mouseEnter = false;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<RectTransform>();
        handPosition = transform.position;
        initLocakscale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (_b_mouseEnter)
        {
            transform.position = Vector3.MoveTowards(transform.position,handPosition ,upSpeed*Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
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
        transform.position = targetPosition + Vector3.up * adjustDistance;
        handPosition = targetPosition + Vector3.up * maxDistance;
        transform.localScale = initLocakscale * 2;
    }
    private void OnMouseExit()
    {
        _b_mouseEnter = false;
        transform.localScale = initLocakscale;
        transform.position = targetPosition;
    }
}
