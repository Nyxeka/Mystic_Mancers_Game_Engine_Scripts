using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(Resource))]
[CanEditMultipleObjects]
public class ResourceEditor : Editor
{

    SerializedProperty maximum;
    SerializedProperty minimum;
    SerializedProperty resourceName;
    SerializedProperty regenOn;
    SerializedProperty regen;
    SerializedProperty amount;

    SerializedProperty eventsOnEmpty;

    List<UnityEvent> eventList;

    /*
	 * So, we want a list of events, to possibly trigger when the resource goes below a certain percentage.
	 * 
	 * 
	 * 
	 * 
	 */

    void OnEnable()
    {

        maximum = serializedObject.FindProperty("maximum");
        minimum = serializedObject.FindProperty("minimum");
        resourceName = serializedObject.FindProperty("resourceName");
        regenOn = serializedObject.FindProperty("regenOn");
        regen = serializedObject.FindProperty("regen");
        amount = serializedObject.FindProperty("amount");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Resource rs = (Resource)target;

        GUILayout.Box("-------------\nThis is a Resource for your character.\nUse this for things like mana/energy/etc...\n-------------", GUILayout.ExpandWidth(true));

        Rect r = EditorGUILayout.BeginVertical();
        EditorGUI.ProgressBar(r, amount.floatValue / (maximum.floatValue - minimum.floatValue), resourceName.stringValue + ": " + amount.floatValue.ToString());
        GUILayout.Space(16);
        EditorGUILayout.EndVertical();


        resourceName.stringValue = EditorGUILayout.TextField("Resource Name:", resourceName.stringValue);
        maximum.floatValue = EditorGUILayout.FloatField("Maximum amount:", maximum.floatValue);
        minimum.floatValue = EditorGUILayout.FloatField("Minimum amount:", minimum.floatValue);

        regenOn.boolValue = EditorGUILayout.Toggle("Regenerate?", regenOn.boolValue);

        if (regenOn.boolValue)
        {

            regen.floatValue = EditorGUILayout.FloatField("Amount per second: ", regen.floatValue);

        }

        GUILayout.Box("Event list to be triggered when resource goes below value.", GUILayout.ExpandWidth(true));


        //fix later:
        //if (GUILayout.Button ("Add Event")) {

        //eventList.Add(new UnityEvent());
        //}

        //foreach (UnityEvent listedEvent in eventList){

        //EditorGUILayout.PropertyField(listedEvent);

        //}

        //--------------
        serializedObject.ApplyModifiedProperties();
    }


}
