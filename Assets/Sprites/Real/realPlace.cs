using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class realPlace : MonoBehaviour
{
    public place thisplace;
    private Button button;
    public void Init(place _place)
    {
        thisplace = _place;
        button = GetComponent<Button>();
        button.onClick.AddListener(_place.onclick);
        button.image.sprite = gameManager.Instance.instantiatemanager.mapPlaceSprites[_place.imageorder];
    }
}
