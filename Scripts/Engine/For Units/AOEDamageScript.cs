using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEDamageScript : MonoBehaviour {

    public int damage;

    public float explosionForce = 100;

    void OnTriggerEnter(Collider collided)
    {

        Health h;
        Rigidbody rb;
        if (h = collided.gameObject.GetComponent<Health>())
        {

            h.removeHealth(damage);

        }
        if (explosionForce > 0)
        {
            if (rb = collided.gameObject.GetComponent<Rigidbody>())
            {
                rb.AddExplosionForce(explosionForce, transform.position, gameObject.GetComponent<SphereCollider>().radius);
            }

        }

    }
}
