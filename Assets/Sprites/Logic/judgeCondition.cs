using System;
using System.Collections.Generic;

public delegate bool DeleWhether(int num, battleInfo battleInfo);

public class judgeCondition
{
    //条件
    public int param;
    public DeleWhether whetherDele;
    public string describe;
    public bool Whether(int num,battleInfo battleinfo)
    {
        return whetherDele(num, battleinfo);
    }
}

public class Judge_EnemyWillAttack : judgeCondition
{
    public Judge_EnemyWillAttack(int _param)
    {
        param = _param;
        whetherDele = AllAsset.judgeAsset.EnemyWillAttack;
        describe = "如果敌人有攻击意图";
    }
}