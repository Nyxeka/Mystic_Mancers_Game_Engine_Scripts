using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerColliderDamage : MonoBehaviour
{

    public int damage;
    HashSet<Collider> didDamage;

    int layerMask = 1 << 9;

    void Start()
    {

        layerMask = ~layerMask;

        // using hash-set bc its pretty fast for checking if something is in a list, even if the list is very long.
        didDamage = new HashSet<Collider>();

    }

    void OnTriggerEnter(Collider collided)
    {
        RaycastHit hit;
        Health h;
        if (h = collided.gameObject.GetComponent<Health>())
        {


            Debug.DrawRay(transform.position, -1 * (transform.position - collided.transform.position) * 10, Color.red, 1.0f);
            Ray ray = new Ray(transform.position, -1 * (transform.position - collided.transform.position));
            if (Physics.Raycast(ray, out hit, 50, layerMask, QueryTriggerInteraction.Ignore))
            {

                if (hit.transform.name == collided.transform.name && !didDamage.Contains(collided))
                {
                    h.removeHealth(damage);
                    didDamage.Add(collided);
                }
            }
            // second ray to save time so we don't have to change a bunch of the enemy prefabs.
            ray = new Ray(transform.position, -1 * (transform.position - collided.transform.position - (Vector3.down*0.2f)));
            if (Physics.Raycast(ray, out hit, 50, layerMask, QueryTriggerInteraction.Ignore))
            {

                if (hit.transform.name == collided.transform.name && !didDamage.Contains(collided))
                {
                    h.removeHealth(damage);
                    didDamage.Add(collided);
                }
            }

            /*if (Physics.SphereCast(ray,0.5f, out hit, 30.0f,layerMask, QueryTriggerInteraction.Ignore))
            {

                if (hit.transform.name == collided.transform.name && !didDamage.Contains(collided))
                {
                    h.removeHealth(damage);
                    didDamage.Add(collided);
                }
            }*/


        }

    }
}
