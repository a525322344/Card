using System;
using System.Collections.Generic;
using System.Collections;

public delegate bool DeleWhether(int num, battleInfo battleInfo,out int returnnum);

public class judgeCondition
{
    //条件
    public int param;
    public int returnNum;
    public DeleWhether whetherDele;
    public string describe;
    public bool Whether(battleInfo battleinfo)
    {
        return whetherDele(param, battleinfo,out returnNum);
    }
}

public class Judge_NullTrue : judgeCondition
{
    public Judge_NullTrue()
    {
        param = 0;
        whetherDele = (int num,battleInfo battle,out int a) => 
        {
            a = 0;
            return true;
        };
        describe = "你想的话";
    }
}
public class Judge_EnemyWillAttack : judgeCondition
{
    public Judge_EnemyWillAttack()
    {
        param = 0;
        whetherDele = AllAsset.judgeAsset.EnemyWillAttack;
        describe = "如果敌人意图攻击";
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
        param = num;     
        whetherDele = AllAsset.judgeAsset.EnemyBurnNumber;
        describe = "如果敌人灼烧有"+param+"层以上";
    }
}


public class Judge_buqiheng : judgeCondition
{
    public Judge_buqiheng(int num)
    {
        param = 0;
        whetherDele = AllAsset.judgeAsset.BuQiHeng;
        describe = "每补齐一横行";
    }
}



public class Judge_buqishu : judgeCondition
{
    public Judge_buqishu(int num)
    {
        param = 0;
        whetherDele = AllAsset.judgeAsset.BuQiShu;
        describe = "每补齐一纵行";
    }
}

public class Judge_BuQi : judgeCondition
{
    public Judge_BuQi()
    {
        param = 0;
        whetherDele = AllAsset.judgeAsset.BuQi;
        describe = "每补齐一横行或纵行";
    }
}