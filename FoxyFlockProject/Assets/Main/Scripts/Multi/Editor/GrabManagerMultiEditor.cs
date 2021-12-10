using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
[CustomEditor(typeof(GrabManagerMulti))]
public class GrabManagerMultiEditor : Editor
{
    SerializedProperty grabableObjects;
    SerializedProperty batches;
    SerializedProperty numberPerBatch;
    SerializedProperty numbersPerModifer;
    SerializedProperty modifiers;
    SerializedProperty representations;

    private GrabManagerMulti managerTarget;
    ReorderableList rlistModifier;
    private bool doOnce;
    private string[] popUpBacthes;
    private void OnEnable()
    {
        managerTarget = target as GrabManagerMulti;
        grabableObjects = serializedObject.FindProperty("grabableObjects");
        batches = serializedObject.FindProperty("batches");
        numbersPerModifer = serializedObject.FindProperty("numbersPerModifer");
        numberPerBatch = serializedObject.FindProperty("numberPerBatch");
        modifiers = serializedObject.FindProperty("modifiers");
        representations = serializedObject.FindProperty("representations");
        rlistModifier = new ReorderableList(serializedObject, modifiers, true, true, true, true);
        rlistModifier.onAddCallback += Add;
        rlistModifier.drawHeaderCallback += HeaderDrawer;
        rlistModifier.onRemoveCallback += Remove;
        rlistModifier.drawElementCallback += ElementDrawer;
        rlistModifier.elementHeightCallback += ElementHeigh;

    
    }

    public override void OnInspectorGUI()
    {
        if (!doOnce)
        {
            doOnce = true;
            Object[] tempflockes = Resources.LoadAll("Floxes", typeof(GameObject));
            managerTarget.allflocks = new GameObject[tempflockes.Length];
            popUpBacthes = new string[tempflockes.Length];
            for (int i = 0; i < tempflockes.Length; i++)
            {
                managerTarget.allflocks[i] = (GameObject)tempflockes[i];
                popUpBacthes[i] = managerTarget.allflocks[i].name;
            }
        }
        managerTarget.basicMats[0] = (PhysicMaterial) EditorGUILayout.ObjectField("Grabed mat" ,managerTarget.basicMats[0], typeof(PhysicMaterial), true);
        managerTarget.basicMats[1] = (PhysicMaterial) EditorGUILayout.ObjectField("default released mat",managerTarget.basicMats[1], typeof(PhysicMaterial), true);
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
        managerTarget.fireBallPrefab = (GameObject)EditorGUILayout.ObjectField("Fire ball prefab", managerTarget.fireBallPrefab, typeof(GameObject), true);
        managerTarget.fireBallPrefabOut = (GameObject)EditorGUILayout.ObjectField("Fire ball prefab out", managerTarget.fireBallPrefabOut, typeof(GameObject), true);
        EditorGUILayout.PropertyField(batches);
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
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(representations);
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(managerTarget);
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


}
