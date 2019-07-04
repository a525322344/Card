using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AllAsset
{
    public static class effectAsset
    {
        public static void addDemage(int num)
        {
            Debug.Log("造成" + num + "点伤害");
        }
    }
}

public class cardAsset : MonoBehaviour
{
    //所有卡
    public playerCard[] AllIdCard;
    private void Start()
    {
        AllIdCard[0] = new playerCard(0,"法力冲击",CardKind.PlayerCard,new CardEffect(AllAsset.effectAsset.addDemage));
    }
}
