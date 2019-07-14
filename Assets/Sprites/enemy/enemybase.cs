using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemybase : pawnbase
{
    public void HurtHealth(int i)
    {
        if (i >= 0)
        {
            healthnow -= i;
        }
    }
    public void GetArmor(int i)
    {
        if (i >= 0)
        {
            armor += i;
        }
    }
}
