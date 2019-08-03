using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class part : MonoBehaviour
{
    [SerializeField]
    public grid[,] grids = new grid[3,3];
    [SerializeField]
    public bool[] gridbools = { false, false, false, false, true, false, false, true, false };

    public GameObject realgridMode;
    public List<realgrid> realgrids = new List<realgrid>();
    public float distance = 0.333f;
    // Start is called before the first frame update
    void Start()
    {
        //根据bool数组创建grid类
        for(int i = 0; i < 9; i++)
        {
            grids[i / 3, i % 3] = new grid(gridbools[i]);
        }
        //根据grid类二维数组创建格子实例
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                GameObject realgrid = Instantiate(realgridMode,transform);
                realgrid.GetComponent<RectTransform>().localPosition = new Vector3((j - 1) * distance, (i - 1) * distance, 0);
                realgrid nowrealgrid = realgrid.GetComponent<realgrid>();
                nowrealgrid.Init();
                nowrealgrid.thisgrig = grids[i, j];
                nowrealgrid.changeMaterial();
                realgrids.Add(nowrealgrid);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
