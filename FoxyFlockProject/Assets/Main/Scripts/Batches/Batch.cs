using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Batch 
{
   public List<GameObject> pieces;
    public int weight;
    public ModifierBatch batchModifier;
    [HideInInspector] public bool isEmpty;
}
[System.Serializable]
public class ModifierBatch
{
    public List< GameObject > positiveModifiers;
    public List< GameObject > negativeModifier;
}