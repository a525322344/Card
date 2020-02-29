using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllAsset;
//读取数据，初始化数据类
public class InitData : MonoBehaviour
{
    Dictionary<int, csvcard> csvcard = new Dictionary<int, csvcard>();
    public List<playerCard> ShowAllCards;

    public void Awake()
    {
        CsvCardInit();
        MagicPartInit();
    }
    //数据加载全卡
    void CsvCardInit()
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
        playerCard card_jinGangQiangPo = new playerCard(3,"金刚枪破", CardKind.PlayerCard, 1, 5, 0);
        card_jinGangQiangPo.AddEffect(new DrawCard(2));
        cardAsset.AllIdCards.Add(card_jinGangQiangPo);
        //2费 打8 护甲8
        playerCard attackAndDeffence = new playerCard(4, "攻击和防御", CardKind.PlayerCard, 2);
        attackAndDeffence.AddEffect(new Damage(8));
        attackAndDeffence.AddEffect(new Armor(8));
        cardAsset.AllIdCards.Add(attackAndDeffence);
        //1费 灼烧4
        playerCard fire = new playerCard(5, "烈焰", CardKind.PlayerCard, 1);
        fire.AddEffect(new Burn(4));
        cardAsset.AllIdCards.Add(fire);
        //1费 链接
        playerCard link = new playerCard(6, "不稳定连结", CardKind.PlayerCard, 1);
        link.AddEffect(new LinkRandom());
        link.AddEffect(new CardEffect_ToExitLink());
        cardAsset.AllIdCards.Add(link);
        //1费 魔珠连环
        playerCard mozhulianhuan = new playerCard(7, "魔珠连环", CardKind.PlayerCard, 1);
        mozhulianhuan.AddEffect(new Repeat(3,new Damage(3)));
        cardAsset.AllIdCards.Add(mozhulianhuan);
        //1费 降神 打7，如果敌人要攻击，则抽三张卡
        playerCard xiangshen = new playerCard(8, "降神", CardKind.PlayerCard, 1);
        xiangshen.AddEffect(new Damage(7));
        cardEffectBase whethereffect = new CardEffect_Whether(new Judge_EnemyWillAttack(), new DrawCard(3));
        xiangshen.AddEffect(whethereffect);
        cardAsset.AllIdCards.Add(xiangshen);
        //1费 闪光爆裂 打8，抽1，弃1
        playerCard shanguangbaolie = new playerCard(9, "闪光爆裂", CardKind.PlayerCard, 1);
        shanguangbaolie.AddEffect(new Damage(8));
        shanguangbaolie.AddEffect(new DrawCard(1));
        shanguangbaolie.AddEffect(new CardEffect_DisSomeCard(3));
        cardAsset.AllIdCards.Add(shanguangbaolie);
        
    }
    //“手动”加载全部件 可能是暂定
    void MagicPartInit()
    {
        int[] a = { 0, 1, 0, 0, 1, 0, 0, 0, 0 };
        Reaction reaction = new Reaction_Affect("伤害增加",new extraAttackUp(2), EventKind.Event_Damage);
        MagicPart Init_ATTACKUP_1 = new MagicPart(a,0);
        Init_ATTACKUP_1.describe = "伤害+2";
        Init_ATTACKUP_1.addReaction(reaction);

        AllAsset.magicpartAsset.AllMagicParts.Add(Init_ATTACKUP_1);

        a[4] = 0;
        reaction = new Reaction_Affect("护甲增加",new extraDeffenceUp(1), EventKind.Event_Armor);
        MagicPart Init_DefenceUp_1 = new MagicPart(a,1);
        Init_DefenceUp_1.describe = "护甲+1";
        Init_DefenceUp_1.addReaction(reaction);

        AllAsset.magicpartAsset.AllMagicParts.Add(Init_DefenceUp_1);
    }


}
