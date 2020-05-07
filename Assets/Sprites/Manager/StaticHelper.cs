using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
            int index = UnityEngine.Random.Range(0, now.Count);

            //若b中还没有此元素，添加到b中
            get.Add(now[index]);
            //成功添加后，将此元素从a中移除，避免重复取值
            now.Remove(now[index]);
            
        }
        return get;
    }
    /// <summary>
    /// 随机排序链表<泛型>
    /// </summary>
    /// <typeparam name="T_class"></typeparam>
    /// <param name="tList"></param>
    /// <returns></returns>
    public static List<T_class> Shufle<T_class>(List<T_class> tList)
    {
        List<T_class> temporary = new List<T_class>(tList);
        List<T_class> result = new List<T_class>();
        int countNum = tList.Count;
        //Debug.Log(countNum);
        while (result.Count < countNum)
        {
            int index = UnityEngine.Random.Range(0, temporary.Count);
            result.Add(temporary[index]);
            temporary.Remove(temporary[index]);
        }
        return result;
    }
    /// <summary>
    /// 随机返回链表中的一个值
    /// </summary>
    /// <typeparam name="T_class"></typeparam>
    /// <param name="tList"></param>
    /// <returns></returns>
    public static T_class RandomValue<T_class>(List<T_class> tList)
    {
        return tList[UnityEngine.Random.Range(0, tList.Count)];
    }
    /// <summary>
    /// 随机选择项，返回num长度的链表
    /// </summary>
    /// <typeparam name="T_class"></typeparam>
    /// <param name="tlist">母表</param>
    /// <param name="num">选择长度</param>
    /// <returns></returns>
    public static List<T_class> RandomValueList<T_class>(List<T_class> tlist,int num)
    {
        List<T_class> alllist = new List<T_class>(tlist);
        List<T_class> returnlist = new List<T_class>();
        for(int i = 0; i < num;)
        {
            if (alllist.Count == 0)
            {
                Debug.Log("总池不足，只选择数：" + returnlist.Count);
                break;
            }
            T_class newt = RandomValue<T_class>(alllist);
            if (!returnlist.Contains(newt))
            {
                returnlist.Add(newt);
                alllist.Remove(newt);
                i++;
            }
        }
        return returnlist;
    }
    /// <summary>
    /// 将整个链表插入另一个链表中
    /// </summary>
    /// <typeparam name="T_class"></typeparam>
    /// <param name="tListA"></param>
    /// <param name="tListB"></param>
    public static void InsertList<T_class>(List<T_class> tListA,List<T_class> tListB)
    {
        foreach(T_class t in tListB)
        {
            tListA.Add(t);
        }
    }
    public static void InsertItemAfter<T_class>(List<T_class> tList,T_class t_posi,T_class item)
    {
        if (tList.Contains(t_posi))
        {
            tList.Insert(tList.IndexOf(t_posi)+1, item);
        }
        else
        {
            Debug.Log("没有找到t_posi");
        }
    }
}
 
public static class ReflectOperation
{
    //输入抽象父类定义的子类对象，创建新的抽象父类定义的子类对象（用于解除引用
    //参数的子类必须定义public Ta(Ta t)样式的构造函数
    public static Ta NewClassByReflect<Ta>(Ta ta)
    {
        Type type = ta.GetType();
        object[] parms = new object[] { ta };
        Ta obj = (Ta)Activator.CreateInstance(type, parms);
        return obj;
    }
}