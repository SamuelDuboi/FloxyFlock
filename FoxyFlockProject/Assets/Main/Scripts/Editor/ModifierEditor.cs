using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Modifier))]
public class ModifierEditor : Editor
{
    private Modifier targeModifier;
    private SerializedProperty hasPhysiqueMaterial;
    private SerializedProperty physiqueMaterial;
    private SerializedProperty mats;
    private SerializedProperty matsT;


    private void OnEnable()
    {
        targeModifier = target as Modifier;
        hasPhysiqueMaterial = serializedObject.FindProperty("hasPhysiqueMaterial");
        physiqueMaterial = serializedObject.FindProperty("physiqueMaterial");
        mats = serializedObject.FindProperty("material");
        matsT = serializedObject.FindProperty("materialt");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(hasPhysiqueMaterial);
     
        if(hasPhysiqueMaterial.boolValue)
            EditorGUILayout.PropertyField(physiqueMaterial);
        targeModifier.actions = (ModifierAction)EditorGUILayout.ObjectField(targeModifier.actions, typeof(ModifierAction), true);
        EditorGUILayout.PropertyField(mats);
        EditorGUILayout.PropertyField(matsT);
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(targeModifier);
        serializedObject.Update();
    }
}
