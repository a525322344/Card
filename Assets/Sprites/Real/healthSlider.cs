﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthSlider : MonoBehaviour
{
    public Image sliderImage;
    public Text healthText;
    public bool isbattle=true;
    public Image armorIcon;
    public Text armorText;
    public Transform statePosi;
    public float statedistance;
    public float stateHNum;
    private float maxhealth;
    private float nowhealth;
    private float armor;

    public SpriteRenderer HeadRenderer;

    public void Init(pawnbase pawn)
    {
        maxhealth = pawn.healthmax;
        nowhealth = pawn.healthnow;
        armor = pawn.armor;
        SetSlider(armor, nowhealth);
    }
    public void Init(playerInfo playerInfo)
    {
        maxhealth = playerInfo.playerHealthMax;
        nowhealth = playerInfo.playerHealth;
        SetSlider(armor, nowhealth);
    }
    public void SetMonsterHead(int i)
    {
        HeadRenderer.sprite = instantiateManager.instance.monsterHeadList[i];
    }

    public void SetSlider(float finalarmor,float finalheath)
    {
        armor = finalarmor;
        nowhealth = finalheath;
        if (isbattle)
        {
            if (armor > 0)
            {
                armorText.text = "" + armor;
                armorIcon.gameObject.SetActive(true);
            }
            else
            {
                armorIcon.gameObject.SetActive(false);
            }
        }

        healthText.text = "" + nowhealth + "/" + maxhealth;
        sliderImage.fillAmount = nowhealth / maxhealth;
    }
    public void SetSlider(float finalarmor, float finalheath,float _maxhealth)
    {
        armor = finalarmor;
        nowhealth = finalheath;
        maxhealth = _maxhealth;
        if (isbattle)
        {
            if (armor > 0)
            {
                armorText.text = "" + armor;
                armorIcon.gameObject.SetActive(true);
            }
            else
            {
                armorIcon.gameObject.SetActive(false);
            }
        }
        healthText.text = "" + nowhealth + "/" + maxhealth;
        sliderImage.fillAmount = nowhealth / maxhealth;
    }
}
