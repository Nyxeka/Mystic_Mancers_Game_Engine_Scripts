using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StunnerController : MonoBehaviour {

    EnemyVision vis;

    public float attackDelay = 1.0f;

    public UnityEvent eventOnActivate;

    public Transform projectile;

    public Vector3 projectileOffset;
    
	// Use this for initialization
	void Start () {
        vis = GetComponentInChildren<EnemyVision>();
        StartCoroutine(checkForEnemy());
	}


    IEnumerator checkForEnemy()
    {

        while (true)
        {
            if (vis != null)
            if (vis.enemyInSight)
            {

                eventOnActivate.Invoke();
                if (projectile != null)
                {

                    Instantiate(projectile, transform.position + projectileOffset, transform.rotation);

                }
                yield return new WaitForSeconds(attackDelay);

            }

            yield return new WaitForFixedUpdate();

        }

    }
}
