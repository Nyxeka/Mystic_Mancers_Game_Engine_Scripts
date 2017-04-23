using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour {

    //enemy vision script
    //by Nick
    //requires a trigger collider

    [HideInInspector]
    public bool enemyInSight = false;

    [HideInInspector]
    public Transform _enemy;

    RaycastHit hit;

    public string targetTag = "Player";

    bool checking = false;

    Transform target;

    public float visionCheckIncrement = 0.2f;

    public Transform enemy
    {
        get
        {
            return _enemy;
        }
    }

    public bool doRayCastCheck = true;

	void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == targetTag)
        {
            target = other.transform;
            checking = true;
        }

    }

    void Update()
    {

        if (checking)
        {
            if (doRayCastCheck)
            {
                if (Physics.Raycast(transform.parent.position, -1 * (transform.parent.position - target.position), out hit, 50))
                {

                    //Debug.DrawLine(transform.parent.position, target.position, Color.white, 0.5f);
                    //Debug.DrawRay(transform.parent.position,-1 * (transform.parent.position - target.position), Color.white, 0.5f);
                    //Physics.Raycast()

                    if (hit.transform.tag == targetTag)
                    {
                        enemyInSight = true;
                        _enemy = target;
                    }

                }
            } else
            {
                
                enemyInSight = true;
                _enemy = target;

            }

        }

    }

    void OnTriggerExit(Collider other)
    {

        if (other.tag == targetTag)
        {

            enemyInSight = false;
            checking = false;

        }

    }
    
}
