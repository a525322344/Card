using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiEventBoard : MonoBehaviour
{
    #region 默认不更改
    public Image blackImage;        //黑底
    public Image headImage;         //标题底图

    public Button buttonGO;
    #endregion
    public Image befallImage;       //遭遇卡图
    public Text befallName;         //标题
    public Text befallDescribe;     //描述

    public List<Button> buttons;

    public void EnterEventBoard(befallinfo befall)
    {
        befallImage.sprite = gameManager.Instance.instantiatemanager.beFallSprites[befall.textureOrder];
        befallName.text = befall.name;
        befallDescribe.text = befall.describe;

        for(int i = 0; i < befall.buttons.Count; i++)
        {
            buttons[i].onClick.AddListener(befall.buttons[i].onclick);
            buttons[i].GetComponentInChildren<Text>().text = befall.buttons[i].buttonDescribe;
            buttons[i].gameObject.SetActive(true);
        }

        gameObject.SetActive(true);
    }
    //退出菜单
    public void ExitEventBoard()
    {
        foreach(Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }


}
