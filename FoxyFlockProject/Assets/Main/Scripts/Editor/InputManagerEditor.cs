using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof (InputManager))]
[ExecuteInEditMode]

public class InputManagerEditor : Editor
{

    private InputManager player;
    private bool eventFloldout;

    private SerializedProperty OnLeftTrigger;
    private SerializedProperty OnRightTrigger;
    private SerializedProperty OnBothTrigger;
    private SerializedProperty OnLeftTriggerRelease;
    private SerializedProperty OnRightTriggerRelease;
    private SerializedProperty OnCanMove;
    private void OnEnable()
    {
        player = target as InputManager;
        OnLeftTrigger = serializedObject.FindProperty("OnLeftTrigger");
        OnRightTrigger = serializedObject.FindProperty("OnRightTrigger");
        OnBothTrigger = serializedObject.FindProperty("OnBothTrigger");
        OnLeftTriggerRelease = serializedObject.FindProperty("OnLeftTriggerRelease");
        OnRightTriggerRelease = serializedObject.FindProperty("OnRightTriggerRelease");
        OnCanMove = serializedObject.FindProperty("OnCanMove");
    }
    public override void OnInspectorGUI()
    {

        player.rightHand = (HandController) EditorGUILayout.ObjectField("Right hand", player.rightHand, typeof(HandController), true);
        player.leftHand = (HandController) EditorGUILayout.ObjectField("Left hand", player.leftHand, typeof(HandController), true);
        player.characterStats = (CharacterStats) EditorGUILayout.ObjectField("Character stats", player.characterStats, typeof(CharacterStats), true);
        eventFloldout = EditorGUILayout.Foldout(eventFloldout, "Events");
        if (eventFloldout)
        {
            EditorGUILayout.PropertyField(OnLeftTrigger);
            EditorGUILayout.PropertyField(OnRightTrigger);
            EditorGUILayout.PropertyField(OnBothTrigger);
            EditorGUILayout.PropertyField(OnLeftTriggerRelease);
            EditorGUILayout.PropertyField(OnRightTriggerRelease);
            EditorGUILayout.PropertyField(OnCanMove);
        }


    EditorUtility.SetDirty(player);
        Repaint();
        serializedObject.ApplyModifiedProperties();
    }
}
