using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BatController : Projectile
{

    //by Nick

    EnemyVision vis;
    [Header("Starting frequency of pulsing before it explodes.")]
    public float pulseTime = 0.5f;

    public float endingFrequency = 0.1f;

    public float lifetime = 2.5f;

    [Header("Use this to access the Animation Controller and whatever else you like.")]
    public UnityEvent callOnAwake;

    DamageFlash flasher;

    [Header("Movespeed in meters per second")]
    public float moveSpeed = 1.0f;

    public float endingMoveSpeed = 1.0f;

    public float minDistanceToExplode = 1.0f;

    public Transform explosionProjectile;

    float curLife = 0;

    void Start()
    {

        vis = GetComponentInChildren<EnemyVision>(); //don't need to be safe, since this is a custom one-off script for a custom game-object
        flasher = GetComponent<DamageFlash>();
        StartCoroutine(BatControl());
    }

    IEnumerator BatControl()
    {
        //wait until the bat sees the player. Checks each frame until true as far as I know.
        yield return new WaitUntil(() => vis.enemyInSight);
        StartCoroutine(pulseAndThenExplode());
        callOnAwake.Invoke();

        float newMoveSpeed = moveSpeed;

        // now we wanna fly towards the player...
        if (vis.enemy != null)
        {
            // this is the transform we want to fly at.
            Transform t = vis.enemy;

            // fly at it.
            bool atTarget = false;
            while (!atTarget)
            {

                transform.LookAt(t);

                transform.position = transform.position + (transform.rotation * Vector3.forward*Time.fixedDeltaTime* newMoveSpeed);

                if (Vector3.Distance(transform.position,t.transform.position) < minDistanceToExplode)
                {
                    atTarget = true;
                    BlowUp();

                }

                newMoveSpeed = Mathf.Lerp(moveSpeed, endingMoveSpeed, (curLife / lifetime));
               
                yield return new WaitForFixedUpdate();

            }

        }
    }

    public void BlowUp()
    {
        
        if (explosionProjectile != null)
        {

            Instantiate(explosionProjectile, transform.position, transform.rotation);

        }
        // delete us after we blowd up.
        shakeOnHit();

        GameObject.Destroy(gameObject);

    }

    IEnumerator pulseAndThenExplode()
    {
        //Debug.Log("starting co=routine to pulse and then explode!");
        float frequency = pulseTime;
        

        if (frequency == 0)
        {
            // stuff just aint happenin man.
            Debug.Log("break out bc frequency = 0!");
            yield break;

        }

        while (true)
        {

            
            curLife += frequency;
            if (flasher != null)
            {
                flasher.FlashMaterial(frequency/2);
            }
            else
            {

                Debug.Log("No damage flasher found!");

            }
            //Debug.Log(frequency.ToString());
            //curlife / lifetime gets bigger the closer curLife is to lifetime, which means we get closer to the end frequency.
            frequency = Mathf.Lerp(frequency, endingFrequency, curLife / lifetime);

            if (curLife >= lifetime)
            {

                BlowUp();

            }

            
            yield return new WaitForSeconds(frequency);
            yield return null;
            curLife += Time.deltaTime;
        }


    }
}
