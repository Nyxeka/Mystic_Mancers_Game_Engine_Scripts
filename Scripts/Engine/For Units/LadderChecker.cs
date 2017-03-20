using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderChecker : MonoBehaviour {

	GameUnit _unit;

	void OnTriggerStay(Collider collisionData){
		
		_unit = collisionData.gameObject.GetComponent<GameUnit> ();

		if (_unit) {

			_unit.ladder = true;
			_unit.ladderLockXLocation = gameObject.transform.position.x;

		}

	}

	void OnTriggerExit(Collider collisionData){

		_unit = collisionData.gameObject.GetComponent<GameUnit> ();

		if (_unit) {
			
			_unit.ladder = false;

		}

	}
}
