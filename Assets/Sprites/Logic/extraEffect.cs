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
    public abstract int AdjustEffect(int cardnum);
    public abstract bool canInfluence(cardEffectBase _cardEffectBase);
    public cardEffectBase getInfluenceEffect()
    {
        return CanInffenceEffect;
    }

    protected int priority;
    protected int adjustnum;
    protected cardEffectBase CanInffenceEffect;
    protected DeleExtraEffect deleAdjust;
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

}
