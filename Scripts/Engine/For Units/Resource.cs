using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

[AddComponentMenu("Final Integrated Stuff/Resource")]
public class Resource : MonoBehaviour {

	//used for things like health and mana



	public string resourceName = "New Resource";
	[Space(20)]
	public float amount = 100.0f;
	public float minimum = 0.0f;

	public float maximum = 100.0f;

	[Space(20)]

	[Header("Regen per second")]

	public float regen = 5;

	[Header("Ignore triggers if you don't need them")]

	public bool OnEmptyTrigger = true;
	public bool SecondaryTrigger = true;

	[Space(20)]

	public float secondaryTrigger = 0.25f;

	[Header("If amount < secondaryTriggerEvent, do this:")]
	public UnityEvent secondaryTriggerEvent;

	[Header("If amount is at minimum, do this:")]
	public UnityEvent onEmptyEvent;

	[SerializeField]
	public bool regenOn;

	//to be used whenever we change the amount of health 
	private void checkInBounds(){

		if (amount > maximum)
			amount = maximum;
		if (amount < minimum)
			amount = minimum;

	}

	void Start(){

		//Debug.Log ("enableRegen: " + regenOn.ToString ());

	}

	//Check every frame is event has passed, then invoke method.
	void Update(){
		if (SecondaryTrigger) {
			if (amount <= secondaryTrigger)
				secondaryTriggerEvent.Invoke ();
		}

		if (OnEmptyTrigger) {
			if (amount <= minimum)
				onEmptyEvent.Invoke ();
		}

		//apply regen
		if (regenOn == true && amount < maximum) 
			amount += regen * Time.deltaTime;
		checkInBounds ();
	}

	public void Add(float amountToAdd){
		amount = amount + amountToAdd;
		checkInBounds ();
	}

	public void Remove(float amountToRemove){
		amount = amount - amountToRemove;
		checkInBounds ();
	}

	public void Set(float newAmount){
		amount = newAmount;
		checkInBounds ();
	}

	public void triggerOnEmptyEvent(){

		onEmptyEvent.Invoke ();

	}

	public void triggerSecondaryTriggerEvent(){

		secondaryTriggerEvent.Invoke ();

	}


	public float getResourceAmount(){

		return amount;

	}

    /// <summary>
    /// Returns a value between 0 and 1, relative to the current resource amount
    /// </summary>
    /// <returns></returns>
    public float getPercentage()
    {
        return (amount / (maximum - minimum));
    }

}

[CustomEditor (typeof(Resource))]
[CanEditMultipleObjects]
public class ResourceEditor : Editor {

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

	void OnEnable(){

		maximum = serializedObject.FindProperty ("maximum");
		minimum = serializedObject.FindProperty ("minimum");
		resourceName = serializedObject.FindProperty ("resourceName");
		regenOn = serializedObject.FindProperty ("regenOn");
		regen = serializedObject.FindProperty ("regen");
		amount = serializedObject.FindProperty ("amount");

	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update ();

		//Resource rs = (Resource)target;

		GUILayout.Box("-------------\nThis is a Resource for your character.\nUse this for things like mana/energy/etc...\n-------------",GUILayout.ExpandWidth(true));

		Rect r = EditorGUILayout.BeginVertical ();
		EditorGUI.ProgressBar (r, amount.floatValue / (maximum.floatValue - minimum.floatValue), resourceName.stringValue + ": " + amount.floatValue.ToString());
		GUILayout.Space (16);
		EditorGUILayout.EndVertical();


		resourceName.stringValue = EditorGUILayout.TextField ( "Resource Name:", resourceName.stringValue);
		maximum.floatValue = EditorGUILayout.FloatField ("Maximum amount:", maximum.floatValue);
		minimum.floatValue = EditorGUILayout.FloatField ("Minimum amount:" ,minimum.floatValue);

		regenOn.boolValue = EditorGUILayout.Toggle ("Regenerate?", regenOn.boolValue);

		if (regenOn.boolValue) {

			regen.floatValue = EditorGUILayout.FloatField ("Amount per second: ", regen.floatValue);

		}

		GUILayout.Box("Event list to be triggered when resource goes below value.",GUILayout.ExpandWidth(true));


		//fix later:
		//if (GUILayout.Button ("Add Event")) {

			//eventList.Add(new UnityEvent());
		//}

		//foreach (UnityEvent listedEvent in eventList){

			//EditorGUILayout.PropertyField(listedEvent);

		//}

		//--------------
		serializedObject.ApplyModifiedProperties ();
	}


}
