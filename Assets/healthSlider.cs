using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthSlider : MonoBehaviour
{
    public Image sliderImage;
    public Text healthText;
    public Image armorIcon;
    public Text armorText;
    private float maxhealth;
    private float nowhealth;
    private float armor;

    public void Init(pawnbase pawn)
    {
        maxhealth = pawn.healthmax;
        nowhealth = pawn.healthnow;
        armor = pawn.armor;
        SetSlider(armor, nowhealth);
    }
    

    public void SetSlider(float finalarmor,float finalheath)
    {
        armor = finalarmor;
        nowhealth = finalheath;
        if (armor > 0)
        {
            armorText.text = "" + armor;
            armorIcon.gameObject.SetActive(true);
        }
        else
        {
            armorIcon.gameObject.SetActive(false);
        }
        healthText.text = "" + nowhealth + "/" + maxhealth;
        sliderImage.fillAmount = nowhealth / maxhealth;
    }
}
