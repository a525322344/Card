using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#region 编辑类演示
public enum PerformKind
{
    添加演出,
    动画,
    特效,
};
public enum Charactor
{
    主角,
    怪物,
}
public enum EffectWay
{
    主角位置,
    怪物位置,
    从主角到怪物,
    从怪物到主角,
    主角武器位置,
}
[System.Serializable]
public class editorPerform:IComparable
{
    public editorPerform(PerformKind kind)
    {
        performkind = kind;
    }
    public PerformKind performkind;
    public float time;
    //动画演示参数
    public Charactor charactor;       //0主角 1怪物
    public int animation;       //动画序号
    public float animationSpeed;  //动画倍速
    //效果演示参数
    public EffectWay effectWay;       //路径方式 0玩家位置，1敌人位置，2玩家到敌人，3敌人到玩家
    public GameObject effectGO;
    public float effecttime;
    public float movespeed;       //仅对2，3有效

    public int CompareTo(object obj)
    {
        return this.time.CompareTo((obj as editorPerform).time);
    }
}
#endregion
#region 编辑类判断
public enum EnumJudge
{
    默认,
    敌人意图攻击,
    每补齐一横行,
    每补齐一纵行,
    每补齐一横行或纵行,
}
[System.Serializable]
public class editorJudge
{
    public editorJudge(EnumJudge judge)
    {
        judgeKind = judge;
    }
    public int num;
    public bool b_UseNum;
    public EnumJudge judgeKind;
}
#endregion
#region 编辑类效果
public enum EnumEffect
{
    Default,
    Damage,
    DamageByJudge,
    Armor,
    Repeat,
    DrawCard,
    Burn,
    DoubleBurn,
    LinkRandom,
    Whether,
    DisCard,
    RepeatByFill,
    RepeatByEffect,//此效果前面必须有效果，会使用前者的效果返回值
    DisAllCard,
}
[System.Serializable]
public class editorEffect
{
    public editorEffect(EnumEffect kind)
    {
        effectKind = kind;
        name = kind.ToString();
    }
    public string name;
    public int num;
    public EnumEffect effectKind;
    public bool b_haveJudge;
    public List<editorJudge> judges = new List<editorJudge>();
    public bool b_haveChildEffect;
    public List<editorEffect> childeffects = new List<editorEffect>();
}
#endregion
#region 编辑类卡牌
public enum Rank
{
    基本,
    普通,
    稀有,
    罕见,
    衍生,
}
[System.Serializable]
public class editorCard
{
    public editorCard() {
        name = "默认名称";
    }
    public string name;
    public int id;

    public int cost;
    public Rank Rank;
    public CardKind Kind;
    public int rank;
    public int textureId;
    public List<editorEffect> playEffects = new List<editorEffect>();
}
[System.Serializable]
public class editorCardCollect
{
    public editorCardCollect()
    {
        Card = new editorCard();
        gradeCard = new editorCard();
        showGrade = false;
    }
    public bool showGrade;
    public editorCard Card;
    public editorCard gradeCard;

    public float alltime;
    public List<editorPerform> performlist = new List<editorPerform>();
}
#endregion
[CreateAssetMenu(menuName = "CardEditorBoard")]
public class CardEditorBoard : ScriptableObject
{
    //public List<editorCard> AllCards = new List<editorCard>();

    public List<editorCardCollect> allCards = new List<editorCardCollect>();
    public void AddCard()
    {
        allCards.Add(new editorCardCollect());
    } 
    public void AutoIdOrder()
    {
        for(int i = 0; i < allCards.Count; i++)
        {
            allCards[i].Card.id = i;
            allCards[i].gradeCard.id = i;
        }
    }

    public static editorEffect EffectFromEnum(EnumEffect enumEffect)
    {
        editorEffect effect = new editorEffect(enumEffect);
        switch (enumEffect)
        {
            case EnumEffect.Damage:
                break;
            case EnumEffect.Armor:
                break;
            case EnumEffect.DrawCard:
                break;
            case EnumEffect.Repeat:
                effect.b_haveChildEffect = true;
                break;
            case EnumEffect.Burn:
                break;
            case EnumEffect.LinkRandom:
                break;
            case EnumEffect.DoubleBurn:
                break;
            case EnumEffect.Whether:
                effect.b_haveJudge = true;
                break;
            case EnumEffect.DisCard:
                break;
            case EnumEffect.RepeatByFill:
                effect.b_haveChildEffect = true;
                break;
            case EnumEffect.DisAllCard:          
                break;
            case EnumEffect.DamageByJudge:
                effect.b_haveJudge = true;
                break;
            case EnumEffect.RepeatByEffect:
                effect.b_haveChildEffect = true;
                break;
        }
        return effect;
    }
    public static editorJudge JudgeFormEnum(EnumJudge judge)
    {
        editorJudge Ejudge = new editorJudge(judge);
        switch (judge)
        {
            case EnumJudge.敌人意图攻击:
                break;
        }
        return Ejudge;
    }
}
