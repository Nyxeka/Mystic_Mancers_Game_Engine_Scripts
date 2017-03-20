using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DebugGUIHandler : MonoBehaviour {

	Text myEdit;

	// Use this for initialization
	void Start () {
		myEdit = gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (myEdit) {
			myEdit.text = Input.inputString;
		}
	}
}
