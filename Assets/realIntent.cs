using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class realIntent : MonoBehaviour
{
    public GameObject attackin;
    public GameObject defencein;
    public GameObject powerupin;
    public GameObject debuffin;
    public TextMeshPro num;
    // Start is called before the first frame update
    public void Init(actionAbstract action)
    {
        attackin.SetActive(false);
        defencein.SetActive(false);
        powerupin.SetActive(false);
        debuffin.SetActive(false);
        num.gameObject.SetActive(false);
        if (action.Kind == ACTIONKIND.Attack)
        {
            attackin.SetActive(true);
            num.gameObject.SetActive(true);
            if(action.times == 1)
            {
                num.text = "" + action.num;
            }
            else
            {
                num.text = "" + action.num + "X" + action.times;
            }
        }
        else if (action.Kind == ACTIONKIND.Defense)
        {
            defencein.SetActive(true);
        }
        else if (action.Kind == ACTIONKIND.Debuff)
        {
            debuffin.SetActive(true);
        }
        else if (action.Kind == ACTIONKIND.StrongUP)
        {
            powerupin.SetActive(true);
        }
        else if (action.Kind == ACTIONKIND.Combin)
        {
            foreach(actionAbstract a in action.actionList)
            {
                if (a.Kind == ACTIONKIND.Attack)
                {
                    attackin.SetActive(true);
                    num.gameObject.SetActive(true);
                    if (a.times == 1)
                    {
                        num.text = "" + a.num;
                    }
                    else
                    {
                        num.text = "" + a.num + "X" + a.times;
                    }
                }
                else if (a.Kind == ACTIONKIND.Defense)
                {
                    defencein.SetActive(true);
                }
                else if (a.Kind == ACTIONKIND.Debuff)
                {
                    debuffin.SetActive(true);
                }
                else if (a.Kind == ACTIONKIND.StrongUP)
                {
                    powerupin.SetActive(true);
                }
            }
        }
    }
}
