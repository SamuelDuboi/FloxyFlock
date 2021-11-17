using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GrabManager))]
public class GrabManagerEditor : Editor
{
    SerializedProperty grabableObjects;
    private GrabManager managerTarget;
    private void OnEnable()
    {
        managerTarget = target as GrabManager;
        grabableObjects = serializedObject.FindProperty("grabableObjects");
    }

    public override void OnInspectorGUI()
    {
        for (int i = 0; i < managerTarget.modifiers.Count; i++)
        {
            if (managerTarget.modifiers[i] ==  null) continue;
            if (!managerTarget.modifiers[i].actions)
            {
                EditorGUILayout.HelpBox("The modifier " + managerTarget.modifiers[i].name + " need to have an action linked", MessageType.Error);
            }
        }
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Add all Scenes Grabable"))
        {
            var _object = Object.FindObjectsOfType(typeof(GrabablePhysicsHandler));
            if (_object == null || _object.Length ==0) return;
            for (int i = 0; i < _object.Length; i++)
            {
                grabableObjects.InsertArrayElementAtIndex(grabableObjects.arraySize - 1);
                grabableObjects.GetArrayElementAtIndex(grabableObjects.arraySize - 1).objectReferenceValue = _object[i];
            }
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            
        }
    }
}
