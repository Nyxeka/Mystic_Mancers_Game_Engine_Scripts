using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimationHandler))]
[CanEditMultipleObjects]
public class AnimationHandlerEditor : Editor
{

    SerializedProperty playerObject;
    
    //SerializedProperty manuallySetFilePaths;
    SerializedProperty animControllerPath;

    SerializedProperty runningAnimSpeedMult;
    SerializedProperty climbingAnimSpeedMult;

    SerializedProperty attackingAnimSpeed;

    SerializedProperty animControllerName;

    bool manualAnimController;

    void OnEnable()
    {

        playerObject = serializedObject.FindProperty("playerObject");
        //manuallySetFilePaths = serializedObject.FindProperty("manuallySetFilePaths");
        runningAnimSpeedMult = serializedObject.FindProperty("runningAnimSpeedMult");
        climbingAnimSpeedMult = serializedObject.FindProperty("climbingAnimSpeedMult");
        attackingAnimSpeed = serializedObject.FindProperty("attackingAnimSpeed");
        animControllerName = serializedObject.FindProperty("animControllerName");
    }



    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //GUILayout.Label ("-------------\nThis is a Label in a Custom Editor\n-------------");
        GUILayout.Box("-------------\nThis is a custom editor for setting up the animations in your Game Unit.\n-------------", GUILayout.ExpandWidth(true));

        //GUILayout.Label("");
        EditorGUILayout.PropertyField(playerObject, new GUIContent("Player Model", "You'll find the player model under the player holder object in the player unit."));

        

        GUILayout.Box("Change animation speeds here", GUILayout.ExpandWidth(true));

        climbingAnimSpeedMult.floatValue = EditorGUILayout.Slider("Climb anim speed:", climbingAnimSpeedMult.floatValue, .0f, 10.0f);
        runningAnimSpeedMult.floatValue = EditorGUILayout.Slider("Run anim speed:", runningAnimSpeedMult.floatValue, .0f, 10.0f);
        attackingAnimSpeed.floatValue = EditorGUILayout.Slider("Attack anim speed:", attackingAnimSpeed.floatValue, .0f, 10.0f);

        GUILayout.Box("-------------------------"
            + "\n", GUILayout.ExpandWidth(true));

        manualAnimController = EditorGUILayout.Toggle("Custom Anim Controller", manualAnimController);

        if (manualAnimController)
        {
            GUILayout.Box("Put your custom animation controller in a Resources folder, and then input the name of the controller here.", GUILayout.ExpandWidth(true));
            animControllerName.stringValue = EditorGUILayout.TextField(animControllerName.stringValue);

        }

        GUILayout.Space(60);

        serializedObject.ApplyModifiedProperties();
    }
}