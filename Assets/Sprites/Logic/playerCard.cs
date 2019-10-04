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
public class card
{
    public string Name;
    public int Id;
    public CardKind Kind;
    public int Cost;
    protected int[,] priCostVector2;
    public int[,] costVector2
    {
        get
        {
            return priCostVector2;
        }
    }
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
    public playerCard(int id, string name, CardKind kind,int cost,int _damageToEnemy,int _deffenceToOwn)
    {
        Id = id;
        Name = name;
        Kind = kind;
        Cost = cost;
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
        priCostVector2 = new int[3,3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                priCostVector2[i, j] = 0;
            }
        }
        switch (Cost)
        {
            case 0:
                break;
            case 1:
                priCostVector2[1, 1] = 1;
                break;
            case 2:
                priCostVector2[1, 1] = 1;
                priCostVector2[2, 1] = 1;
                break;
                //other
        }
        damageToEnemy = _damageToEnemy;
        deffenceToOwn = _deffenceToOwn;
        setEffect();
        CardDescribe();
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
        }
        if (deffenceToOwn > 0)
        {
            cardEffectBase effect = new Armor(deffenceToOwn);
            EffectPlayList.Add(effect);
        }       
    }
    public string CardDescribe()
    {
        Describe = "";
        foreach (cardEffectBase effect in EffectPlayList)
        {
            Describe += effect.DescribeEffect() + ",";
        }
        Describe = Describe.Substring(0, Describe.Length - 1);
        return Describe;
    }

    //    逻辑操作 手动添加效果
    public void AddEffect(cardEffectBase effect)
    {
        EffectPlayList.Add(effect);
        CardDescribe();
    }
}

