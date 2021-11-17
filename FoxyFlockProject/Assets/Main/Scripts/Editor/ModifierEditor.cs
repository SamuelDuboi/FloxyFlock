using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Modifier))]
public class ModifierEditor : Editor
{
    private Modifier targeModifier;
    private SerializedProperty asPhysiqueMaterial;
    private SerializedProperty physiqueMaterial;
    private SerializedProperty OnGrabed;
    private SerializedProperty OnHitSomething;
    private SerializedProperty OnHitGround;
    private SerializedProperty OnEnterStasis;
    private SerializedProperty OnReleased;


    private void OnEnable()
    {
        targeModifier = target as Modifier;
        asPhysiqueMaterial = serializedObject.FindProperty("asPhysiqueMaterial");
        physiqueMaterial = serializedObject.FindProperty("physiqueMaterial");
        OnGrabed = serializedObject.FindProperty("OnGrabed");
        OnHitSomething = serializedObject.FindProperty("OnHitSomething");
        OnHitGround = serializedObject.FindProperty("OnHitGround");
        OnEnterStasis = serializedObject.FindProperty("OnEnterStasis");
        OnReleased = serializedObject.FindProperty("OnReleased");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(asPhysiqueMaterial);
        if(asPhysiqueMaterial.boolValue)
            EditorGUILayout.PropertyField(physiqueMaterial);

        EditorGUILayout.PropertyField(OnGrabed);
        EditorGUILayout.PropertyField(OnReleased);
        EditorGUILayout.PropertyField(OnHitSomething, new GUIContent("On Hit Something", "Throw the velocity at the collision and the collider"));
        EditorGUILayout.PropertyField(OnHitGround);
        EditorGUILayout.PropertyField(OnEnterStasis);
    }
}
