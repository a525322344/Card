using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "InitControllBoard")]
public class InitControllBoard : ScriptableObject
{
    public List<int> carddeckInit = new List<int>();
    public bool[] knapsackLaticInit = new bool[25]; 
}
