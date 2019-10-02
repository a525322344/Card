using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instantiateManager : MonoBehaviour
{
    public static instantiateManager instance
    {
        get
        {
            return _instance;
        }
    }
    private static instantiateManager _instance;

    public Canvas uiCanvas;
    public GameObject partGO;
    public GameObject cardGO;
    public GameObject gridGO;
    //牌库的位置
    public Transform dicktran;
    //手牌的位置
    public Transform handCardControll;
    public Transform bookFolderTran;
    //储存部件的坐标
    public List<Vector3> partPositionList = new List<Vector3>();

    public List<Transform> parttransforms = new List<Transform>();


    private void Awake()
    {
        _instance = GetComponent<instantiateManager>();
    }
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
            parttransforms[0].parent.GetComponent<bookFolderControll>().realparts.Add(realpart);
        }
    }

    public void instanDrawACard(card playercard)
    {
        GameObject card = Instantiate(cardGO, handCardControll);
        realCard realcard = card.GetComponentInChildren<realCard>();
        realcard.SetThiscard(playercard);
        realcard.ShowDraw();
        handCardControll.GetComponent<handcardControll>().playerHandCards.Add(realcard);
    }

}
