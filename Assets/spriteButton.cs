using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteButton : MonoBehaviour
{
    private toDo onclick = () => { };
    private void OnMouseDown()
    {
        onclick();
    }
    public void AddListener(toDo todo)
    {
        onclick += todo;
    }
}
