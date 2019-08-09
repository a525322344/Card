using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CardKind
{
    PlayerCard,
    StateCard,
    CurseCard
}
[System.Serializable]
public class card : object
{
    public string Name;
    public int Id;
    public CardKind Kind;
    public int cost;
    public string Describe;
    private bool canplay;
    public void SetCanPlay(bool b)
    {
        canplay = b;
    }
    public bool GetCanPlay()
    {
        return canplay;
    }
}

//游戏卡牌数据类，负责：记录原始信息，字段数据，生成并保存效果类链表；不包含打出函数
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
    public List<cardEffectBase> getEffectList()
    {
        return EffectPlayList;
    }
    #region 卡牌效果字段
    //伤害
    public int damageToEnemy;
    //护甲
    public int deffenceToOwn;
    #endregion
    //打出效果链表
    public List<cardEffectBase> EffectPlayList = new List<cardEffectBase>();

    private void setEffect()
    {
        if (damageToEnemy > 0)
        {
            cardEffectBase effect = new Damage(damageToEnemy);
            EffectPlayList.Add(effect);

            Describe += effect.DescribeEffect(damageToEnemy);
        }
        if (deffenceToOwn > 0)
        {
            cardEffectBase effect = new Deffence(deffenceToOwn);
            EffectPlayList.Add(effect);
            Describe += effect.DescribeEffect(deffenceToOwn);
        }       
    }    
}

