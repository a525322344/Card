using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CardKind
{
    PlayerCard,
    AttackCard,
    SkillCard,
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
    public int Rank;        //基本0，普通1，稀有2，罕见3，
    public int TextureId;

    public Dictionary<Vector2, int> vecCostPairs = new Dictionary<Vector2, int>();

    public string Describe;
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
                break;
            case CardKind.PlayerCard:
                break;
        }

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                vecCostPairs.Add(new Vector2(i, j), 0);
            }
        }
        switch (Cost)
        {
            case 0:
                break;
            case 1:
                vecCostPairs[new Vector2(0,0)] = 1;
                break;
            case 2:
                vecCostPairs[new Vector2(0, 0)] = 1;
                vecCostPairs[new Vector2(0, -1)] = 1;
                break;
                //other
        }
        damageToEnemy = _damageToEnemy;
        deffenceToOwn = _deffenceToOwn;
        setEffect();
        CardDescribe();
    }
    public playerCard(int id, string name, CardKind kind, int cost,int rank)
    {
        TextureId = 0;
        Id = id;
        Name = name;
        Kind = kind;
        Cost = cost;
        Rank = rank;
        switch (kind)
        {
            case CardKind.CurseCard:
            case CardKind.StateCard:
                break;
            case CardKind.PlayerCard:
                break;
        }
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                vecCostPairs.Add(new Vector2(i, j), 0);
            }
        }
        switch (Cost)
        {
            case 0:
                break;
            case 1:
                vecCostPairs[new Vector2(0, 0)] = 1;
                break;
            case 2:
                vecCostPairs[new Vector2(0, 0)] = 1;
                vecCostPairs[new Vector2(0, -1)] = 1;
                break;
                //other
        }
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
            if (!effect.b_hideDesctibe)
            {
                Describe += effect.DescribeEffect() + ",";
            }
        }
        if (Describe.Length != 0)
        {
            Describe = Describe.Substring(0, Describe.Length - 1);
        }
        return Describe;
    }

    //    逻辑操作 手动添加效果
    public void AddEffect(cardEffectBase effect)
    {
        EffectPlayList.Add(effect);
        CardDescribe();
    }
}

