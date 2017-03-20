using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * List of states:
 * 
 * initialize:
 * AI starting up. Do whatever needs to be done here.
 * 
 * idle:
 * AI will sit and idle
 * 
 * patrolling:
 * AI is just walking back and forth
 * 
 * hunting: 
 * If patrolling or following finds a unit of enemy faction, then give chase and attack with whatever methods are available
 * 
 * 
 * -------
 * While our AI is moving, we want it to try to move, check and see if its moving, and think about how it could get past an obstical. Also Check to see if there's something for it to walk on. 
 * 
 * What to do about flying enemies?
 * 
 * Would have abilities for moving up and down.
 * 
 * AI controller will have separate controls for special enemies.
 * 
 * Flying enemies will have to have abilities that say stuff like:
 * "move towards player", with triggers like "isPlayerVisible"
 * Attack player when ability is off cooldown
 * 
 * Will be handled by abilities.
 * Need to add some more ability triggers somehow.
 * 
 * Default AI will have a set "resolution" for pathfinding.
 * We'll set a range of vision, and a bunch of triggers for what happens when things appear or disappear in the vision.
 * We'll provide functions like 
 * (if this percentage of the vision returns nothing from the top to the bottom, then do this thing)
 * "if more than this percentage of the vision returns something,
 * or if it returns nothing between this angle of visiona nd this angle of vision, then do something, like change direction, cast ability, etc...
 * 
 * 
 * AI will also have some sort of defined "awareness zone". We need to figure out how to do that.
 * 
 * So like, if a unit of a certain faction enters the awareness "zone", then a raycast is drawn to that unit, and if it hits that unit, then
 * the AI can "see" that unit.
 * Maybe just draw raycasts to all units in the gameworld of a certain faction. (this would be the faction they are hunting).
 * 
 * Enemy awareness zones will be handled with 2D colliders.
 * 
 * Make sure to Get list of abilities properly.
 * 
 * use: physics.spherecast to check and see if there are things that we can walk through?
 * 
 * use overlapSphere 
 * 
 */

public enum AIState { 
	initialize, idle, patrolling, hunting, escaping, following
}

public enum MoveType {
	moveWalk,moveHop,moveFloat
}

[AddComponentMenu("Final Integrated Stuff/AI Controller")]
public class AIController : UnitController {

	//if you don't declare the scope of a variable, it defaults to private.
	AIState currentState;

	public MoveType moveType = MoveType.moveHop;

	public bool friendly;

	public bool flyingUnit = false;

	public float followDistance = 20.0f;

	public float fovMin = -60.0f;
	public float fovMax = 60.0f;

	public float visionRange = 20.0f;

	public float distanceToAttackTarget = 10.0f;

	public bool followPlayer = false;
	[Header("When out of range:")]
	public bool teleportToPlayer = true;

	[Space(20)]
	public bool noclip;

	public bool abilityControlMovement;

	public bool AI_RUNNING = true;

	public float speedMultManualMove = 15;

	public float jumpHeight = 10;

	public List<Transform> patrolPointList;
	public int currentPatrolPointTarget = 0;

	GameUnit targetUnit;

	GameUnit followUnit;

	//how many seconds will pass before we check the distance between us and target, and if we're not getting closer, then turn around.
	public float delayNoMoveTurnAround = 2;

	float maxJumpHeight; //This is set every time we do a jump.

	Vector3 targetLocation;

	//bool constantForwardVelocity = false;

	int numTriesForPoint = 0;

	public int maxAttemptsToTravel = 6;

	bool attemptedJump = false;

	bool nextMoveIsLeap = false;

	//bool checkedGround = false;

	/// <summary>
	/// The number of raycasts to put between the two angles of FOV on the ground infront of the unit.
	/// Vision Resolution
	/// </summary>
	/*public int visRes = 3;

	public float minFOV = -80.0f;
	public float maxFOV = -45.0f;

	public float checkRange = 8;
	//probably not going to use these.
	*/

	void Start(){
		//since we are overriding start method from unitController parent:
		_unit = gameObject.GetComponent<GameUnit> ();

		//set start state
		currentState = AIState.initialize;

		if (_unit)
			AI_RUNNING = true;
		else
			AI_RUNNING = false;

		StartCoroutine (AI_FSM ());

	}

	//assuming enemy is facing player, which it should be, especially if the player is in the AI zone.
	public void MoveForwards(){
		

		if (nextMoveIsLeap) {
			//leap!
			StartCoroutine( JumpForwards ());
			nextMoveIsLeap = false;
		}else if (abilityControlMovement) {
			
			_unit.TriggerAbility ("Move");

		} else if (moveType == MoveType.moveHop) {
			//normal move
			_unit.unitRB.AddForce (Vector3.right * (_unit.directionMult * speedMultManualMove));

		} else if (moveType == MoveType.moveWalk) {

			_unit.unitRB.AddForce (Vector3.right * (_unit.directionMult * speedMultManualMove));
		}

	}

