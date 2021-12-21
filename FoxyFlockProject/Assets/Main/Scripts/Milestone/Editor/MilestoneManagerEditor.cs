using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MilestoneManager))]
[CanEditMultipleObjects]
public class MilestoneManagerEditor : Editor
{
    SerializedProperty numberOfMilestones;
    SerializedProperty milestonePrefab;
    SerializedProperty _transform;
    SerializedProperty milestonesInstantiated;
    SerializedProperty _tableTransform;
    SerializedProperty distance;
    SerializedProperty milestones;
    public void OnEnable()
    {
        numberOfMilestones = serializedObject.FindProperty("numberOfMilestones");
        milestonePrefab = serializedObject.FindProperty("milestonePrefab");
        _transform = serializedObject.FindProperty("_transform");
        MilestoneManager _target = target as MilestoneManager;
        if(_transform.objectReferenceValue == null)
        {
            _transform.objectReferenceValue = _target.transform;
        }
        _target.transform.parent.GetComponent<PlayGround>().milestoneManager = _target;

        _tableTransform = serializedObject.FindProperty("_tableTransform");
        if (_tableTransform.objectReferenceValue == null)
        {
            _tableTransform.objectReferenceValue = _target.transform.parent.GetChild(0).transform;
        }

        milestonesInstantiated = serializedObject.FindProperty("milestonesInstantiated");
        milestones = serializedObject.FindProperty("milestones");
       if(numberOfMilestones.intValue == 0)
        {
            GameObject currentMilestone = (GameObject)PrefabUtility.InstantiatePrefab(milestonePrefab.objectReferenceValue, (Transform)_transform.objectReferenceValue);
            currentMilestone.GetComponent<Milestone>().isFinale = true;
            milestonesInstantiated.InsertArrayElementAtIndex(milestonesInstantiated.arraySize );
            milestones.InsertArrayElementAtIndex(milestones.arraySize );
            milestonesInstantiated.GetArrayElementAtIndex(milestonesInstantiated.arraySize-1).objectReferenceValue = currentMilestone;
            milestones.GetArrayElementAtIndex(milestones.arraySize-1).objectReferenceValue = currentMilestone.GetComponent<Milestone>();
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
        GameObject currentMilestone = (GameObject)PrefabUtility.InstantiatePrefab(milestonePrefab.objectReferenceValue, (Transform)_transform.objectReferenceValue);
        distance.floatValue = Vector3.Distance(((Transform)_transform.objectReferenceValue).position, ((Transform)_tableTransform.objectReferenceValue).position);
        currentMilestone.transform.position -= Vector3.up * (distance.floatValue / (numberOfMilestones.intValue + 1)) * numberOfMilestones.intValue;
        currentMilestone.GetComponent<MeshRenderer>().enabled= false;
        milestonesInstantiated.InsertArrayElementAtIndex(milestonesInstantiated.arraySize - 1);
        milestones.InsertArrayElementAtIndex(milestones.arraySize - 1);
        milestonesInstantiated.GetArrayElementAtIndex(milestonesInstantiated.arraySize - 1).objectReferenceValue = currentMilestone;
        milestones.GetArrayElementAtIndex(milestones.arraySize - 1).objectReferenceValue = currentMilestone.GetComponent<Milestone>();
        numberOfMilestones.intValue++;
    }
    private void RemoveMilestone()
    {
        DestroyImmediate(milestonesInstantiated.GetArrayElementAtIndex(milestonesInstantiated.arraySize - 1).objectReferenceValue);
        milestonesInstantiated.DeleteArrayElementAtIndex(milestonesInstantiated.arraySize - 1);
        milestonesInstantiated.arraySize--;
        milestones.arraySize--;
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
