using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class battleButton : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    { 

    }


    void OnClick()
    {
        Debug.Log("battle");
        SceneManager.LoadScene("SampleScene");
        //先用场景转换实现战斗
    }

}
