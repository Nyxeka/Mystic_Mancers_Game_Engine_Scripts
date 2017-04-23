using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    //------------------------
    // Moving platform script
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    //requires a trigger collider & player tag


    public List<Transform> targets;
    
    int curTarget = 0;

    public float minDistToTarget = 0.1f;

    [Header("Meters per second")]
    public float speed = 1.0f;

    public float waitDestinationTime = 1.0f;

    public bool lerp = false;
    public float lerpDelay = 0.5f;
    
    //this is dependant upon how much we moved in the last frame.
    Vector3 lastPos;

    Vector3 offset;

    Vector3 destination;

    public bool waitForPlayer = false;

    bool playerOn = false;

    int lastTarget = 0;

    bool goingBack = false;

    bool hitFirstTarget = false;

    [Header("Turn this off if you're using it with an enemy.")]
    public bool movePlayer = true;

    void Start()
    {
        lastPos = new Vector3();
        offset = new Vector3();

        destination = targets[curTarget].position;

        StartCoroutine(moveToTarget());

    }

    void getNextTarget(bool goBack = false)
    {
        if (goBack)
        {
            curTarget = lastTarget;
            destination = targets[curTarget].position;
            StartCoroutine(moveToTarget());
        }
        else {
            lastTarget = curTarget;
            curTarget = (curTarget + 1) % targets.Count;
            destination = targets[curTarget].position;
            StartCoroutine(moveToTarget());
        }

    }

    IEnumerator moveToTarget()
    {
        yield return null; // wait 1 frame, just in case something weird happens with this being triggered in an infinite loop.
        
        bool arrived = false;
        
        // for calculating the percentage of the journey completed.
        float distAtStart = Vector3.Distance(transform.position, destination);
        float percentageToTarget = .0f;

        lastPos = transform.position;

        if (!goingBack)
        {
            // wait for the player to hop on!
            if (waitForPlayer)
                yield return new WaitUntil(() => playerOn);
        } 
        


        //Debug.Log("Starting Journey." + curTarget.ToString());
        while (!arrived)
        {
            if (lerp)
            {
                // move lerpy 
                transform.position = Vector3.Lerp(transform.position, destination, lerpDelay * Time.fixedDeltaTime);
                

            } else
            {
                // move normal
                transform.position = transform.position + (Vector3.Normalize(destination - transform.position) * speed * Time.fixedDeltaTime);

            }
            
            // check for arrival
            if (Vector3.Distance(transform.position, destination) < minDistToTarget)
            {

                arrived = true;

                playerOn = false;

                goingBack = false;

                if (!hitFirstTarget)
                {
                    playerOn = true;
                    hitFirstTarget = true;
                }
                
            }
            // find out how much we moved.
            offset = transform.position - lastPos;

            // so we can find out how much we moved next loop
            lastPos = transform.position;

            //don't leave the palyer behind.
            if (!goingBack)
            {
                if (waitForPlayer)
                {
                    percentageToTarget = (1.0f - (Vector3.Distance(transform.position, destination) / distAtStart));
                    // so, if the player jumps off the platform at the start of its journey, we dont want the platform to abandon the player.
                    if (!playerOn)
                    {
                        if (percentageToTarget < 0.2f)
                        {
                            //Debug.Log("Resetting, player left too early.");
                            hitFirstTarget = true;
                            goingBack = true;
                            break;
                        }
                    }
                }
            }

            yield return new WaitForFixedUpdate();

        }
        // reset offset when not in motion
        offset = Vector3.zero;

        if (!waitForPlayer)
        {
            
            yield return new WaitForSeconds(waitDestinationTime);
            getNextTarget();

        } else if (!playerOn && goingBack)
        {
            getNextTarget(true);
        } else
        {
            getNextTarget();
        }

        

        
    }

    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player" && movePlayer)
        {
            other.transform.position = other.transform.position + offset;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("player hit platform.");
            playerOn = true;
            
        }

    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player"){
            //Debug.Log("player left platform.");
            playerOn = false;

        }

    }

}
