using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllAsset;
//读取数据，初始化数据类
[System.Serializable]
public class InitData
{
    Dictionary<int, csvcard> csvcard = new Dictionary<int, csvcard>();
    public List<playerCard> ShowAllCards;

    public void Awake()
    {
        CardInit();
        //EditorCardInit(gameManager.Instance.CardEditorBoard);
        MagicPartInit();
    }
    //数据加载全卡
    void CardInit()
    {
        ShowAllCards = cardAsset.AllIdCards;
        csvcard = CSVLoader.LoadCsvData<csvcard>(Application.streamingAssetsPath + "/cardcsv.csv");
        foreach (int i in csvcard.Keys)
        {
            cardAsset.AllIdCards.Add(new playerCard(csvcard[i].id, csvcard[i].name, CSVLoader.StringToEnum(csvcard[i].kind),csvcard[i].cost ,csvcard[i].damage, csvcard[i].deffence));
        }
        //给火球术添加灼烧效果
        cardAsset.AllIdCards[2].AddEffect(new Burn(3));
        //1费 抽2，打5
        playerCard card_jinGangQiangPo = new playerCard(3,"金刚枪破", CardKind.AttackCard, 1, 1);
        card_jinGangQiangPo.AddEffect(new Damage(5));
        card_jinGangQiangPo.AddEffect(new DrawCard(2));
        cardAsset.AllIdCards.Add(card_jinGangQiangPo);
        //2费 打8 护甲8
        playerCard attackAndDeffence = new playerCard(4, "攻击和防御", CardKind.SkillCard, 2,1);
        attackAndDeffence.AddEffect(new Damage(8));
        attackAndDeffence.AddEffect(new Armor(8));
        cardAsset.AllIdCards.Add(attackAndDeffence);
        //1费 灼烧4
        playerCard fire = new playerCard(5, "烈焰", CardKind.SkillCard, 1,1);
        fire.AddEffect(new Burn(4));
        cardAsset.AllIdCards.Add(fire);
        //1费 链接
        playerCard link = new playerCard(6, "不稳定连结", CardKind.SkillCard, 1,2);
        link.AddEffect(new LinkRandom());
        link.AddEffect(new CardEffect_ToExitLink());
        cardAsset.AllIdCards.Add(link);
        //1费 魔珠连环
        playerCard mozhulianhuan = new playerCard(7, "魔珠连环", CardKind.AttackCard, 1,2);
        mozhulianhuan.AddEffect(new Repeat(3,new Damage(3)));
        cardAsset.AllIdCards.Add(mozhulianhuan);
        //1费 降神 打7，如果敌人要攻击，则抽三张卡
        playerCard xiangshen = new playerCard(8, "降神", CardKind.AttackCard, 1,2);
        xiangshen.AddEffect(new Damage(7));
        cardEffectBase whethereffect = new CardEffect_Whether(new Judge_EnemyWillAttack(), new DrawCard(3));
        xiangshen.AddEffect(whethereffect);
        cardAsset.AllIdCards.Add(xiangshen);
        //1费 闪光爆裂 打8，抽1，弃3
        playerCard shanguangbaolie = new playerCard(9, "闪光爆裂", CardKind.AttackCard, 1,1);
        shanguangbaolie.AddEffect(new Damage(8));
        shanguangbaolie.AddEffect(new DrawCard(1));
        shanguangbaolie.AddEffect(new CardEffect_DisSomeCard(1));
        cardAsset.AllIdCards.Add(shanguangbaolie);
        //1费 飞弹 打6 
        playerCard feidan = new playerCard(10, "飞弹", CardKind.AttackCard, 1,0);
        feidan.AddEffect(new Damage(6));
        cardAsset.AllIdCards.Add(feidan);
        //1费 飞弹+ 打9
        playerCard feidanplus = new playerCard(11, "飞弹+", CardKind.AttackCard, 1,0);
        feidanplus.AddEffect(new Damage(9));
        cardAsset.AllIdCards.Add(feidanplus);
        //1费 护盾 5甲
        playerCard hudun = new playerCard(12, "护盾", CardKind.SkillCard, 1,0);
        hudun.AddEffect(new Armor(5));
        cardAsset.AllIdCards.Add(hudun);
        //1费 护盾+ 8甲
        playerCard hudunplus = new playerCard(13, "护盾+", CardKind.SkillCard, 1,0);
        hudunplus.AddEffect(new Armor(8));
        cardAsset.AllIdCards.Add(hudunplus);
        //2费 火球术 10点伤害，3层灼烧
        playerCard huoqiushu = new playerCard(14, "火球术", CardKind.AttackCard, 2,1);
        huoqiushu.AddEffect(new Damage(10));
        huoqiushu.AddEffect(new Burn(3));
        cardAsset.AllIdCards.Add(huoqiushu);
        //1费 点燃 2层灼烧，打1两次
        playerCard dianran = new playerCard(15, "点燃", CardKind.AttackCard, 1,1);
        dianran.AddEffect(new Burn(2));
        dianran.AddEffect(new Repeat(2, new Damage(1)));
        cardAsset.AllIdCards.Add(dianran);
        //2费 火焰屏障 12甲，8层灼烧
        playerCard huoyanpingzhang = new playerCard(16, "火焰屏障", CardKind.SkillCard, 2,2);
        huoyanpingzhang.AddEffect(new Armor(12));
        huoyanpingzhang.AddEffect(new Burn(8));
        cardAsset.AllIdCards.Add(huoyanpingzhang);
        //0费 火花 使敌人获得2点灼烧 
        playerCard huohua = new playerCard(17, "火花", CardKind.SkillCard, 0,1);
        huohua.AddEffect(new Burn(2));
        cardAsset.AllIdCards.Add(huohua);
        //2费 炎爆 使敌人的灼烧层数翻倍
        playerCard yanbao = new playerCard(18, "炎爆", CardKind.SkillCard, 2,2);
        yanbao.AddEffect(new DoubleBurn(2));
        cardAsset.AllIdCards.Add(yanbao);
        //0费 临时媒介 在本回合使两个部件处于连接状态
        playerCard linshimeijie = new playerCard(19, "临时媒介", CardKind.SkillCard, 0,2);
        linshimeijie.AddEffect(new LinkRandom());
        linshimeijie.AddEffect(new CardEffect_ToExitLink());
        cardAsset.AllIdCards.Add(linshimeijie);
        //1费 修整 获得8点格挡，抽两张牌
        playerCard xiuzheng = new playerCard(20, "修整", CardKind.SkillCard, 1,1);
        xiuzheng.AddEffect(new Armor(8));
        xiuzheng.AddEffect(new DrawCard(2));
        cardAsset.AllIdCards.Add(xiuzheng);
        //1费 风暴 造成3点伤害3次
        playerCard fengbao = new playerCard(21, "风暴", CardKind.AttackCard, 1,1);
        fengbao.AddEffect(new Repeat(3, new Damage(3)));
        cardAsset.AllIdCards.Add(fengbao);
        //0费 充能 抽一张牌，弃置一张牌，如果敌人灼烧层数大于等于5，额外抽两张牌
        playerCard chongneng = new playerCard(22, "充能", CardKind.SkillCard, 0,2);
        chongneng.AddEffect(new DrawCard(1));
        chongneng.AddEffect(new CardEffect_DisSomeCard(1));
        cardEffectBase whethereffect2 = new CardEffect_Whether(new Judge_BrunNumber(5), new DrawCard(2));
        cardAsset.AllIdCards.Add(chongneng);
        //1费 攻守兼备 每补齐一个横行，造成6点伤害；每补齐一个纵列，获得6点格挡
        playerCard gongshoujianbei = new playerCard(23, "攻守兼备", CardKind.SkillCard, 0, 1);
        judgeCondition judgeFillH = new Judge_buqiheng(0);
        judgeCondition judgeFillV = new Judge_buqishu(0);
        cardEffectBase whethereffect3 = new CardEffect_Whether(judgeFillH, new CardEffect_RepeatByFill(judgeFillH, new Damage(6)));
        cardEffectBase whethereffect4 = new CardEffect_Whether(judgeFillV, new CardEffect_RepeatByFill(judgeFillV, new Armor(6)));
        gongshoujianbei.AddEffect(whethereffect3);
        gongshoujianbei.AddEffect(whethereffect4);

        cardAsset.AllIdCards.Add(gongshoujianbei);
    }
    //“手动”加载全部件 可能是暂定
    void MagicPartInit()
    {
        int[] a = { 0, 1, 0, 0, 1, 0, 0, 0, 0 };
        Reaction reaction = new Reaction_Create(new EffectEvent(new Burn(1),null), EventKind.Event_PlayCard);
        MagicPart Init_BURNUP_1 = new MagicPart(a,0);
        Init_BURNUP_1.describe = "灼烧+1";
        Init_BURNUP_1.addReaction(reaction);

        AllAsset.magicpartAsset.AllMagicParts.Add(Init_BURNUP_1);

        a[1] = 1;
        reaction = new Reaction_Affect("敏捷增加",new extraDeffenceUp(2), EventKind.Event_Armor);
        MagicPart Init_DefenceUp_1 = new MagicPart(a,1);
        Init_DefenceUp_1.describe = "敏捷+2";
        Init_DefenceUp_1.addReaction(reaction);

        AllAsset.magicpartAsset.AllMagicParts.Add(Init_DefenceUp_1);
    }

