using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class anim_play : MonoBehaviour
{
    private Animator m_anim;
    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            m_anim.SetBool("attack", true);
        }
        else { m_anim.SetBool("attack", false); }
        if (Input.GetKey(KeyCode.S))
        {
            m_anim.SetBool("defense", true);
        }
        else { m_anim.SetBool("defense", false); }
        if (Input.GetKey(KeyCode.D))
        {
            m_anim.SetBool("hurted", true);
        }
        else { m_anim.SetBool("hurted", false); }
    }
}
