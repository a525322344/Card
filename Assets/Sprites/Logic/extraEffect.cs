using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//对效果的强化函数
public delegate int DeleExtraEffect(int cardnum,int extraadjust);

public enum AffectEffectKind
{
    Affect_DamageUp,
    Affect_ArmorUp,
    Affect_BurnUp

}
public abstract class extraEffectBase
{
    public string name;
    public abstract int AdjustEffect(int cardnum);
    public abstract bool canInfluence(cardEffectBase _cardEffectBase);
    public EffectBase getInfluenceEffect()
    {
        return CanInffenceEffect;
    }

    protected int priority;
    protected int adjustnum;
    protected EffectBase CanInffenceEffect;
    public virtual string Describe()
    {
        return "";
    }
    protected DeleExtraEffect deleAdjust;

    protected string ColorGold = "<color=#CFB53B>";
    protected string ColorBlue = "<color=#007FFF>";
    protected string ColorGreen = "<color=#32CD32>";
    protected string ColorEnd = "</color>";
}

public class extraAttackUp : extraEffectBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="adjust">可以调整的数值</param>
    public extraAttackUp(int adjust)
    {
        priority = 5;
        adjustnum = adjust;
        CanInffenceEffect = new Damage();
        deleAdjust = new DeleExtraEffect(AllAsset.extraAsset.addSubNum);
    }
    public override int AdjustEffect(int _cardnum)
    {
        return deleAdjust(_cardnum, adjustnum);
    }
    public override bool canInfluence(cardEffectBase _cardEffectBase)
    {
        return _cardEffectBase.GetType() == CanInffenceEffect.GetType();
    }
    public override string Describe()
    {
        return "卡牌造成的伤害增加" + ColorBlue + adjustnum + ColorEnd;
    }
}
public class extraDeffenceUp : extraEffectBase
{
    public extraDeffenceUp(int adjust)
    {
        priority = 5;
        adjustnum = adjust;
        CanInffenceEffect = new Armor();
        deleAdjust = new DeleExtraEffect(AllAsset.extraAsset.addSubNum);
    }
    public override int AdjustEffect(int _cardnum)
    {
        return deleAdjust(_cardnum, adjustnum);
    }
    public override bool canInfluence(cardEffectBase _cardEffectBase)
    {
        return _cardEffectBase.GetType() == CanInffenceEffect.GetType();
    }
    public override string Describe()
    {
        return "从卡牌获得的护甲增加" + ColorBlue + adjustnum + ColorEnd;
    }
}
public class extraBurnUp : extraEffectBase
{
    public extraBurnUp(int adjust)
    {
        priority = 5;
        adjustnum = adjust;
        CanInffenceEffect = new Burn();
        deleAdjust = new DeleExtraEffect(AllAsset.extraAsset.addSubNum);
    }
    public override int AdjustEffect(int _cardnum)
    {
        return deleAdjust(_cardnum, adjustnum);
    }
    public override bool canInfluence(cardEffectBase _cardEffectBase)
    {
        return _cardEffectBase.GetType() == CanInffenceEffect.GetType();
    }
    public override string Describe()
    {
        return "给与的灼烧层数增加" + ColorBlue + adjustnum + ColorEnd;
    }
}

public class extraMonsterAttackUp : extraEffectBase
{
    stateAbstarct PowerUpState;
    public extraMonsterAttackUp(stateAbstarct state)
    {
        PowerUpState = state;
        priority = 5;
        adjustnum = state.num;
        CanInffenceEffect = new ActionEffect_MonsterHurt(0);
        deleAdjust = new DeleExtraEffect(AllAsset.extraAsset.addSubNum);
    }
    public override int AdjustEffect(int _cardnum)
    {
        Debug.Log(PowerUpState.name + PowerUpState.num);
        return deleAdjust(_cardnum, PowerUpState.num);
    }
    public override bool canInfluence(cardEffectBase _cardEffectBase)
    {
        return _cardEffectBase.GetType() == CanInffenceEffect.GetType();
    }
    public override string Describe()
    {
        return "怪物造成的伤害增加" + ColorBlue + adjustnum + ColorEnd;
    }
}

public class extraRoundDrawUp : extraEffectBase
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="adjust">可以调整的数值</param>
    public extraRoundDrawUp(int adjust)
    {
        priority = 5;
        adjustnum = adjust;
        CanInffenceEffect = new RoundStartDrawCard(0);
        deleAdjust = new DeleExtraEffect(AllAsset.extraAsset.addSubNum);
    }
    public override int AdjustEffect(int _cardnum)
    {
        return deleAdjust(_cardnum, adjustnum);
    }
    public override bool canInfluence(cardEffectBase _cardEffectBase)
    {
        return _cardEffectBase.GetType() == CanInffenceEffect.GetType();
    }
    public override string Describe()
    {
        return "每回合抽牌数减少" + ColorBlue + adjustnum + ColorEnd;
    }
}