using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldSetup : MonoBehaviour {

    /*
	 * 
	 * This script is used to do things like change the gravity for rigidbodies, etc...
	 * 
	 */

     //static because this is not supposed to ever change.
    static float grav = -9.8f;

    public float gravMult = 3.0f;

	bool paused = false;

	float defaultTimeScale = 1;

	public UnityEvent pauseStart;
	public UnityEvent pauseEnd;

	// Use this for initialization
	void Start () {
        Vector3 newGrav = new Vector3(.0f, grav*gravMult, .0f);
        Physics.gravity = newGrav;
	}

	void Update(){

		if (Input.GetButtonDown ("Escape") && !paused) {
			
			Time.timeScale = 0;
			//StartCoroutine (waitForUnpause ());
			paused = true;
			pauseStart.Invoke ();
			Debug.Log ("Pausing Game");
			
		} else if (Input.GetButtonDown ("Escape") && paused) {

			Time.timeScale = defaultTimeScale;
			paused = false;
			pauseEnd.Invoke ();
			Debug.Log ("Unpausing Game");
		}
	}
}
