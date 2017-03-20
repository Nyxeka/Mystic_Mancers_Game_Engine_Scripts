using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

	public Transform followTarget;

	public bool smoothFollow;

	public float smoothFollowSpeed = 0.5f;

	public Vector3 offset;

	public float zoom = 9.13f;

	public float zoomSensMult = 3.0f; 

	public float minDistance = 5.0f;
	public float maxDistance = 20.0f;

	private Camera cam;

	private Vector3 TargetLocation;

	private float shake = 0.0f;
	private float shakeStopSpeed = 3.0f;

    public bool cameraMoveAhead = true;
    [Header("Recommend you set this to be very small(0.0-0.5)")]
    public float cameraAheadMult = 0.1f;

    Rigidbody targRB;
    
    bool targRBOn = false;

	float newY = 0;

	// Use this for initialization
	void Start () {
		cam = gameObject.GetComponentInChildren<Camera> ();
		newY = offset.y;

        targRB = followTarget.GetComponent<Rigidbody>();
        if (targRB)
        {

            targRBOn = true && cameraMoveAhead;

        }
	}

	public void CameraShake(float intensity){

		shake = intensity;

	}

	void Update(){

		if (Input.GetMouseButtonDown(4)){
			CameraShake(0.5f);
		}

	}
	
	// Update is called once per frame
	//Everything for camera is done in here, since all that matters is stuff that happens by the frame
	void FixedUpdate () {

		zoom += Input.GetAxis ("Mouse ScrollWheel") * zoomSensMult;

		if (zoom > maxDistance)
			zoom = maxDistance;
		if (zoom < minDistance)
			zoom = minDistance;

		cam.orthographicSize = zoom;

		newY = offset.y + (zoom / 2);
		if (followTarget) {
			
			TargetLocation = followTarget.position + new Vector3(offset.x,newY,offset.z);
            
			if (shake > .01f) {
				cam.transform.localPosition = Random.insideUnitSphere * shake;
				shake = Mathf.Lerp (shake, .0f, shakeStopSpeed * Time.fixedDeltaTime);
			}
			
			if (smoothFollow) {

                if (targRBOn)
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, TargetLocation + targRB.velocity*cameraAheadMult, (smoothFollowSpeed * Time.deltaTime));
                }
                else {

                    this.transform.position = Vector3.Lerp(this.transform.position, TargetLocation, smoothFollowSpeed * Time.deltaTime);
                }

			} else {
				transform.position = TargetLocation;
			}
		}

		//shake effect:


	}
}
