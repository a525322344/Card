using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realLatice : MonoBehaviour
{
    public Material black_lock;
    public Material white_unlock;
    public Material green_caninstall;

    [HideInInspector]
    public realKnapsack realknapsack;
    public latice thislatice;

    public grid grid;
    private MeshRenderer renderer;

    private void Awake()
    {
        renderer =transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    public void Init(latice l,realKnapsack father)
    {
        thislatice = l;
        realknapsack = father;
        changeColor();
    }

    public bool CanInstallPart(MagicPart magicPart)
    {
        return realknapsack.CanInstallPart(magicPart,thislatice.position);
    }
    public void InstallPart(MagicPart magicPart,out Transform positionTran)
    {
        realknapsack.InstallPart(magicPart, thislatice.position,out positionTran);
    }
    public void ExitCanInstall()
    {
        realknapsack.ExitCanInstall();
    }

    public void changeColor()
    {
        switch (thislatice.state)
        {
            case LaticeState.NotActive:
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
}
