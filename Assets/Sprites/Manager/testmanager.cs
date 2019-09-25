using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmanager : MonoBehaviour
{
    public List<playerCard> a = new List<playerCard>();
    public List<playerCard> b;
    // Start is called before the first frame update
    void Start()
    {
        a.Add(AllAsset.cardAsset.AllIdCards[1]);
        a.Add(AllAsset.cardAsset.AllIdCards[2]);
        b = new List<playerCard>(a);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
