using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Health))]
[CanEditMultipleObjects]
public class HealthEditor : Editor
{

    SerializedProperty numPips;
    SerializedProperty maxPips;
    SerializedProperty minPips;

    SerializedProperty pipVerticalOffset;

    SerializedProperty healthPip;

    SerializedProperty toggleDisplayHealth;

    string curHealthToDisplay;
    string emptyHealth;

    void updateHealthString()
    {
        curHealthToDisplay = "";
        for (int i = minPips.intValue; i < maxPips.intValue; i++)
        {

            curHealthToDisplay = curHealthToDisplay + "o";

        }
        for (int j = minPips.intValue; ((j < numPips.intValue) && j < maxPips.intValue); j++)
        {

            //curHealthToDisplay = curHealthToDisplay.Remove(j+1);
            curHealthToDisplay = curHealthToDisplay.Remove(j, 1);
            curHealthToDisplay = curHealthToDisplay.Insert(j, "♥");
        }

    }

    void OnEnable()
    {

        numPips = serializedObject.FindProperty("numPips");
        maxPips = serializedObject.FindProperty("maxPips");
        minPips = serializedObject.FindProperty("minPips");

        healthPip = serializedObject.FindProperty("healthPip");

        pipVerticalOffset = serializedObject.FindProperty("pipVerticalOffset");

        toggleDisplayHealth = serializedObject.FindProperty("toggleDisplayHealth");

        //Debug.Log(numPips.intValue);

        updateHealthString();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //GUILayout.Box("Test",GUILayout.ExpandWidth(true));

        GUILayout.Box("-------------\nThis is Health Component\n-------------", GUILayout.ExpandWidth(true));

        minPips.intValue = EditorGUILayout.IntField("Minimum Health:", minPips.intValue);
        maxPips.intValue = EditorGUILayout.IntField("Maximum Health:", maxPips.intValue);
        numPips.intValue = EditorGUILayout.IntSlider(numPips.intValue, minPips.intValue, maxPips.intValue);

        updateHealthString();
        GUILayout.Box("Current Health\n" + curHealthToDisplay, GUILayout.ExpandWidth(true));

        EditorGUILayout.PropertyField(healthPip);

        pipVerticalOffset.floatValue = EditorGUILayout.FloatField("Pip y offset", pipVerticalOffset.floatValue);

        toggleDisplayHealth.boolValue = EditorGUILayout.Toggle("Pips inworld?", toggleDisplayHealth.boolValue);

        GUILayout.Label(new GUIContent("Events on death?", "Choose what happens on death. Recommended you target the GameUnit component in this game object and select \"kill()\""));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("onEmptyEvent"), true);

        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

        if (GUILayout.Button("Kill Unit"))
        {

            numPips.intValue = minPips.intValue;

        }

        serializedObject.ApplyModifiedProperties();
    }


}