	IEnumerator JumpForwards(){
		//Debug.Log ("Trying a Leap");

		if (abilityControlMovement) {
			if (_unit.TriggerAbility ("Leap")) {
				//Debug.Log ("Lept!");
				yield return new WaitForSeconds (0.3f);
				_unit.ForceTriggerAbility ("Move");
			}

		} else {

			_unit.unitRB.AddForce ((_unit.directionMult * speedMultManualMove),jumpHeight,.0f);

		}

		attemptedJump = true;
		yield return null;
	}

	//When patrolling
	public void turnAround(){

		if (_unit.legDir == LegDir.left)
			_unit.legDir = LegDir.right;
		else
			_unit.legDir = LegDir.left;

		_unit.armDir = ArmDir.forwards;

		attemptedJump = false;

	}

	void nextPoint(){

		currentPatrolPointTarget = (currentPatrolPointTarget + 1) % patrolPointList.Count;
		attemptedJump = false;
	}


	bool checkForMissingFloor(){
		//basically in here, we're going to scan the floor before we move. A few simple raycasts should do the trick.
		//if we notice the floor is missing, we don't move forwards again, instead we jump.
		//if it's to far, we turn around. 

		/*bool groundIsThere = true;

		float increment = (Mathf.Abs(maxFOV) - Mathf.Abs(minFOV))/(float)visRes;

		for (int i = 0;i < visRes;i++){
			
			groundIsThere = (Physics.Raycast(transform.position, Quaternion.AngleAxis(minFOV - (increment*(float)i),Vector3.forward) * Vector3.right,8));
				
		}
		return groundIsThere;*/
		return false;


	}

	IEnumerator resetJumpAttempt(){

		while (AI_RUNNING) {

			attemptedJump = false;
			yield return new WaitForSeconds (1.5f);
		}



	}

	IEnumerator checkIfMovingToTarget(){

		Debug.Log ("Started routine: check if moving towards target");

		float distance;

		distance = Vector3.Distance (transform.position, targetLocation);

		float oldDistance = distance;

		yield return new WaitForSeconds (1);

		while (AI_RUNNING) {

			distance = Vector3.Distance (transform.position, targetLocation);
			//if there's no progress, turn around and move in the other direction.

			//if the old distance is less than the new distance, then we haven't made progress.

			if ((Mathf.Abs (oldDistance - distance) < 0.1f) && !attemptedJump) {
				//very little progress. Attempt leap.
				nextMoveIsLeap = true;
				Debug.Log ("Going to Attempt a Leap.");
				//yield return new WaitForSeconds (delayNoMoveTurnAround);
				//numTriesForPoint++;

			} else if ((Mathf.Abs (oldDistance - distance) < 0.1f) && attemptedJump){
				numTriesForPoint++;
				turnAround ();

			}else if (oldDistance < distance) {
				Debug.Log ("Turning around because I'm moving away from my target.");
				numTriesForPoint++;
				turnAround ();

				if (numTriesForPoint > maxAttemptsToTravel) {
					nextPoint ();
					targetLocation = patrolPointList [currentPatrolPointTarget].transform.position;
					numTriesForPoint = 0;
					Debug.Log ("Switching Targets, since this just isn't working.");
				}

			} else {
                attemptedJump = false;
            }

			oldDistance = distance;

			yield return new WaitForSeconds (delayNoMoveTurnAround);

		}

		Debug.Log ("Exiting routine: checkIfMovingToTarget");

	}

	IEnumerator initialize(){

		yield return new WaitForSeconds (2);

		Debug.Log ("Ran init on AI controller for " + gameObject.name);


		currentState = AIState.patrolling;

		yield return null;

	}

	IEnumerator idle(){
		//More or less the same as patrolling, except while standing still.
		//will queue the abilities as usual.

		yield return null;


	}

	IEnumerator patrolling(){
		
		float distance;

		Debug.Log ("Entered Patrolling State");

		targetLocation = patrolPointList[currentPatrolPointTarget].transform.position;

		//WalkAround
		//if point A and point B are set, walk to those. 
		bool goingToPoint = true;

		//float oldDistanceToPoint;
		StartCoroutine (checkIfMovingToTarget ());

		while (goingToPoint) {
			
			distance = Vector3.Distance (transform.position, targetLocation);

			MoveForwards ();
			if (distance < 1.0) {
				
				if (Vector3.Distance (targetLocation, patrolPointList[currentPatrolPointTarget].transform.position) < 1.0) {
					nextPoint ();
					targetLocation = patrolPointList[currentPatrolPointTarget].transform.position;
					Debug.Log ("target set to: " + patrolPointList [currentPatrolPointTarget].gameObject.name);
				}

				//Debug.Log ("Switching Target: " + distance.ToString() + " : " + targetLocation.ToString());

			}

			yield return null;

		}

		yield return null;

		StopCoroutine ("checkIfMovingToTarget");
	}

	IEnumerator hunting(){

		bool inRange = true;

		while (inRange) {

			//make sure this is done in fixedupdate.
			yield return new WaitForFixedUpdate();

		}

		yield return null;

	}

	IEnumerator escaping(){

		yield return null;

	}

	IEnumerator following(){

		yield return null;

	}

	IEnumerator AI_FSM() {

		while (true) {
			if (AI_RUNNING) {
				yield return StartCoroutine(currentState.ToString ());
			}
		}

	}

}
