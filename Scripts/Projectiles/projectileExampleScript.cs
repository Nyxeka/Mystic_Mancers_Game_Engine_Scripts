using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class projectileExampleScript : MonoBehaviour {
	//projectile for player only.


	public float speed = 4.0f;
	public float lifespan = 4.0f;

	float curLifeSpan = 0.0f;
	Rigidbody rb;



	void Start(){

		rb = gameObject.GetComponent<Rigidbody> ();

		if (rb) {
			rb.AddRelativeForce (Vector3.right * 50.0f,ForceMode.Impulse);

		}

	}

	void OnCollisionEnter(Collision collisionList){
		//Debug.Log ("DESTROYED PROEJCTILE collide with: " + collisionList.gameObject.name);
		GameObject.Find ("CameraGuide").GetComponent<cameraController> ().CameraShake (0.3f);
	}

	// Update is called once per frame
	void FixedUpdate () {

		curLifeSpan += Time.fixedDeltaTime;
		if (curLifeSpan >= lifespan)
			GameObject.Destroy (gameObject);

		if (rb) {
			//rb.AddRelativeForce (Vector3.right * (speed - Vector3.Magnitude(rb.velocity)), ForceMode.Impulse);
		}

	}
}
