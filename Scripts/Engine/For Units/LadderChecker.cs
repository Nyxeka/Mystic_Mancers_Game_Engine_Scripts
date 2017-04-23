using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderChecker : MonoBehaviour {

    //------------------------
    // 2.5D Sidescroller Interactible Ladder
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    GameUnit _unit; // Unit using the ladder

	void OnTriggerStay(Collider collisionData){
		
		_unit = collisionData.gameObject.GetComponent<GameUnit> ();

		if (_unit) {

            // tell the unit that we're using the ladder
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
