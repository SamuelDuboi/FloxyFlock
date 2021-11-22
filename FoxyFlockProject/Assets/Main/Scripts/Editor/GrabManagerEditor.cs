using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
[CustomEditor(typeof(GrabManager))]
public class GrabManagerEditor : Editor
{
    SerializedProperty grabableObjects;
    SerializedProperty grabableObjectsToSpawn;
    SerializedProperty numberPerBatch;
    SerializedProperty numbersPerModifer;
    SerializedProperty modifiers;
    private GrabManager managerTarget;
    ReorderableList rlistModifier;
    ReorderableList rlistGrabbableToSpawn;
    private void OnEnable()
    {
        managerTarget = target as GrabManager;
        grabableObjects = serializedObject.FindProperty("grabableObjects");
        grabableObjectsToSpawn = serializedObject.FindProperty("grabableObjectsToSpawn");
        numbersPerModifer = serializedObject.FindProperty("numbersPerModifer");
        numberPerBatch = serializedObject.FindProperty("numberPerBatch");
        modifiers = serializedObject.FindProperty("modifiers");
        rlistModifier = new ReorderableList(serializedObject, modifiers, true, true, true, true);
        rlistModifier.onAddCallback += Add;
        rlistModifier.drawHeaderCallback += HeaderDrawer;
        rlistModifier.onRemoveCallback += Remove;
        rlistModifier.drawElementCallback += ElementDrawer;
        rlistModifier.elementHeightCallback += ElementHeigh;


        rlistGrabbableToSpawn = new ReorderableList(serializedObject, grabableObjectsToSpawn, true, true, true, true);
        rlistGrabbableToSpawn.onAddCallback += AddG;
        rlistGrabbableToSpawn.drawHeaderCallback += HeaderDrawerG;
        rlistGrabbableToSpawn.onRemoveCallback += RemoveG;
        rlistGrabbableToSpawn.drawElementCallback += ElementDrawerG;
        rlistGrabbableToSpawn.elementHeightCallback += ElementHeighG;

        
    }

    public override void OnInspectorGUI()
    {
        managerTarget.modifierFoldout = EditorGUILayout.Foldout(managerTarget.modifierFoldout, "Modifier");
        if (managerTarget.modifierFoldout)
        {
            rlistModifier.DoLayoutList();
            for (int i = 0; i < modifiers.arraySize; i++)
            {
                SerializedProperty prop = modifiers.GetArrayElementAtIndex(i);
                if (prop.serializedObject == null) continue;
                var _object = prop.objectReferenceValue as Modifier;
                if (_object && _object.actions == null)
                {
                    EditorGUILayout.HelpBox("The modifier " + _object.name + " need to have an action linked", MessageType.Error);
                }
                int nextIndex = (i + 1) % modifiers.arraySize;
                SerializedProperty nextProp = modifiers.GetArrayElementAtIndex(nextIndex);
                if (modifiers.arraySize == managerTarget.numbersPerModifer.Count)
                    managerTarget.numbersPerModifer[i] = EditorGUILayout.IntField(managerTarget.modifiers[i].name + "Numbers per batches", managerTarget.numbersPerModifer[i]);

            }
        }

        managerTarget.batcheFoldout = EditorGUILayout.Foldout(managerTarget.batcheFoldout, "Batche");
        if (managerTarget.batcheFoldout)
        {
            rlistGrabbableToSpawn.DoLayoutList();

            for (int i = 0; i < grabableObjectsToSpawn.arraySize; i++)
            {
                SerializedProperty prop = grabableObjectsToSpawn.GetArrayElementAtIndex(i);
                if (prop.serializedObject == null) continue;

                int nextIndex = (i + 1) % grabableObjectsToSpawn.arraySize;
                SerializedProperty nextProp = grabableObjectsToSpawn.GetArrayElementAtIndex(nextIndex);
                if (grabableObjectsToSpawn.arraySize == managerTarget.numberPerBatch.Count && managerTarget.grabableObjectsToSpawn[i])
                    managerTarget.numberPerBatch[i] = EditorGUILayout.IntField(managerTarget.grabableObjectsToSpawn[i].name + "Numbers per batches", managerTarget.numberPerBatch[i]);

            }
        }

        EditorGUILayout.PropertyField(grabableObjects);

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
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    

    private void OnSceneGUI()
    {
        
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }

    void HeaderDrawer(Rect rect)
    {
        EditorGUI.LabelField(rect, "Game mode modifiers");
    }
    void ElementDrawer(Rect rect, int index, bool isActive, bool isFocus)
    {

        EditorGUI.PropertyField(rect, modifiers.GetArrayElementAtIndex(index));
    }
    void Add(ReorderableList rlist)
    {
        modifiers.arraySize++;
        numbersPerModifer.InsertArrayElementAtIndex(numbersPerModifer.arraySize);
        EditorUtility.SetDirty(managerTarget);
        serializedObject.ApplyModifiedProperties();
    }
    void Remove(ReorderableList rlist)
    {
        modifiers.DeleteArrayElementAtIndex(rlist.index);
        if (numbersPerModifer.arraySize > modifiers.arraySize)
            numbersPerModifer.DeleteArrayElementAtIndex(rlist.index);
    }
    void Reorder(ReorderableList rlist)
    {

    }
    float ElementHeigh(int inxed)
    {
        float line = EditorGUIUtility.currentViewWidth > 332 ? 1 : 2;
        float lineHeight = EditorGUIUtility.singleLineHeight + 1;
        return line * lineHeight;
    }

    void HeaderDrawerG(Rect rect)
    {
        EditorGUI.LabelField(rect, "Game mode pieces To Spawn");
    }
    void ElementDrawerG(Rect rect, int index, bool isActive, bool isFocus)
    {

        EditorGUI.PropertyField(rect, grabableObjectsToSpawn.GetArrayElementAtIndex(index));
    }
    void AddG(ReorderableList rlist)
    {
        grabableObjectsToSpawn.arraySize++;
        numberPerBatch.InsertArrayElementAtIndex(numberPerBatch.arraySize);
        EditorUtility.SetDirty(managerTarget);
        serializedObject.ApplyModifiedProperties();
    }
    void RemoveG(ReorderableList rlist)
    {
        grabableObjectsToSpawn.DeleteArrayElementAtIndex(rlist.index);
        if (numberPerBatch.arraySize> grabableObjectsToSpawn.arraySize)
        numberPerBatch.DeleteArrayElementAtIndex(rlist.index);
    }
    void ReorderG(ReorderableList rlist)
    {

    }
    float ElementHeighG(int inxed)
    {
        float line = EditorGUIUtility.currentViewWidth > 332 ? 1 : 2;
        float lineHeight = EditorGUIUtility.singleLineHeight + 1;
        return line * lineHeight;
    }
}
