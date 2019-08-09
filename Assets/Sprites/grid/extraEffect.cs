using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//对效果的强化函数
public delegate int DeleExtraEffect(int cardnum,int extraadjust);
public abstract class extraEffectBase
{
    public abstract int AdjustEffect(int cardnum);
    public abstract bool canInfluence(cardEffectBase _cardEffectBase);

    protected int priority;
    protected int adjustnum;
    protected List<cardEffectBase> CanInffenceEffects = new List<cardEffectBase>();
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
        CanInffenceEffects.Add(new Damage(0));
        deleAdjust = new DeleExtraEffect(AllAsset.extraAsset.addSubNum);
    }
    public override int AdjustEffect(int _cardnum)
    {
        return deleAdjust(_cardnum, adjustnum);
    }
    public override bool canInfluence(cardEffectBase _cardEffectBase)
    {
        bool can = false;
        foreach(cardEffectBase cardEffect in CanInffenceEffects)
        {
            if (cardEffect.GetType() == _cardEffectBase.GetType())
            {
                can = true;
                break;
            }
        }
        return can;
    }

}
public class extraDeffenceUp : extraEffectBase
{
    public extraDeffenceUp(int adjust)
    {
        priority = 5;
        adjustnum = adjust;
        CanInffenceEffects.Add(new Deffence(0));
        deleAdjust = new DeleExtraEffect(AllAsset.extraAsset.addSubNum);
    }
    public override int AdjustEffect(int _cardnum)
    {
        return deleAdjust(_cardnum, adjustnum);
    }
    public override bool canInfluence(cardEffectBase _cardEffectBase)
    {
        bool can = false;
        foreach (cardEffectBase cardEffect in CanInffenceEffects)
        {
            if (cardEffect.GetType() == _cardEffectBase.GetType())
            {
                can = true;
                break;
            }
        }
        return can;
    }
    
}
