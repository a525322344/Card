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
        //初始化
        for (int i = 0; i < 4; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].gameObject.SetActive(false);
        }
        //设置数据
        if (befall.textureOrder == -1)
        {
            befallImage.gameObject.SetActive(false);
        }
        else
        {
            befallImage.gameObject.SetActive(true);
            befallImage.sprite = gameManager.Instance.instantiatemanager.beFallSprites[befall.textureOrder];
        }
        if (befall.describe == null)
        {
            befallDescribe.gameObject.SetActive(false);
        }
        else
        {
            befallDescribe.gameObject.SetActive(true);
            befallDescribe.text = befall.describe;
        }
        befallName.text = befall.name;

        //设置按钮
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
    //关闭窗口但是没有结束事件，目前同上
    public void CloseBoardButContinue()
    {
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
