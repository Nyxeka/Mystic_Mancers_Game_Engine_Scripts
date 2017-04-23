using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour {

    //------------------------
    // Health UI Handler
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    List<Transform> healthPips;

    Health playerHealth;

    int maxPips;

    int curPips;

    int oldPipCount;

    bool initialized = false;

    // Use this for initialization
    void Start () {
        StartCoroutine(initHealthUI());
        healthPips = new List<Transform>();
	}
	
    IEnumerator initHealthUI()
    {
        yield return new WaitForSeconds(2);// this is pretty much what everything else is waiting for.

        GameObject PGO;

        if (PGO = GameObject.FindGameObjectWithTag("Player")){

            if (playerHealth = PGO.GetComponent<Health>())
            {
                maxPips = playerHealth.maxPips;
                


                //need to run through the child objects.

                for (int i = 1; i <= transform.childCount; i++)
                {
                    Transform temp;
                    if (temp = transform.Find(i.ToString()))
                    {

                        healthPips.Add(temp);
                        if (i > maxPips)
                        {
                            
                            healthPips[i-1].Find("Back").gameObject.SetActive(false);
                            healthPips[i-1].Find("Frame").gameObject.SetActive(false);

                        } else
                        {

                            healthPips[i-1].Find("Juice").gameObject.SetActive(true);

                        }
                    }

                }


                Debug.Log("Initialized Health UI Component");
                initialized = true;

            } else
            {
                Debug.Log("player game object doesn't have a health component!");
            }
        } else
        {

            Debug.Log("Player Game Object doesn't exist!");

        }
    }


    void updatePips()
    {

        if (initialized)
        {

            for(int i = 0; i < healthPips.Count; i++)
            {

                if (i < curPips)
                {

                    healthPips[i].Find("Juice").gameObject.SetActive(true);

                } else
                {

                    healthPips[i].Find("Juice").gameObject.SetActive(false);

                }

            }

        }

    }

    void Update()
    {
        if (initialized)
        {

            curPips = playerHealth.numPips;

            if (oldPipCount != curPips)
            {

                // health has changed, we gotta update!
                updatePips();
            }


            oldPipCount = curPips;


        }
    }

}
