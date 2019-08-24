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
        foreach(grid g in thisMagicPart.getGridList())
        {
            //创建realgril实例添加realgril
            GameObject realgridObject = Instantiate(realgridMode, transform);
            realgrid newrealgrid = realgridObject.GetComponent<realgrid>();
            newrealgrid.setThisGrid(g);
            realgridObject.GetComponent<RectTransform>().localPosition = g.getPosition()*distance;

            newrealgrid.Init();
            newrealgrid.changeMaterial();

            realgrids.Add(newrealgrid);
        }
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
