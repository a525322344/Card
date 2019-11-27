using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realEnemy : MonoBehaviour
{
    public enemybase enemy;
    public monsterInfo monsterinfo;

    public void Init(monsterInfo moninfo)
    {
        monsterinfo = moninfo;
        enemy = new enemybase();
        enemy.name = monsterinfo.name;
        enemy.healthmax = monsterinfo.health;
        enemy.healthnow = monsterinfo.health;
    }

    public actionAbstract chooseAction()
    {
        return monsterinfo.selectAction(1);
    }
}
