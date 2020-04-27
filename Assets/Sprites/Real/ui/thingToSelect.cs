using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class thingToSelect<T_toselect> {
    //定义
    public delegate void UseCard(T_toselect card);
    public delegate void OnInit();

    List<T_toselect> thingList = new List<T_toselect>();

    public UseCard onSelectcard;
    public OnInit onInit = () => { };

    public void Init()
    {
        onInit();
    }

    public void selecThisCard(T_toselect card)
    {
        onSelectcard(card);
    }
}
