using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateManager : MonoBehaviour
{

    public GameObject partGO;
    public GameObject cardGO;
    public GameObject gridGO;

    //手牌的位置
    public Transform handCardControll;

    //储存部件的坐标
    public List<Vector3> partPositionList = new List<Vector3>();

    public List<Transform> parttransforms = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instanBattleStartPart(List<MagicPart> magicParts)
    {
        for(int i=0;i<magicParts.Count;i++)
        {
            GameObject part = Instantiate(partGO, parttransforms[i]);
            realpart realpart = part.GetComponent<realpart>();
            realpart.setThisMagicPart(magicParts[i]);
        }
    }

    public void instanDrawACard(card playercard)
    {
        GameObject card = Instantiate(cardGO, handCardControll);
        card.GetComponent<realCard>().SetThiscard(playercard);
        handCardControll.GetComponent<handcardControll>().playerHandCards.Add(card.GetComponent<realCard>());
    }
}
