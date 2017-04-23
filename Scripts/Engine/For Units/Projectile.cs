using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {


    cameraController cam;

    bool checkedForCam = false;

    public float cameraShakeAmount = 0.1f;

    protected void shakeOnHit()
    {

        if (!checkedForCam)
        {
            checkedForCam = true;
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cameraController>();

        }

        if (cam != null)
        {

            cam.CameraShake(cameraShakeAmount);

        }

    }
}
