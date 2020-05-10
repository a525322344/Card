using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class realEnemy : MonoBehaviour
{
    public pawnbase enemy;
    public monsterInfo monsterinfo;
    public healthSlider healthslider;
    public realIntent intent;

    public Transform damagePosi;
    public GameObject realStateGO;
    public GameObject damageshow;

    public Animator animator;
    public TextMeshPro nameTextmesh;
    public bool mouseOver;

    Dictionary<string, realState> nameStatePairs = new Dictionary<string, realState>();
    List<GameObject> statego = new List<GameObject>();
    private actionAbstract nowaction;

    public void Init(monsterInfo moninfo)
    {
        monsterinfo = moninfo;
        enemy = new enemybase();
        enemy.name = monsterinfo.name;
        enemy.healthmax = monsterinfo.health;
        enemy.healthnow = monsterinfo.health;
        healthslider.Init(enemy);
        color0 = new Color(nameTextmesh.color.r, nameTextmesh.color.g, nameTextmesh.color.b, 0);
        color1 = new Color(nameTextmesh.color.r, nameTextmesh.color.g, nameTextmesh.color.b, 1);
    }
    Color color0;
    Color color1;
    private void Update()
    {
        if (mouseOver)
        {
            DOTween.To(() => nameTextmesh.color, x => nameTextmesh.color = x, color1, 0.5f);
        }
        else
        {
            DOTween.To(() => nameTextmesh.color, x => nameTextmesh.color = x, color0, 0.5f);
        }
    }
    public void changeHealthAndArmor(float armor,float heath)
    {
        healthslider.SetSlider(armor, heath);
    }

    public actionAbstract chooseAction()
    {
        nowaction= monsterinfo.selectAction(1);
        return nowaction;
    }
    public void ShowAction(bool isshow)
    {
        if (true)
        {
            intent.Init(nowaction);
        }
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
            GameObject stateg = Instantiate(realStateGO, healthslider.statePosi);
            statego.Add(stateg);
            stateg.transform.localPosition = stateg.transform.localPosition + Vector3.left * healthslider.statedistance*a;
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
    private void OnMouseEnter()
    {
        mouseOver = true;
    }
    private void OnMouseExit()
    {
        mouseOver = false;
    }
}
