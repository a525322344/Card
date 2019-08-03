using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllAsset;

public class InitAsset : MonoBehaviour
{
    Dictionary<int, csvcard> csvcard = new Dictionary<int, csvcard>();

    private void Awake()
    {
        CsvInit();
        PlayerDickInit();
    }

    void CsvInit()
    {
        csvcard = CSVLoader.LoadCsvData<csvcard>(Application.streamingAssetsPath + "/cardcsv.csv");
        foreach (int i in csvcard.Keys)
        {
            cardAsset.AllIdCards.Add(new playerCard(csvcard[i].id, csvcard[i].name, CSVLoader.StringToEnum(csvcard[i].kind), csvcard[i].damage, csvcard[i].deffence));
        }
        gameManager.Instance.playerAsset.AllIdCards = cardAsset.AllIdCards;
    }
    void PlayerDickInit()
    {
        gameManager.Instance.playerAsset.playerDick.Add(cardAsset.AllIdCards[0]);
        gameManager.Instance.playerAsset.playerDick.Add(cardAsset.AllIdCards[0]);
        gameManager.Instance.playerAsset.playerDick.Add(cardAsset.AllIdCards[1]);
        gameManager.Instance.playerAsset.playerDick.Add(cardAsset.AllIdCards[1]);
        gameManager.Instance.playerAsset.playerDick.Add(cardAsset.AllIdCards[1]);
        gameManager.Instance.playerAsset.playerDick.Add(cardAsset.AllIdCards[2]);
    }
}
