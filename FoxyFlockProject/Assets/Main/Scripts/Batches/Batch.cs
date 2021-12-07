using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Batch 
{
   public List<GameObject> pieces;
    public int weight;
    public ModifierBatch positiveModifier;
    [HideInInspector] public bool isEmpty;
}
[System.Serializable]
public class ModifierBatch
{
    public GameObject piece;
    public Modifier modifier;
}