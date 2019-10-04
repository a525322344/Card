using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyControll : MonoBehaviour
{
    public enemybase pikaqiu;
    public List<actionAbstract> actions = new List<actionAbstract>();
    private void Start()
    {
        actions.Add(new actionHurt(6));
        actions.Add(new actionHurt(9));
        actions.Add(new actionAdmix(new actionArmor(5), new actionHurt(5)));
    }
    private actionAbstract lastAction;
    public actionAbstract chooseAction()
    {
        actionAbstract result = ListOperation.RandomValue<actionAbstract>(actions);
        //如果和上一次相同，则再随机取一次，但就取这两次了，保证大概率不连着相同
        if (result == lastAction)
        {
            result= ListOperation.RandomValue<actionAbstract>(actions);
        }
        lastAction = result;
        return result;
    }
}
