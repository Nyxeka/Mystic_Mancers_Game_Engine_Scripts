using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {

	/*
	 * 
	 * Unit Controller Script
	 * 
	 * This is a Parent script. Do not put this in anything, though feel free to inherit this if a custom controller script.
	 * 
	 * In here we're going to provide:
	 * 
	 * functions for generic unit controls. 
	 * 
	 * Some sort of method for executing abilities and movement.
	 * 
	 * Unit will decide what to do itself when the controller tells it what to do
	 * controller will simply be giving it generic commands every update, such as "move left" or "move right" or "execute ability 1"
	 * 
	 * AI controller will be a fair bit more complicated, but it will also only be using these. 
	 */

	[HideInInspector]
	public GameUnit _unit;

	void Start(){

		//Find the unit script in our game object
		_unit = gameObject.GetComponent<GameUnit> ();

	}

	public GameUnit getParentUnit(){

		if (this.GetComponentInParent<GameUnit> () != null) {

			return gameObject.GetComponent<GameUnit> ();

		} else {

			Debug.LogError ("Error: Unit Controller couldn't find a Unit component in game object. Suggestion: Attach Unit script to game object, or remove Unit controller from game object");
			return null;

		}

	}

}
