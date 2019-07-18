using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public delegate void CardEffect(int num);
[System.Serializable]
public class Effectclass
{
    public CardEffect cardEffect;
    public int num;

    public Effectclass(CardEffect _cardEffect, int _num)
    {
        cardEffect = _cardEffect;
        num = _num;
    }
}
[System.Serializable]
public class playerCard : card
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="id">序号</param>
    /// <param name="name">卡名</param>
    /// <param name="kind">种类（enum CardKind）</param>
    /// <param name="_damageToEnemy">伤害</param>
    /// <param name="_deffenceToOwn">护甲</param>
    public playerCard(int id, string name, CardKind kind,int _damageToEnemy,int _deffenceToOwn)
    {
        Id = id;
        Name = name;
        Kind = kind;
        switch (kind)
        {
            case CardKind.CurseCard:
            case CardKind.StateCard:
                SetCanPlay(false);
                break;
            case CardKind.PlayerCard:
                SetCanPlay(true);
                break;
        }
        damageToEnemy = _damageToEnemy;
        deffenceToOwn = _deffenceToOwn;
        setEffect();
    }
    #region 卡牌效果字段
    //伤害
    public int damageToEnemy;
    public void SetDamageToEnemy(int num)
    {
        damageToEnemy = num;
    }
    //护甲
    public int deffenceToOwn;
    public void SetDeffence_own(int num)
    {
        deffenceToOwn = num;
    }
    #endregion
    //效果链表
    public List<Effectclass> Effectclass_Enemy=new List<Effectclass>();
    public List<Effectclass> Effectclass_Own=new List<Effectclass>();
    private void setEffect()
    {
        if (damageToEnemy > 0)
        {            
            Effectclass_Enemy.Add(new Effectclass(new CardEffect(AllAsset.effectAsset.giveDemage),damageToEnemy));
            Describe += "造成" + damageToEnemy + "点伤害;";
        }
        if (deffenceToOwn > 0)
        {
            Effectclass_Own.Add(new Effectclass(new CardEffect(AllAsset.effectAsset.getArmor),deffenceToOwn));
            Describe += "获得" + deffenceToOwn + "点护甲;";
        }
        
    }

    //效果
    public void cardPlayEffect()
    {
        int listLength = Effectclass_Own.Count;
        for(int i = 0; i < listLength; i++)
        {
            Effectclass_Own[i].cardEffect(Effectclass_Own[i].num);
        }
    }
}

