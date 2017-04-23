using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {


    float maxHeight;

    float lifeTime;

    float currentLifeTime;

    FireVase parentVase;

    void Start()
    {

        if (parentVase = GetComponentInParent<FireVase>())
        {

            maxHeight = parentVase.getFireballHeight();
            lifeTime = parentVase.getLifeTime();

        } else
        {

            Debug.Log("This is meant to be used with the fire vase script!");
            GameObject.Destroy(gameObject);
        }

        

        
    }

    void FixedUpdate()
    {
        currentLifeTime += Time.fixedDeltaTime;

        float currentPercentage;
        currentPercentage = currentLifeTime / (lifeTime / 2);
        // so, we want to move the fireball up, and then back down.

        //over the course of the lifetime, it will reach maxheight, and then go back to 0.
        if (currentLifeTime < (lifeTime/2))
        {
            
            // Vector3.up is 0,1,0, multiplying it by maxheight makes it (0, 1 * maxheight, 0)
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.up*maxHeight, currentPercentage/8);

        } else
        {
            currentPercentage = currentPercentage - 1;
            // Vector3.up is 0,1,0, multiplying it by maxheight makes it (0, 1 * maxheight, 0)
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, currentPercentage/8);

        }

        if (currentLifeTime > lifeTime)
        {

            GameObject.Destroy(gameObject);

        }
        
    }

}
