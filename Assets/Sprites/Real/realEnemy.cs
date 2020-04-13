using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realEnemy : MonoBehaviour
{
    public pawnbase enemy;
    public monsterInfo monsterinfo;
    public healthSlider healthslider;
    public Transform intentPosi;
    public Transform statePosi;
    public Transform damagePosi;
    public GameObject realStateGO;
    public GameObject damageshow;

    public Animator animator;

    Dictionary<string, realState> nameStatePairs = new Dictionary<string, realState>();
    List<GameObject> statego = new List<GameObject>();

    public void Init(monsterInfo moninfo)
    {
        monsterinfo = moninfo;
        enemy = new enemybase();
        enemy.name = monsterinfo.name;
        enemy.healthmax = monsterinfo.health;
        enemy.healthnow = monsterinfo.health;
        healthslider.Init(enemy);
    }

    public void changeHealthAndArmor(float armor,float heath)
    {
        healthslider.SetSlider(armor, heath);
    }

    public actionAbstract chooseAction()
    {
        return monsterinfo.selectAction(1);
    }
    public void StateUpdtae()
    {
        for(int i = statego.Count - 1; i >= 0; i--)
        {
            Destroy(statego[i]);
        }
        statego.Clear();
        int a = 0;
        foreach(var state in enemy.nameStatePairs)
        {
            GameObject stateg = Instantiate(realStateGO, statePosi);
            stateg.transform.localPosition = stateg.transform.localPosition + Vector3.right * 5*a;
            stateg.GetComponent<realState>().Init(state.Value);
            a++;
        }
    }
    public void showGetHurt(int num)
    {
        StateUpdtae();
        GameObject damage = Instantiate(damageshow, damagePosi);
        Destroy(damage, 5);
        damage.GetComponent<damagePopup>().Init(num);
    }
    public void changeAnimation(int i)
    {
        animator.SetInteger("animaInt", i);
    }
}
