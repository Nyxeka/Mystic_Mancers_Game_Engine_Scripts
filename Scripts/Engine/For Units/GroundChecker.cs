using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour {

    //------------------------
    // Ground Checker
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    // requires a trigger collider where you want to look for the ground.

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
