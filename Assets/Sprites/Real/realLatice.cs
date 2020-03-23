using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GridState
{
    NotExploit,     //未开发的  纸板上没有解锁的格子
    NotActive,      //未激活的  解锁了但是没有安放部件的格子（也不能使用，但是可以在战斗中临时激活
    Power,          //可以使用
    Can,
    Used,           //使用过的
}
public class realLatice : MonoBehaviour
{
    #region 外部资源
    public Material black_lock;
    public Material white_unlock;
    public Material green_caninstall;

    public Material grid_lock;
    public Material grid_power;
    public Material grid_can;
    public Material grid_used;
    #endregion
    [HideInInspector]
    public realKnapsack realknapsack;
    public latice thislatice;
    public GameState gameState;
    public GridState gridState;


    public realpart realpart;
    public realgrid realgrid;
    private MeshRenderer renderer;

    public void Init(latice l,realKnapsack father,GameState state)
    {
        renderer = transform.GetChild(0).GetComponent<MeshRenderer>();

        gameState = state;
        thislatice = l;
        realknapsack = father;

        if (gameState == GameState.MapSence)
        {
            changeColor();
        }
        else if(gameState == GameState.BattleSence)
        {
            switch (thislatice.state)
            {
                case LaticeState.NotExploit:
                    gridState = GridState.NotExploit;
                    gameObject.SetActive(false);
                    break;
                case LaticeState.Exploit:
                    gridState = GridState.NotActive;
                    break;
                case LaticeState.Install:
                    gridState = GridState.Power;
                    break;
                case LaticeState.CanInstall:
                    Debug.Log("错误，战斗下的Latice的state不能是CanInstall");
                    break;
            }
            changeColor();
        }
    }
    //地图操作
    public bool CanInstallPart(MagicPart magicPart)
    {
        return realknapsack.CanInstallPart(magicPart,thislatice.position);
    }
    public void InstallPart(MagicPart magicPart,out Transform positionTran)
    {
        realknapsack.InstallPart(magicPart, thislatice.position,out positionTran);
    }
    public void ExitInstall()
    {
        realknapsack.ExitInstall(thislatice.position);
    }
    public void ExitCanInstall()
    {
        realknapsack.ExitCanInstall();
    }
    //战斗操作
    public bool CanCostPlay(Dictionary<Vector2, int> vectorInts)
    {
        return realknapsack.CanCostPlay(thislatice.position, vectorInts);
    }
    public void ToSetPart(card _selectcard)
    {
        realknapsack.ToSetPart(_selectcard);
    }
    public void BackSetLitice(bool canplay)
    {
        if (gridState != GridState.Used)
        {
            if (canplay)
            {
                gridState = GridState.Can;
                realgrid.gridState = GridState.Can;
            }
            else
            {
                gridState = GridState.Power;
                realgrid.gridState = GridState.Power;
            }
            realgrid.changeMaterial();
            changeColor();
        }
    }

    public void changeColor()
    {
        if (gameState == GameState.MapSence)
        {
            switch (thislatice.state)
            {
                case LaticeState.NotExploit:
                    renderer.material = black_lock;
                    break;
                case LaticeState.Exploit:
                    renderer.material = white_unlock;
                    break;
                case LaticeState.CanInstall:
                    renderer.material = green_caninstall;
                    break;
                case LaticeState.Install:
                    break;
            }
        }
        else if(gameState==GameState.BattleSence)
        {
            switch (gridState)
            {
                case GridState.NotExploit:
                    //Debug.Log("gridstate.NotExploit不该触发改变颜色");
                    break;
                case GridState.NotActive:
                    renderer.material = grid_lock;
                    break;
                case GridState.Power:
                    renderer.material = grid_power;
                    break;
                case GridState.Can:
                    renderer.material = grid_can;
                    break;
                case GridState.Used:
                    renderer.material = grid_used;
                    break;
            }
        }
    }
    public void changeColor(GridState state)
    {
        gridState = state;
        if (gameState == GameState.MapSence)
        {
            switch (thislatice.state)
            {
                case LaticeState.NotExploit:
                    renderer.material = black_lock;
                    break;
                case LaticeState.Exploit:
                    renderer.material = white_unlock;
                    break;
                case LaticeState.CanInstall:
                    renderer.material = green_caninstall;
                    break;
                case LaticeState.Install:
                    break;
            }
        }
        else if (gameState == GameState.BattleSence)
        {
            switch (gridState)
            {
                case GridState.NotExploit:
                    //Debug.Log("gridstate.NotExploit不该触发改变颜色");
                    break;
                case GridState.NotActive:
                    renderer.material = grid_lock;
                    break;
                case GridState.Power:
                    renderer.material = grid_power;
                    break;
                case GridState.Can:
                    renderer.material = grid_can;
                    break;
                case GridState.Used:
                    renderer.material = grid_used;
                    break;
            }
        }
    }
}
