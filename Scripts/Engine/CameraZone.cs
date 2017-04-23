using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour {

    [SerializeField]
    bool changePerm;
    
    cameraController camControl;

    GameObject camGO;

    [SerializeField]
    Vector3 cameraViewOffset;

    [SerializeField]
    float newCameraZoom = 10.0f;

    //public float newCamWidth = 1;

    void Start()
    {
        

        camGO = GameObject.FindGameObjectWithTag("MainCamera");

        if (camGO != null)
        {

            camControl = camGO.GetComponent<cameraController>();
            if (camControl == null)
            {

                camControl = camGO.GetComponentInParent<cameraController>();

            }

        }
        if (GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().enabled = false;
        }

    }

    void OnTriggerStay()
    {



    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            camControl.ReleaseControl();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            
            camControl.SetTargetLocation(transform.position + cameraViewOffset, newCameraZoom);

        }

    }

}
