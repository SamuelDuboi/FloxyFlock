using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName ="NewModifier", menuName ="Modifier",order =1)]
public class Modifier : ScriptableObject
{
    public bool hasPhysiqueMaterial;
    public PhysicMaterial physiqueMaterial;
    public ModifierAction actions;
    public Material material;
    public bool isBasciFloat;

}
