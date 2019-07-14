using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ListOperation
{
    public static List<playerCard> shuffle(List<playerCard> playerDeck)
    {
        //想要排序的List 
        List<playerCard> now = new List<playerCard>(playerDeck);
        //排序之后的List 
        List<playerCard> get = new List<playerCard>();
        //为了降低运算的数量级，当执行完一个元素时，就需要把此元素从原List中移除
        int countNum = now.Count;
        //使用while循环，保证将a中的全部元素转移到b中而不产生遗漏
        while (get.Count < countNum)
        {
            //随机将a中序号为index的元素作为b中的第一个元素放入b中
            int index = Random.Range(0, now.Count);

            //若b中还没有此元素，添加到b中
            get.Add(now[index]);
            //成功添加后，将此元素从a中移除，避免重复取值
            now.Remove(now[index]);
            
        }
        return get;
    }
}
