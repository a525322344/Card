using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realEnemy : MonoBehaviour
{
    public enemybase enemy;
    public monsterInfo monsterinfo;

    public actionAbstract chooseAction()
    {
        return monsterinfo.selectAction(1);
    }
}
