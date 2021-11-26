using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Batch 
{
   public List<GameObject> pieces;
    public int weight;
    [HideInInspector] public bool isEmpty;
}
