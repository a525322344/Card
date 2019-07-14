using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handcardControll : MonoBehaviour
{
    private RectTransform transform;
    public int cardCount;
    public float maxDistance;
    public float AllDistance;
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
                    //playerHandCards[i].handPosition = playerHandCards[i].targetPosition;
                    //playerHandCards[cardCount - 1 - i].handPosition = playerHandCards[cardCount - 1 - i].targetPosition;
                    //Debug.Log("" + disleft + "   " + disright);
                }
            }
            else                                //奇数
            {

            }
        }
    }
}
