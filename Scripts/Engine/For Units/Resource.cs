using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Final Integrated Stuff/Resource")]
public class Resource : MonoBehaviour {

    //------------------------
    // RPG Resource Component
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    public string resourceName = "New Resource";
	[Space(20)]
	public float amount = 100.0f;
	public float minimum = 0.0f;

	public float maximum = 100.0f;

	[Space(20)]

	[Header("Regen per second")]

	public float regen = 5;

	[Header("Ignore triggers if you don't need them")]

	public bool OnEmptyTrigger = true;
	public bool SecondaryTrigger = true;

	[Space(20)]

	public float secondaryTrigger = 0.25f;

	[Header("If amount < secondaryTriggerEvent, do this:")]
	public UnityEvent secondaryTriggerEvent;

	[Header("If amount is at minimum, do this:")]
	public UnityEvent onEmptyEvent;

	[SerializeField]
	public bool regenOn;

	//to be used whenever we change the amount of health 
	private void checkInBounds(){

		if (amount > maximum)
			amount = maximum;
		if (amount < minimum)
			amount = minimum;

	}

	void Start(){

		//Debug.Log ("enableRegen: " + regenOn.ToString ());

	}

	//Check every frame is event has passed, then invoke method.
	void Update(){
		if (SecondaryTrigger) {
			if (amount <= secondaryTrigger)
				secondaryTriggerEvent.Invoke ();
		}

		if (OnEmptyTrigger) {
			if (amount <= minimum)
				onEmptyEvent.Invoke ();
		}

		//apply regen
		if (regenOn == true && amount < maximum) 
			amount += regen * Time.deltaTime;
		checkInBounds ();
	}

	public void Add(float amountToAdd){
		amount = amount + amountToAdd;
		checkInBounds ();
	}

	public void Remove(float amountToRemove){
		amount = amount - amountToRemove;
		checkInBounds ();
	}

	public void Set(float newAmount){
		amount = newAmount;
		checkInBounds ();
	}

	public void triggerOnEmptyEvent(){

		onEmptyEvent.Invoke ();

	}

    public void refresh()
    {

        amount = maximum;

    }

	public void triggerSecondaryTriggerEvent(){

		secondaryTriggerEvent.Invoke ();

	}


	public float getResourceAmount(){

		return amount;

	}

    /// <summary>
    /// Returns a value between 0 and 1, relative to the current resource amount
    /// </summary>
    /// <returns></returns>
    public float getPercentage()
    {
        return (amount / (maximum - minimum));
    }

}
