using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour {

	private PlayerController myCon;

	void OnTriggerStay(){

		myCon.grounded = true;

	}

	void OnTriggerExit(){

		myCon.grounded = false;

	}

	// Use this for initialization
	void Start () {
		myCon = GetComponentInParent<PlayerController> ();
	}
}
