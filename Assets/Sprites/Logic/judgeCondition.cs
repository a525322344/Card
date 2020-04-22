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

public class Judge_NullTrue : judgeCondition
{
    public Judge_NullTrue()
    {
        param = 0;
        whetherDele = (num, battle) => { return true; };
        describe = "你想的话";
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


public class Judge_BrunNumber : judgeCondition
{
    public Judge_BrunNumber(int num)
    { 
      param = 0;     
      whetherDele = AllAsset.judgeAsset.EnemyBurnNumber;
    }
}


public class Judge_buqiheng : judgeCondition
{
    public Judge_buqiheng(int num)
    {
        param = 0;
        whetherDele = AllAsset.judgeAsset.BuQiHeng;
    }
}



public class Judge_buqishu : judgeCondition
{
    public Judge_buqishu(int num)
    {
        param = 0;
        whetherDele = AllAsset.judgeAsset.BuQiShu;
    }
}