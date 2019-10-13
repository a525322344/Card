using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showcontroll : MonoBehaviour
{
    public Slider healthslider;
    public Text healthnum;
    public enemybase enemybase;
    public GameObject damagepop;
    public List<realAction> realActionList = new List<realAction>();
    public GameObject Armorgo;
    public Text armortext;

    public GameObject playerarmor;
    public Text playerarmortext;

    public GameObject fireState;
    public Text fireText;

    public float distance = 2;
    // Start is called before the first frame update
    void Start()
    {
        ShowArmor(0);
        ShowPlayerArmor(0);
        ShowFire(0);
    }

    // Update is called once per frame
    void Update()
    {
        healthslider.value = enemybase.healthnow;
        healthnum.text = "" + enemybase.healthnow + "/" + enemybase.healthmax;
    }
    public void init()
    {
        enemybase = GetComponent<enemyControll>().pikaqiu;
        healthslider.maxValue = enemybase.healthmax;
        healthslider.minValue = 0;
    }
    public void gethurt(int num)
    {
        GameObject dpp = Instantiate(damagepop, transform);
        dpp.GetComponent<damagePopup>().value = num;
        dpp.GetComponent<damagePopup>().Init(num);
    }
    public void CreateNewAction(actionAbstract action)
    {
        realAction realaction;
        GameObject newRealAction;
        for (int i= realActionList.Count-1; i>=0;i--)
        {
            realaction = realActionList[i];
            realActionList.Remove(realaction);
            Destroy(realaction.gameObject);
        }

        int j = 0;

        switch (action.Kind)
        {
            case ACTIONKIND.Attack:
                newRealAction = Instantiate(gameManager.Instance.instantiatemanager.actionAttack, gameManager.Instance.instantiatemanager.actionTran.position + Vector3.left * distance * j, Quaternion.identity, gameManager.Instance.instantiatemanager.actionTran);
                realaction = newRealAction.GetComponent<realAction>();
                realaction.SetNum(""+action.effects[0].getNum());
                //Debug.Log("" + action.effects[0].getNum());
                realActionList.Add(realaction);
                break;
            case ACTIONKIND.Defense:
                newRealAction = Instantiate(gameManager.Instance.instantiatemanager.actionDefense, gameManager.Instance.instantiatemanager.actionTran.position + Vector3.left * distance * j, Quaternion.identity, gameManager.Instance.instantiatemanager.actionTran);
                realaction = newRealAction.GetComponent<realAction>();
                realaction.SetNum("");
                realActionList.Add(realaction);
                break;
            case ACTIONKIND.Combin:
                foreach(actionAbstract act in action.actionList)
                {
                    switch (act.Kind)
                    {
                        case ACTIONKIND.Attack:
                            newRealAction = Instantiate(gameManager.Instance.instantiatemanager.actionAttack, gameManager.Instance.instantiatemanager.actionTran.position + Vector3.left * distance * 0, Quaternion.identity, gameManager.Instance.instantiatemanager.actionTran);
                            realaction = newRealAction.GetComponent<realAction>();
                            realaction.SetNum("" + action.effects[0].getNum());
                            realActionList.Add(realaction);
                            j++;
                            break;
                        case ACTIONKIND.Defense:
                            newRealAction = Instantiate(gameManager.Instance.instantiatemanager.actionDefense, gameManager.Instance.instantiatemanager.actionTran.position + Vector3.left * distance * 0, Quaternion.identity, gameManager.Instance.instantiatemanager.actionTran);
                            realaction = newRealAction.GetComponent<realAction>();
                            realaction.SetNum("");
                            realActionList.Add(realaction);
                            j++;
                            break;
                    }
                }
                break;
        }
    }

    public void ShowArmor(int num)
    {
        if (num == 0)
        {
            armortext.text = "" + num;
            Armorgo.SetActive(false);
        }
        else
        {
            Armorgo.SetActive(true);
            armortext.text = "" + num;
        }
    }
    public void ShowPlayerArmor(int num)
    {
        if (num == 0)
        {
            playerarmortext.text = "" + num;
            playerarmor.SetActive(false);
        }
        else
        {
            playerarmor.SetActive(true);
            playerarmortext.text = "" + num;
        }
    }

    public void ShowFire(int num)
    {
        if (num == 0)
        {
            fireText.text = "" + num;
            fireState.SetActive(false);
        }
        else
        {
            fireState.SetActive(true);
            fireText.text = "" + num;
        }
    }
}
