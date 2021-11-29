using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Modifier))]
public class ModifierEditor : Editor
{
    private Modifier targeModifier;
    private SerializedProperty hasPhysiqueMaterial;
    private SerializedProperty physiqueMaterial;
    private SerializedProperty mats;
    private SerializedProperty actions;


    private void OnEnable()
    {
        targeModifier = target as Modifier;
        hasPhysiqueMaterial = serializedObject.FindProperty("hasPhysiqueMaterial");
        physiqueMaterial = serializedObject.FindProperty("physiqueMaterial");
        mats = serializedObject.FindProperty("material");
        actions = serializedObject.FindProperty("actions");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(hasPhysiqueMaterial);
     
        if(hasPhysiqueMaterial.boolValue)
            EditorGUILayout.PropertyField(physiqueMaterial);
        targeModifier.actions = (ModifierAction)EditorGUILayout.ObjectField(targeModifier.actions, typeof(ModifierAction), true);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.PropertyField(mats);
        EditorUtility.SetDirty(targeModifier);
        serializedObject.Update();
    }
}
