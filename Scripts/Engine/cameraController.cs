using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour {

    //------------------------
    // 2.5D Camera Guide Script
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    private Transform followTarget;

	public bool smoothFollow;

	public float smoothFollowSpeed = 0.5f;
    float oldLerpDelay;

    float zoomLerpDelay = 2.0f;

	public Vector3 offset;

	public float zoom = 9.13f;

    float oldZoom;

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

    // for private use, but will be set when something else wants control of the camera.
    bool externalControl = false;

    float camWidthMult = 1;

    float currentCamWidthMult = 1;
    
	// Use this for initialization
	void Start () {
        //how to delay?
        StartCoroutine("init");
        

	}

    IEnumerator init()
    {

        yield return new WaitForSeconds(1.0f);
        cam = gameObject.GetComponentInChildren<Camera>();
        newY = offset.y;
        followTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        targRB = followTarget.GetComponent<Rigidbody>();



        if (targRB)
        {

            targRBOn = true && cameraMoveAhead;

        }

        StartCoroutine("startFixedUpdateDelay");
    }

	public void CameraShake(float intensity){

		shake = intensity;

	}
	
    public void SetTargetLocation(Vector3 newTarget, float newZoom = 9.13f)
    {

        TargetLocation = new Vector3(newTarget.x, newTarget.y, offset.z);
        oldZoom = zoom;
        zoom = newZoom;
        oldLerpDelay = smoothFollowSpeed;
        smoothFollowSpeed = 0.5f;

        externalControl = true;

    }

    /// <summary>
    /// Deprecated.
    /// </summary>
    /// <param name="newTarget"></param>
    /// <param name="newWidth"></param>
    /// <param name="newZoom"></param>
    public void SetTargetLocation(Vector3 newTarget,float newWidth, float newZoom = 9.13f)
    {

        TargetLocation = new Vector3(newTarget.x, newTarget.y, offset.z);
        oldZoom = zoom;
        zoom = newZoom;
        oldLerpDelay = smoothFollowSpeed;
        smoothFollowSpeed = 0.5f;
        camWidthMult = newWidth;
        externalControl = true;

    }

    public void setCamRect(Rect newView)
    {

        cam.rect = newView;

    }

    public void ReleaseControl()
    {

        externalControl = false;
        smoothFollowSpeed = oldLerpDelay;
        zoom = oldZoom;
        camWidthMult = 1;

    }

    IEnumerator startFixedUpdateDelay()
    {
        //float camHeight;
        //float camY;

        while (true)
        {
            // mousewheel zooming:
            // disabling this for now.
            // zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSensMult;
            // keep a hold on it
            if (!externalControl)
            {
                if (zoom > maxDistance)
                    zoom = maxDistance;
                if (zoom < minDistance)
                    zoom = minDistance;
            }

            //Rect.

            //now set it
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, zoomLerpDelay * Time.fixedDeltaTime);

            //currentCamWidthMult = Mathf.Lerp(currentCamWidthMult, camWidthMult, zoomLerpDelay * Time.fixedDeltaTime);

            //camHeight = 1 / currentCamWidthMult;
            //camY = (1 - camHeight) / 2;

            //cam.rect = new Rect(cam.rect.x, camY, cam.rect.width, camHeight);
            
            //set the initial y offset while zooming to maintain vision of the player.
            newY = offset.y + (zoom / 2);
            if (followTarget)
            {
                if (!externalControl)
                {
                    TargetLocation = followTarget.position + new Vector3(offset.x, newY, offset.z);
                }

                if (shake > .01f)
                {
                    cam.transform.localPosition = Random.insideUnitSphere * shake;
                    shake = Mathf.Lerp(shake, .0f, shakeStopSpeed * Time.fixedDeltaTime);
                }

                if (smoothFollow)
                {

                    if (targRBOn)
                    {
                        this.transform.position = Vector3.Lerp(this.transform.position, TargetLocation + targRB.velocity * cameraAheadMult, (smoothFollowSpeed * Time.deltaTime));
                    }
                    else {

                        this.transform.position = Vector3.Lerp(this.transform.position, TargetLocation, smoothFollowSpeed * Time.deltaTime);
                    }

                }
                else {
                    transform.position = TargetLocation;
                }
            }

            yield return new WaitForFixedUpdate();
        }

    }
}
