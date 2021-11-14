using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(PlayerMovement))]
public class PlayerMovementEditor : Editor
{
    private PlayerMovement playerMovement;
    private bool clampBool;
    private void OnEnable()
    {
        playerMovement = target as PlayerMovement;
    }
    public override void OnInspectorGUI()
    {
        playerMovement.inputManager = (InputManager) EditorGUILayout.ObjectField("Input Manager", playerMovement.inputManager, typeof(InputManager), true);
        playerMovement.vrHeadSett = (Transform) EditorGUILayout.ObjectField("Vr Head Sett", playerMovement.vrHeadSett, typeof(Transform), true);
        playerMovement.tableRenderer = (Renderer) EditorGUILayout.ObjectField("Table Renderer", playerMovement.tableRenderer, typeof(Renderer), true);
        playerMovement.tableTransform = (Transform) EditorGUILayout.ObjectField("Table Transform", playerMovement.tableTransform, typeof(Transform), true);
        playerMovement.tempLeftHand = (Transform) EditorGUILayout.ObjectField("Left Hand Transform", playerMovement.tempLeftHand, typeof(Transform), true);
        playerMovement.tempRighttHand = (Transform) EditorGUILayout.ObjectField("Right Hand Transform", playerMovement.tempRighttHand, typeof(Transform), true);

        //foldout
        clampBool = EditorGUILayout.Foldout( clampBool, "Clamp transforms");
        if (clampBool)
        {
            playerMovement.zClampMin = (Transform)EditorGUILayout.ObjectField("Forward clamp min", playerMovement.zClampMin, typeof(Transform), true);
            playerMovement.zClampMax = (Transform)EditorGUILayout.ObjectField("Forward clamp max", playerMovement.zClampMax, typeof(Transform), true);
            playerMovement.upClamp = (Transform)EditorGUILayout.ObjectField("Up clamp", playerMovement.upClamp, typeof(Transform), true);
            playerMovement.downClamp = (Transform)EditorGUILayout.ObjectField("Down clamp", playerMovement.downClamp, typeof(Transform), true);
        }
       

    }

}
