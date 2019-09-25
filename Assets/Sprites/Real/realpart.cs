using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class realpart : MonoBehaviour
{
    public MagicPart thisMagicPart;
    public GameObject realgridMode;

    public List<realgrid> realgrids = new List<realgrid>();

    public float distance = 0.333f;

    public void setThisMagicPart(MagicPart magicPart)
    {
        thisMagicPart = magicPart;

        //根据magicpart中的grid表，创建realgird
        foreach(var g in thisMagicPart.getGridDic())
        {
            //创建realgril实例添加realgril
            GameObject realgridObject = Instantiate(realgridMode, transform);
            realgrid newrealgrid = realgridObject.GetComponent<realgrid>();
            newrealgrid.setThisGrid(g.Value);
            newrealgrid.fatherPart = this;
            realgridObject.GetComponent<RectTransform>().localPosition = g.Value.getPosition()*distance;

            newrealgrid.Init();
            newrealgrid.changeMaterial();

            realgrids.Add(newrealgrid);
        }
    }
    public bool CanCostPlay(grid bygrid,card selectcard)
    {
        bool result=true;
        int gridx = bygrid.PositionX;
        int gridy = bygrid.PositionY;
        //遍历selectcard,找到有1的cost块
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                //
                if (selectcard.costVector2[i, j] == 1)
                {
                    int costx = j - 1;
                    int costy = i - 1;
                    int fitx = gridx + costx;
                    int fity = gridy + costy;
                    if (thisMagicPart.getGridDic().ContainsKey(new Vector2(fitx, fity)))
                    {
                        grid fitgrid = thisMagicPart.getGridDic()[new Vector2(fitx, fity)];
                        if (fitgrid.Opening && fitgrid.Power)
                        {

                        }
                        else
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
        }

        return result;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
