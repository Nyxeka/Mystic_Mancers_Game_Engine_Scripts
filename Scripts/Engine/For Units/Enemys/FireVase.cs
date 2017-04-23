using System.Collections;
using UnityEngine;

public class FireVase : MonoBehaviour {

    // serialized because it's private (doesn't need to be accessed by other objects, but we want the user to be able to change it
    [SerializeField]
    float shootFrequency = 2;

    [SerializeField]
    float fireBallHeight = 4;

    [SerializeField]
    float fireBallLifeTime = 2;

    [SerializeField]
    float startDelay = 0;

    public Transform fireBallObject;
    
    void Start()
    {

        if (fireBallObject != null)
        {
            //not null
            StartCoroutine(ShootFireballs());

        } else
        {
            //null
            Debug.Log("Holy shit, you didn't put a fireball object prefab in here. Please check fire vases.");

        }

    }

    IEnumerator ShootFireballs()
    {
        if (startDelay > 0)
        {
            yield return new WaitForSeconds(startDelay);
        }

        if (shootFrequency <= 0)
        {

            Debug.Log("Warning: Frequency for vase shooting fireball MUST be greater than 0");

        } else {

            while (true)
            {

                Instantiate(fireBallObject, transform, false);

                yield return new WaitForSeconds(shootFrequency);

            }
        }
    }

    public float getFireballHeight()
    {
        // for spawned fireball children, so they can ask us how hiehg we want them to go.

        return fireBallHeight;

    }

    public float getLifeTime()
    {

        return fireBallLifeTime;

    }


}
