using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//
//新建一个Part过程
//Part();
//addReaction()*n;
//SetinReactions();
[System.Serializable]
public class Part 
{
    //基础信息
    public string name;
    public int id;

    public Part(){}
    public virtual void addReaction(Reaction reaction)
    {
        m_overallReactionList.Add(reaction);
    }
    public virtual void addAndSetin(Reaction reaction)
    {
        Debug.Log("part's set");
        m_overallReactionList.Add(reaction);
        ReactionListController.recesiveReactonToSetIn(reaction);
    }
    public void SetinReactions()
    {
        foreach(Reaction reaction in m_overallReactionList)
        {
            ReactionListController.recesiveReactonToSetIn(reaction);
        }
    }
    protected List<Reaction> m_overallReactionList = new List<Reaction>();
}
[System.Serializable]
public class MagicPart : Part
{
    //功能信息
    public int gridsum;
    public int gridpower;
    public MagicPart() { }
    //暂定：数组a应该是9位
    public MagicPart(int[] a)
    {
        Vector2GridPairs = new Dictionary<Vector2, grid>();
        gridsum = 0;
        for(int i = 0; i < a.Length; i++)
        {
            grid newgrid = new grid(true);
            int posy = i / 3;
            int posx = i % 3;
            newgrid.setPosition(posx, posy);
            newgrid.fatherPart = this;
            if (a[i] == 1)
            {
                newgrid.Opening = true;
                gridsum++;
            }
            else
            {
                newgrid.Opening = false;
            }
            Vector2GridPairs.Add(new Vector2(posx, posy), newgrid);
        }
    }
    public override void addReaction(Reaction reaction)
    {
        reaction.Active = false;
        base.addReaction(reaction);
    }
    public override void addAndSetin(Reaction reaction)
    {
        Debug.Log("magicpart's set");
        reaction.Active = false;
        base.addAndSetin(reaction);
    }
    //
    //激活部件
    //  卡牌在该部件打出时，使该部件的reaction可以响应effect(reaction.Active=true)
    public void activatePart()
    {
        foreach(Reaction reaction in m_overallReactionList)
        {
            reaction.Active = true;
        }
    }
    //
    //休眠部件
    public void sleepPart()
    {
        foreach(Reaction reaction in m_overallReactionList)
        {
            reaction.Active = false;
        }
    }
    public void PowerAllGrid()
    {
        foreach (var g in Vector2GridPairs)
        {
            if (g.Value.Opening)
            {
                g.Value.Power = true;
            }
        }
    }
    public Dictionary<Vector2, grid> getGridDic()
    {
        return Vector2GridPairs;
    }
    //7 8 9
    //4 5 6
    //1 2 3
    private Dictionary<Vector2, grid> Vector2GridPairs;


    public string describe;
}
