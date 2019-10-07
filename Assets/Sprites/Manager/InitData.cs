using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllAsset;
//读取数据，初始化数据类
public class InitData : MonoBehaviour
{
    Dictionary<int, csvcard> csvcard = new Dictionary<int, csvcard>();

    public void Awake()
    {
        CsvCardInit();
        MagicPartInit();
    }
    //数据加载全卡
    void CsvCardInit()
    {
        csvcard = CSVLoader.LoadCsvData<csvcard>(Application.streamingAssetsPath + "/cardcsv.csv");
        foreach (int i in csvcard.Keys)
        {
            cardAsset.AllIdCards.Add(new playerCard(csvcard[i].id, csvcard[i].name, CSVLoader.StringToEnum(csvcard[i].kind),csvcard[i].cost ,csvcard[i].damage, csvcard[i].deffence));
        }

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
    }
    //“手动”加载全部件 可能是暂定
    void MagicPartInit()
    {
        int[] a = { 0, 1, 0, 0, 1, 0, 0, 0, 0 };
        Reaction reaction = new Reaction_Affect(new extraAttackUp(1), EventKind.Event_Damage);
        MagicPart Init_ATTACKUP_1 = new MagicPart(a);
        Init_ATTACKUP_1.addReaction(reaction);

        AllAsset.magicpartAsset.AllMagicParts.Add(Init_ATTACKUP_1);

        a[4] = 0;
        reaction = new Reaction_Affect(new extraDeffenceUp(1), EventKind.Event_Armor);
        MagicPart Init_DefenceUp_1 = new MagicPart(a);
        Init_DefenceUp_1.addReaction(reaction);

        AllAsset.magicpartAsset.AllMagicParts.Add(Init_DefenceUp_1);
    }


}
