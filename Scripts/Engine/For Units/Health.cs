using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Final Integrated Stuff/HealthInPips")]
public class Health : MonoBehaviour {

	/*
	 * 
	 * Health Pips Scripts
	 * 
	 * Special resource that uses fixed integers as health amount.
	 * 
	 * functions needed:
	 * 
	 * Add()
	 * Remove()
	 * Set()
	 * Get()
	 * Manually trigger onEmptyEvents
	 * 
	 * SetMax()
	 * SetMin()
	 * 
	 * 
	 */

	[Header("Health Resource")]

	//Standard set for a resource
	public int numPips = 8;

	public int maxPips = 8;

	public int minPips = 0;

	public bool invokeOnEmptyEvent = true;

	public Transform healthPip;

	public UnityEvent onEmptyEvent;

    public float pipVerticalOffset = 10f;

    //distance that goes between floating health-pips.
    float distBetweenPips = 0.3f;

    //used for checking if the value changed.
    int oldPips;

    public bool toggleDisplayHealth = true;

    //floating pips
    List<Transform> fPips;

	public void Start(){
		numPips = maxPips;
        oldPips = numPips;
        fPips = new List<Transform>();
        updateFloatingPips();

    }

    void updateFloatingPips()
    {
        /*
        So, we need to check the number of pips we have
        we need to make a distance between the pips
        we need to spawn the pips as a child object
        in a for loop, increasing the increment between them each time one is spawned.
        Instead of re-making them every time, we can simply go over the list. If the     
        list is too big, then destroy an object, and remove it from the list, then set the location of the remaining health pips.

        */
        if (toggleDisplayHealth)
        {
            if (numPips > fPips.Count)
            {
                //more pips. Update.
                fPips.Add(Instantiate(healthPip, transform));
                //Debug.Log("AddedFloatingHealthPip");
                updateFloatingPips();
                return;
            }
            if (numPips < fPips.Count)
            {

                GameObject.Destroy(fPips[fPips.Count - 1].gameObject);
                fPips.RemoveAt(fPips.Count - 1);
                updateFloatingPips();
                return;

            }

            for (int i = 0; i < fPips.Count; i++)
            {
                //distbetweenPips*i-((float)fPip.Count/2)
                fPips[i].position = transform.position + new Vector3((distBetweenPips * ((float)i)) - (((float)fPips.Count / 2) * distBetweenPips) + distBetweenPips / 2, pipVerticalOffset, 0);

            }
        }


    }
    
	public void Update(){
        checkInBounds();
        
		if (numPips <= minPips && invokeOnEmptyEvent) {

			onEmptyEvent.Invoke ();

		}

        if (oldPips != numPips)
        {

            //value changed! change the list of floating health pips!
            
            updateFloatingPips();
            oldPips = numPips;
        }

	}

	public void addHealth(int healthMod){

		numPips += healthMod;
		checkInBounds ();
        //Debug.Log(numPips.ToString());
    }

    public void removeHealth(int healthMod)
    {

        numPips -= healthMod;
        checkInBounds();
        //Debug.Log(numPips.ToString());

    }

	//For use within this object
	private void checkInBounds(){

		if (numPips > maxPips)
			numPips = maxPips;

		if (numPips < minPips)
			numPips = minPips;

	}

	public void Kill(){

		numPips = minPips;

	}

}