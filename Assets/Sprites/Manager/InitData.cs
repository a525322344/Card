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
