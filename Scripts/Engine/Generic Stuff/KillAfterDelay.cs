using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAfterDelay : MonoBehaviour {
    public float delay = 1.0f;

	// Use this for initialization
	void Start () {
        StartCoroutine(kms());
	}
    //we want a delay, so we're just gonna use this
    IEnumerator kms(){
        //wait for delay in seconds:
        yield return new WaitForSeconds(delay);
        //kill this game object.
        GameObject.Destroy(gameObject);

    }
}
