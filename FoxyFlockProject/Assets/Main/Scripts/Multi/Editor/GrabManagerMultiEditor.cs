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
    SerializedProperty positiveModifiers;
    SerializedProperty negativeModifiers;
    SerializedProperty baseModifier;
    SerializedProperty weightOfBasicInRandom;
    SerializedProperty representations;
    SerializedProperty representationsModifiers;
    SerializedProperty fireballRepresentation;
    SerializedProperty nextIsFireBallBatche;

    ReorderableList rlistPositiveModifier;
    ReorderableList rlisNegativeModifier;

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
        positiveModifiers = serializedObject.FindProperty("positiveModifiers");
        negativeModifiers = serializedObject.FindProperty("negativeModifiers");
        baseModifier = serializedObject.FindProperty("baseModifier");
        weightOfBasicInRandom = serializedObject.FindProperty("weightOfBasicInRandom");
        representations = serializedObject.FindProperty("representations");
        representationsModifiers = serializedObject.FindProperty("representationsModifiers");
        fireballRepresentation = serializedObject.FindProperty("fireballRepresentation");
        nextIsFireBallBatche = serializedObject.FindProperty("nextIsFireBallBatche");

        rlistPositiveModifier = new ReorderableList(serializedObject, positiveModifiers, true, true, true, true);
        rlistPositiveModifier.onAddCallback += Add;
        rlistPositiveModifier.drawHeaderCallback += HeaderDrawer;
        rlistPositiveModifier.onRemoveCallback += Remove;
        rlistPositiveModifier.drawElementCallback += ElementDrawer;
        rlistPositiveModifier.elementHeightCallback += ElementHeigh;

        rlisNegativeModifier = new ReorderableList(serializedObject, negativeModifiers, true, true, true, true);
        rlisNegativeModifier.onAddCallback += AddN;
        rlisNegativeModifier.drawHeaderCallback += HeaderDrawerN;
        rlisNegativeModifier.onRemoveCallback += RemoveN;
        rlisNegativeModifier.drawElementCallback += ElementDrawerN;
        rlisNegativeModifier.elementHeightCallback += ElementHeigh;


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
        managerTarget.basicMats[1] = (PhysicMaterial) EditorGUILayout.ObjectField("Default released mat",managerTarget.basicMats[1], typeof(PhysicMaterial), true);
        managerTarget.modifierFoldout = EditorGUILayout.Foldout(managerTarget.modifierFoldout, "Modifier");
        if (managerTarget.modifierFoldout)
        {
            EditorGUILayout.PropertyField(baseModifier, new GUIContent("Base modifier "));
            if (baseModifier.objectReferenceValue != null)
                EditorGUILayout.PropertyField(weightOfBasicInRandom, new GUIContent("  Weight of base  modifier "));
            rlistPositiveModifier.DoLayoutList();
            for (int i = 0; i < positiveModifiers.arraySize; i++)
            {
                SerializedProperty prop = positiveModifiers.GetArrayElementAtIndex(i);
                if (prop.serializedObject == null) continue;
                var _object = prop.objectReferenceValue as Modifier;
                if (_object && _object.actions == null)
                {
                    EditorGUILayout.HelpBox("The modifier " + _object.name + " need to have an action linked", MessageType.Error);
                }
               /* int nextIndex = (i + 1) % positiveModifiers.arraySize;
                SerializedProperty nextProp = positiveModifiers.GetArrayElementAtIndex(nextIndex);
                if (positiveModifiers.arraySize == managerTarget.numbersPerModifer.Count)
                    managerTarget.numbersPerModifer[i] = EditorGUILayout.IntField(managerTarget.positiveModifiers[i].name + "Numbers per batches", managerTarget.numbersPerModifer[i]);*/

            }
            rlisNegativeModifier.DoLayoutList();
            for (int i = 0; i < negativeModifiers.arraySize; i++)
            {
                SerializedProperty prop = negativeModifiers.GetArrayElementAtIndex(i);
                if (prop.serializedObject == null) continue;
                var _object = prop.objectReferenceValue as Modifier;
                if (_object && _object.actions == null)
                {
                    EditorGUILayout.HelpBox("The modifier " + _object.name + " need to have an action linked", MessageType.Error);
                }
                /*int nextIndex = (i + 1) % negativeModifiers.arraySize;
                SerializedProperty nextProp = negativeModifiers.GetArrayElementAtIndex(nextIndex);
                if (negativeModifiers.arraySize == managerTarget.negativeModifiers.Count)
                    managerTarget.numbersPerModifer[i] = EditorGUILayout.IntField(managerTarget.negativeModifiers[i].name + "Numbers per batches", managerTarget.numbersPerModifer[i]);*/

            }

        }
        managerTarget.fireBallPrefab = (GameObject)EditorGUILayout.ObjectField("Fire ball prefab", managerTarget.fireBallPrefab, typeof(GameObject), true);
        managerTarget.fireBallPrefabOut = (GameObject)EditorGUILayout.ObjectField("Fire ball prefab out", managerTarget.fireBallPrefabOut, typeof(GameObject), true);

        EditorGUILayout.PropertyField(nextIsFireBallBatche, new GUIContent("First batch has fireball","if not the second bacth will have a fireBall"));
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
        EditorGUILayout.PropertyField(representationsModifiers);
        EditorGUILayout.PropertyField(fireballRepresentation);

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
        EditorGUI.LabelField(rect, "Positives modifiers");
    }
    void HeaderDrawerN(Rect rect)
    {
        EditorGUI.LabelField(rect, "Negative modifiers");
    }
    void ElementDrawer(Rect rect, int index, bool isActive, bool isFocus)
    {

        EditorGUI.PropertyField(rect, positiveModifiers.GetArrayElementAtIndex(index));
    }
    void ElementDrawerN(Rect rect, int index, bool isActive, bool isFocus)
    {

        EditorGUI.PropertyField(rect, negativeModifiers.GetArrayElementAtIndex(index));
    }
    void Add(ReorderableList rlist)
    {
        positiveModifiers.arraySize++;
        numbersPerModifer.InsertArrayElementAtIndex(numbersPerModifer.arraySize);
        EditorUtility.SetDirty(managerTarget);
        serializedObject.ApplyModifiedProperties();
    }
    void AddN(ReorderableList rlist)
    {
        negativeModifiers.arraySize++;
        numbersPerModifer.InsertArrayElementAtIndex(numbersPerModifer.arraySize);
        EditorUtility.SetDirty(managerTarget);
        serializedObject.ApplyModifiedProperties();
    }
    void Remove(ReorderableList rlist)
    {
        positiveModifiers.DeleteArrayElementAtIndex(rlist.index);
        if (numbersPerModifer.arraySize > positiveModifiers.arraySize)
            numbersPerModifer.DeleteArrayElementAtIndex(rlist.index);
    }
    void RemoveN(ReorderableList rlist)
    {
        negativeModifiers.DeleteArrayElementAtIndex(rlist.index);
        if (numbersPerModifer.arraySize > negativeModifiers.arraySize)
            numbersPerModifer.DeleteArrayElementAtIndex(rlist.index);
    }
    float ElementHeigh(int inxed)
    {
        float line = EditorGUIUtility.currentViewWidth > 332 ? 1 : 2;
        float lineHeight = EditorGUIUtility.singleLineHeight + 1;
        return line * lineHeight;
    }


}
