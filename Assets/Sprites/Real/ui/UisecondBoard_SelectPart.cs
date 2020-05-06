using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UisecondBoard_SelectPart : uiSecondBoard
{
    public Transform partZeroPosition;
    public TextMeshPro describeText;
    public int numHor;
    public float distanceHor;

    //数据上
    public thingToSelect<MagicPart> partsToSelect;
    public int selectNum = 1;
    public List<MagicPart> freePartList;
    public List<MagicPart> selectPartList = new List<MagicPart>();
    //外部设置
    public onSelectCards<MagicPart> onSelectParts;
    public spriteButton CancelButton;

    void Update()
    {
        
    }
    public void Init(List<MagicPart> partlist,int selectnum)
    {
        freePartList = new List<MagicPart>(partlist);
        selectNum = selectnum;
        partsToSelect = new thingToSelect<MagicPart>();
        partsToSelect.onSelectcard += (_part) =>
        {
            bool once = true;
            if (freePartList.Contains(_part) && once)
            {
                once = false;
                freePartList.Remove(_part);
                selectPartList.Add(_part);
            }
            else if (selectPartList.Contains(_part) && once)
            {
                once = false;
                selectPartList.Remove(_part);
                freePartList.Add(_part);
            }
            if (selectPartList.Count == selectNum)
            {
                Exit();
            }
        };

        float starth = -(float)(partlist.Count - 1) / 2;
        for(int i = 0; i < partlist.Count; i++)
        {
            GameObject partGO = Instantiate(instantiateManager.instance.partGO, partZeroPosition);
            partGO.transform.localPosition = new Vector3((starth + i) * distanceHor, 0, 0);
            realpart rp = partGO.GetComponent<realpart>();
            rp.Init(partlist[i], RealPartState.Select);
            rp.partToSelect = partsToSelect;
        }
        
    }
    public override void Exit()
    {
        base.Exit();
        onSelectParts(selectPartList);
        Destroy(gameObject);
    }
}
