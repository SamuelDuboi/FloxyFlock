using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MilestoneManager))]
public class MilestoneManagerEditor : Editor
{
    SerializedProperty numberOfMilestones;
    SerializedProperty milestonePrefab;
    SerializedProperty _transform;
    SerializedProperty milestonesInstantiated;
    SerializedProperty _tableTransform;
    SerializedProperty distance;
    public void OnEnable()
    {
        numberOfMilestones = serializedObject.FindProperty("numberOfMilestones");
        milestonePrefab = serializedObject.FindProperty("milestonePrefab");
        _transform = serializedObject.FindProperty("_transform");
        if(_transform.objectReferenceValue == null)
        {
            MilestoneManager _target = target as MilestoneManager;
            _transform.objectReferenceValue = _target.transform;
        }


        _tableTransform = serializedObject.FindProperty("_tableTransform");
        if (_tableTransform.objectReferenceValue == null)
        {
            MilestoneManager _target = target as MilestoneManager;
            _tableTransform.objectReferenceValue = _target.transform.parent.GetChild(0).transform;
        }

        milestonesInstantiated = serializedObject.FindProperty("milestonesInstantiated");
       if(numberOfMilestones.intValue == 0)
        {
            GameObject currentMilestone = (GameObject)Instantiate(milestonePrefab.objectReferenceValue, (Transform)_transform.objectReferenceValue);
            currentMilestone.GetComponent<Milestone>().isFinale = true;
            milestonesInstantiated.InsertArrayElementAtIndex(milestonesInstantiated.arraySize );
            milestonesInstantiated.GetArrayElementAtIndex(milestonesInstantiated.arraySize-1).objectReferenceValue = currentMilestone;
            numberOfMilestones.intValue++;
        }

        distance = serializedObject.FindProperty("distance");
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(milestonePrefab, new GUIContent("Milstone Prefab"));
        if(milestonePrefab.objectReferenceValue != null)
        {
            EditorGUILayout.LabelField("Number of  milestones " +numberOfMilestones.intValue.ToString());
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+", EditorStyles.miniButton) )
                AddMilestone();
            if(numberOfMilestones.intValue>0)
                if (GUILayout.Button("-", EditorStyles.miniButton))
                    RemoveMilestone();
            EditorGUILayout.EndHorizontal();
                
            
        }
        distance.floatValue = Vector3.Distance(((Transform)_transform.objectReferenceValue).position, ((Transform)_tableTransform.objectReferenceValue).position);

        ActualiseMilestonesPos();
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    private void AddMilestone()
    {
        GameObject currentMilestone = (GameObject)Instantiate(milestonePrefab.objectReferenceValue, (Transform)_transform.objectReferenceValue);
        distance.floatValue = Vector3.Distance(((Transform)_transform.objectReferenceValue).position, ((Transform)_tableTransform.objectReferenceValue).position);
        currentMilestone.transform.position -= Vector3.up * (distance.floatValue / (numberOfMilestones.intValue + 1)) * numberOfMilestones.intValue;
        milestonesInstantiated.InsertArrayElementAtIndex(milestonesInstantiated.arraySize - 1);
        milestonesInstantiated.GetArrayElementAtIndex(milestonesInstantiated.arraySize - 1).objectReferenceValue = currentMilestone;
        numberOfMilestones.intValue++;
    }
    private void RemoveMilestone()
    {
        DestroyImmediate(milestonesInstantiated.GetArrayElementAtIndex(milestonesInstantiated.arraySize - 1).objectReferenceValue);
        milestonesInstantiated.DeleteArrayElementAtIndex(milestonesInstantiated.arraySize - 1);
        numberOfMilestones.intValue--;
    }
    private void ActualiseMilestonesPos()
    {
        for (int i = 1; i < numberOfMilestones.intValue; i++)
        {
          ((GameObject)  milestonesInstantiated.GetArrayElementAtIndex(i).objectReferenceValue).transform.position = 
                ((Transform)_transform.objectReferenceValue).position 
                - Vector3.up 
                * (distance.floatValue/ (numberOfMilestones.intValue)) 
                * i;
        }
    }
}
