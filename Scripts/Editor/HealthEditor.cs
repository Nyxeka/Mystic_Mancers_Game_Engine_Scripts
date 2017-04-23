using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Health))]
[CanEditMultipleObjects]
public class HealthEditor : Editor
{

    //------------------------
    // Health Pip Component Editor Code
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    SerializedProperty numPips;
    SerializedProperty maxPips;
    SerializedProperty minPips;

    SerializedProperty pipVerticalOffset;

    SerializedProperty healthPip;

    SerializedProperty toggleDisplayHealth;

    SerializedProperty immuneWhileCooldown;

    SerializedProperty takeDamageCooldown;

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

        immuneWhileCooldown = serializedObject.FindProperty("immuneWhileCooldown");

        takeDamageCooldown = serializedObject.FindProperty("takeDamageCooldown");

        //Debug.Log(numPips.intValue);

        updateHealthString();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //GUILayout.Box("Test",GUILayout.ExpandWidth(true));

        GUILayout.Box("-------------\nHealth Component\n-------------", GUILayout.ExpandWidth(true));

        minPips.intValue = EditorGUILayout.IntField("Minimum Health:", minPips.intValue);
        maxPips.intValue = EditorGUILayout.IntField("Maximum Health:", maxPips.intValue);
        numPips.intValue = EditorGUILayout.IntSlider(numPips.intValue, minPips.intValue, maxPips.intValue);

        updateHealthString();
        GUILayout.Box("Current Health\n" + curHealthToDisplay, GUILayout.ExpandWidth(true));

        EditorGUILayout.PropertyField(healthPip);

        pipVerticalOffset.floatValue = EditorGUILayout.FloatField("Pip y offset", pipVerticalOffset.floatValue);

        toggleDisplayHealth.boolValue = EditorGUILayout.Toggle("Pips inworld?", toggleDisplayHealth.boolValue);

        GUILayout.Space(20);
        GUILayout.Label(new GUIContent("Immune to damage for delay after taking damage?"));
        immuneWhileCooldown.boolValue = EditorGUILayout.Toggle("immune cooldown?", immuneWhileCooldown.boolValue);

        takeDamageCooldown.floatValue = EditorGUILayout.FloatField("remove health cooldown.", takeDamageCooldown.floatValue);

        GUILayout.Label(new GUIContent("Events on take damage?", "Set something to happen when this unit takes damage. Make it flash white, do a hit-reaction, etc..."));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("takeDamageEvent"), true);

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

