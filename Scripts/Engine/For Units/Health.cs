using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Final Integrated Stuff/HealthInPips")]
public class Health : MonoBehaviour {

    //------------------------
    // Health Pip Component
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    [Header("Health Resource")]

	//Standard set for a resource
	public int numPips = 8;

	public int maxPips = 8;

	public int minPips = 0;

	public bool invokeOnEmptyEvent = true;

	public Transform healthPip;

	public UnityEvent onEmptyEvent;

    public UnityEvent takeDamageEvent;

    public float takeDamageCooldown = 0.0f;

    public bool immuneWhileCooldown = false;

    public float pipVerticalOffset = 10f;

    //distance that goes between floating health-pips.
    float distBetweenPips = 0.3f;

    //used for checking if the value changed.
    int oldPips;

    public bool toggleDisplayHealth = true;

    //floating pips
    List<Transform> fPips;

    public bool dead = false;
    bool immune = false;

	public void Start(){
		numPips = maxPips;
        oldPips = numPips;
        fPips = new List<Transform>();
        

        if (healthPip == null)
        {

            healthPip = Resources.Load<Transform>("HealthPipPrefab");

        }
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

    IEnumerator CooldownTakeDamage()
    {
        immune = true;
        yield return new WaitForSeconds(takeDamageCooldown);
        immune = false;
    }
    
	public void Update(){
        checkInBounds();
        
		if (numPips <= minPips && invokeOnEmptyEvent) {
            if (dead == false)
            {
                onEmptyEvent.Invoke();
                dead = true;
            }

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
        if (!immune)
        {
            numPips -= healthMod;
            checkInBounds();
            takeDamageEvent.Invoke();
            StartCoroutine("CooldownTakeDamage");
            //Debug.Log(numPips.ToString());
        }
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