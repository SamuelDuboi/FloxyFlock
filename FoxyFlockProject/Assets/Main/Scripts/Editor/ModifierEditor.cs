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
     
        if(asPhysiqueMaterial.boolValue)
            EditorGUILayout.PropertyField(physiqueMaterial);
        targeModifier.actions = (ModifierAction)EditorGUILayout.ObjectField(targeModifier.actions, typeof(ModifierAction), true);
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(targeModifier);
        serializedObject.Update();
    }
}
