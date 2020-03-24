using System;
using System.Collections.Generic;

public delegate bool DeleWhether(int num, battleInfo battleInfo);

public class judgeCondition
{
    //条件
    public int param;
    public DeleWhether whetherDele;
    public string describe;
    public bool Whether(battleInfo battleinfo)
    {
        return whetherDele(param, battleinfo);
    }
}

public class Judge_EnemyWillAttack : judgeCondition
{
    public Judge_EnemyWillAttack()
    {
        param = 0;
        whetherDele = AllAsset.judgeAsset.EnemyWillAttack;
        describe = "敌人意图攻击";
    }
}

public class Judge_HaveSelectedHandCard : judgeCondition
{
    public Judge_HaveSelectedHandCard()
    {
        param = 0;
        whetherDele = AllAsset.judgeAsset.HaveSelectHandCard;
    }
}

