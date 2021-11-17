using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Modifier))]
public class ModifierEditor : Editor
{
    private Modifier targeModifier;
    private SerializedProperty asPhysiqueMaterial;
    private SerializedProperty physiqueMaterial;
    private SerializedProperty actions;


    private void OnEnable()
    {
        targeModifier = target as Modifier;
        asPhysiqueMaterial = serializedObject.FindProperty("asPhysiqueMaterial");
        physiqueMaterial = serializedObject.FindProperty("physiqueMaterial");
        actions = serializedObject.FindProperty("actions");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(asPhysiqueMaterial);
        EditorGUILayout.PropertyField(actions);
        if(asPhysiqueMaterial.boolValue)
            EditorGUILayout.PropertyField(physiqueMaterial);
    }
}
