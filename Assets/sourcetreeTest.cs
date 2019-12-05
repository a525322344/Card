using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class b
{
    public int i;
    public b(int a)
    {
        i = a;
    }
}

public class sourcetreeTest : MonoBehaviour
{
    public int a;
    public List<b> show = new List<b>();
    public List<b> other = new List<b>();
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("UMP45AA@*)SUERPOJA8j=-fdsfsf2346%$^%TWdfposj98u092j");
        Debug.Log("MeAqua!!");
        Debug.Log("kirakira");
        show.Add(new b(3));
        other = new List<b>(show);
        other[0].i = 1;
        other.Remove(other[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
