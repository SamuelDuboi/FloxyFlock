using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomPropertyDrawer(typeof(Batch))]
public class BatchDrawer : PropertyDrawer
{

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUILayout.PropertyField(property.FindPropertyRelative("pieces"));
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float numberOflIne = 2;
        float lineHeight = EditorGUIUtility.singleLineHeight;
        return lineHeight * numberOflIne;
    }
}
