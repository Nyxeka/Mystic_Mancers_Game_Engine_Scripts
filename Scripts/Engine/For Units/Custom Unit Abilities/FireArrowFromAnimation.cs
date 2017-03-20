using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArrowFromAnimation : MonoBehaviour {

	public Transform arrowProjectile;

	public float timeToShoot = 0.5f;

	public float delayRefreshArrow = 1.0f;

	public string animationName;

	public Animation anim;

	Vector3 arrowVel;

	bool fireArrow;

	Renderer arrowMesh;

	// Use this for initialization
	IEnumerator TrackDrawnArrowVelocity(){
		Vector3 oldPos = new Vector3 ();
		Vector3 newPos = new Vector3 ();

		newPos = transform.position;
		oldPos = transform.position;

		//co-routines are useful because you can start them, and then leave them to run.
		//a co-routine will run whatever is in a loop, then stop processing and wait whatever you specify to continue the loop again.
		while (true) {
			newPos = transform.position;
			arrowVel = (newPos - oldPos) / Time.fixedDeltaTime;
			oldPos = newPos;

			//tell the co-routine to stop and wait for the next fixedUpdated to finish
			yield return new WaitForFixedUpdate();
		}

	}

	IEnumerator EquipNewArrow(){

		yield return new WaitForSeconds (delayRefreshArrow);

		arrowMesh.enabled = true;

	}

	void Start(){
		arrowVel = new Vector3 ();
		StartCoroutine (TrackDrawnArrowVelocity ());
		arrowMesh = gameObject.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (anim [animationName].time == timeToShoot) {
			fire ();

		}
	}

	void fire(){
		GameObject newArrow = Instantiate (arrowProjectile.gameObject, transform.position, transform.rotation);
		arrowMesh.enabled = false;
		Rigidbody newArrowRB = newArrow.GetComponent<Rigidbody> ();

		if (newArrowRB) {

			newArrowRB.velocity = arrowVel;

		}


	}
}
