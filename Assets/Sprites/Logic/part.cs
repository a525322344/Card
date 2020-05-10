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
        gameManager.Instance.battlemanager.ReactionListController.recesiveReactonToSetIn(reaction);
    }
    public void SetinReactions()
    {
        foreach(Reaction reaction in m_overallReactionList)
        {
            gameManager.Instance.battlemanager.ReactionListController.recesiveReactonToSetIn(reaction);
        }
    }
    public List<Reaction> m_overallReactionList = new List<Reaction>();
    protected void debugReactionList()
    {
        foreach(Reaction r in m_overallReactionList)
        {
            Debug.Log(name + ":" + r.name);
        }
    }

    protected string ColorGold = "<color=#CFB53B>";
    protected string ColorBlue = "<color=#007FFF>";
    protected string ColorGreen = "<color=#32CD32>";
    protected string ColorEnd = "</color>";
}
public class LinkPart : MagicPart
{
    public LinkPart(List<MagicPart> magicParts)
    {
        name = "链接部件";
        gridsum = 0;
        Vector2GridPairs = new Dictionary<Vector2, grid>();
        foreach(var magicpart in magicParts)
        {
            gridsum += magicpart.gridsum;
            foreach(Reaction r in magicpart.m_overallReactionList)
            {
                base.addReaction(r);
            }
            //foreach(var p in magicpart.Vector2GridPairs)
            //{
            //    Vector2GridPairs.Add(p.Key,p.Value);
            //}
        }
        //debugReactionList();
    }

}
[System.Serializable]
public class MagicPart : Part
{
    public int rank;
    //功能信息
    public int gridsum;
    public int gridpower;
    public MagicPart() { }
    //暂定：数组a应该是9位
    public MagicPart(string _name,int[] a,int set)
    {
        name = _name;
        rotateInt = 0;
        Vector2GridPairs = new Dictionary<Vector2, grid>();
        gridsum = 0;
        for(int i = 0; i < a.Length; i++)
        {
            //1
            //0
            //-1  
            //  -1  0  1
            grid newgrid = new grid(true);
            int posy = i / 3-1 ;
            int posx = i % 3-1 ;
            newgrid.position = new Vector2(posx, posy);
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
        Vector2GridRotate = new Dictionary<Vector2, grid>(Vector2GridPairs);
    }
    public MagicPart(MagicPart magicPart)
    {
        name = magicPart.name;
        rotateInt = 0;
        Vector2GridPairs = new Dictionary<Vector2, grid>(magicPart.Vector2GridPairs);
        gridsum = 0;
        Vector2GridRotate = new Dictionary<Vector2, grid>(magicPart.Vector2GridPairs);
        foreach(Reaction r in magicPart.m_overallReactionList)
        {
            Reaction newreaction = ReflectOperation.NewClassByReflect<Reaction>(r);
            m_overallReactionList.Add(newreaction);
        }
        describe = magicPart.describe;
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

    //7 8 9
    //4 5 6
    //1 2 3
    //储存初始grid信息
    public Dictionary<Vector2, grid> Vector2GridPairs;
    //储存旋转后的信息
    public int rotateInt;
    public Dictionary<Vector2, grid> Vector2GridRotate;
    //参数只能是1，或-1，表示正方向旋转与负方向旋转
    //二维旋转矩阵
    // |X| =|cos  -sin|*|x|
    // |Y|  |sin  con | |y|
    public void RotatePart(int r)
    {
        if (r == 1)
        {
            if (rotateInt == 3)
            {
                rotateInt = 0;
            }
            else
            {
                rotateInt++;
            }
        }
        else if(r == -1)
        {
            if (rotateInt == 0)
            {
                rotateInt = 3;
            }
            else
            {
                rotateInt--;
            }
        }
        Vector2GridRotate.Clear();
        foreach (var vg in Vector2GridPairs)
        {
            float angle = (float)rotateInt*90 * Mathf.Deg2Rad;
            Vector2 newvec = new Vector2(
                (int)Mathf.Cos(angle) * vg.Key.x - (int)Mathf.Sin(angle) * vg.Key.y,
                (int)Mathf.Sin(angle) * vg.Key.x + (int)Mathf.Cos(angle) * vg.Key.y);
            
            vg.Value.position = newvec;
            Vector2GridRotate.Add(newvec, vg.Value);
        }
    }

    public string partDescribe()
    {
        describe = ColorGold+ name+"\n"+ColorEnd;
        foreach(Reaction reaction in m_overallReactionList)
        {
            describe += ColorBlue+ reaction.name +ColorEnd+ ":" + reaction.ReactionDescribe() + "\n";
        }
        return describe;
    }
    public string describe;
}
