using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handcardControll : MonoBehaviour
{
    private RectTransform transform;
    public int cardCount;
    public float maxDistance;
    public float AllDistance;

    public float maxMouseOnDistance;
    public float adjustMouseOnDistance;

    public float upfloatSpeed;
    public float moveSpeed;
    public List<realCard> playerHandCards=new List<realCard>();
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        cardCount = playerHandCards.Count;
        foreach(realCard real in playerHandCards)
        {
            //临时实时传递数据，便于调教，最后删除
            real.SetCardMoveNum(maxMouseOnDistance, adjustMouseOnDistance, upfloatSpeed, moveSpeed);
        }
        if (cardCount * maxDistance <= AllDistance)
        {
            if (cardCount % 2 == 0) //偶数
            {
                for(int i = 0; i < cardCount / 2; i++)
                {
                    
                    float disleft = i - (cardCount - 1) / 2;
                    float disright = (cardCount - 1 - i) - (cardCount - 1) / 2;
                    playerHandCards[i].targetPosition = transform.position + new Vector3(1,0,0) * disleft * maxDistance;
                    playerHandCards[cardCount - 1 - i].targetPosition = transform.position + new Vector3(1, 0, 0) * disright * maxDistance;

                    
                }
            }
            else                                //奇数
            {

            }
        }
    }
}