    public void EditorCardInit(CardEditorBoard cardboard)
    {
        foreach(editorCard card in cardboard.AllCards)
        {
            playerCard newcard = new playerCard(card.id, card.name, card.Kind, card.cost, (int)card.Rank);
            foreach(editorEffect eE in card.playEffects)
            {
                newcard.AddEffect(EffectFromInit(eE));
                if (eE.effectKind == EnumEffect.LinkRandom)
                {
                    newcard.AddEffect(new CardEffect_ToExitLink());
                }
            }
            cardAsset.AllIdCards.Add(newcard);
        }
    }
    cardEffectBase EffectFromInit(editorEffect editorEffect)
    {
        cardEffectBase Effect = new emplyPlayCard();
        switch (editorEffect.effectKind)
        {
            case EnumEffect.Damage:
                Effect = new Damage(editorEffect.num);
                break;
            case EnumEffect.Armor:
                Effect = new Armor(editorEffect.num);
                break;
            case EnumEffect.DrawCard:
                Effect = new DrawCard(editorEffect.num);
                break;
            case EnumEffect.Repeat:
                Effect = new Repeat(editorEffect.num);
                foreach(editorEffect eE in editorEffect.childeffects)
                {
                    Effect.childeffects.Add(EffectFromInit(eE));
                }
                break;
            case EnumEffect.Burn:
                Effect = new Burn(editorEffect.num);
                break;
            case EnumEffect.LinkRandom:
                Effect = new LinkRandom(editorEffect.num);
                break;
            case EnumEffect.DoubleBurn:
                Effect = new DoubleBurn(editorEffect.num);
                break;
            case EnumEffect.Whether:
                Effect = new CardEffect_Whether();
                foreach(editorJudge ej in editorEffect.judges)
                {
                    Effect.judgeConditions.Add(JudgeFromInit(ej));
                }
                foreach (editorEffect eE in editorEffect.childeffects)
                {
                    Effect.childeffects.Add(EffectFromInit(eE));
                }
                break;
            case EnumEffect.DisCard:
                break;
            default:
                Debug.Log("没有该EditorEffect对应的Effect转换");
                break;
        }
        return Effect;
    }

    judgeCondition JudgeFromInit(editorJudge ejudge)
    {
        judgeCondition Judge = new Judge_NullTrue();
        switch (ejudge.judgeKind)
        {
            case EnumJudge.敌人意图攻击:
                Judge = new Judge_EnemyWillAttack();
                break;
        }
        return Judge;
    }
    public static List<perform> PerformListFromInit(editorCard card)
    {
        List<perform> performs = new List<perform>();
        foreach(editorPerform eP in card.performlist)
        {
            perform newperform;
            if (eP.performkind == PerformKind.动画)
            {
                newperform = new PerformAnima((int)eP.charactor,eP.animation,eP.animationSpeed);
                newperform.timeTurn = eP.time;
                performs.Add(newperform);
            }
            else if (eP.performkind == PerformKind.特效)
            {
                newperform = new PerformEffect((int)eP.effectWay, eP.effectGO, eP.movespeed,eP.effecttime);
                newperform.timeTurn = eP.time;
                performs.Add(newperform);
            }
        }
        return performs;
    }
}
