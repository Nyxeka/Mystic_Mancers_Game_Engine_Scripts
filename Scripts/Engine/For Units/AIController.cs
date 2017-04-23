using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[AddComponentMenu("Final Integrated Stuff/AI Controller")]
public class AIController : UnitController {

    //------------------------
    // AI Controller Component
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    // Generic ground-unit AI controller
    // requires a unit component
    #region variables
    public enum AIState
    {
        initialize, idle, patrolling, hunting, escaping, following
    }

    //if you don't declare the scope of a variable, it defaults to private.
    AIState currentState;
    
    // use ability components for moving, or built-in functions.
	public bool abilityControlMovement;

    // finite state machine running
	public bool AI_RUNNING = true;

    bool attacking = false;
    public float moveAfterAttackDelay = 0.0f;

    // for non-ability-controlled movement.
    [Header("Speed in meters per second")]
    public float speedmps = 1.0f;
    
	public float jumpHeight = 10.0f;
    public float jumpForwardsForce = 7.0f;

    // range from the player in which we will try to trigger attacks.
    public float attackRange = 10.0f;

    public float attackFrequency = 1.0f;

    // use for the triggercollider methods in the vision child game object
    EnemyVision vis;

    public float visionRange = 10.0f;

    //since we're basically only going to ever be targetting the player for now.
    Transform player;

    //our list of patrol-points.
	public List<Transform> patrolPointList;
	public int currentPatrolPointTarget = 0;


    //distance from the target location we need to be before we decide to stop mgoing forwards.
    public float distanceToStop = 0.0f;

    // this is the position that we'll be trying to move towards.
    Vector3 targetLocation;

    // the current number of attempts we've made to traverse obstacles between us and target.
	int numTriesForPoint = 0;

    // if the unit comes accross an obstacle, it will only try to get to its target
    // this many times before switching to another target.
	public int maxAttemptsToTravel = 6;

    // the delay between progress checks. 
    // minimum every 0.02 seconds. 
    public float progressCheckDelay = 0.5f;

    // we've attempted the jump. May be deprecated soon.
	bool attemptedJump = false;

    // tell the AI to make its next move a jump.
	bool nextMoveIsLeap = false;

    // minimum distance in which we can say we've "reached the target"
    public float minDistance = 0.5f;

    // for general use
    float distanceFromTarget = 0;

    /// <summary>
    /// Move check target location
    /// for the "have we moved closer to our target?" method.
    /// </summary>
    Vector3 moveCheckTL;

    float distanceFromMoveCheckTL;

    float oldDistanceFromTarget = 0;

    public bool printDebug;

    // update move check target location. For later use, 
    // while it's true we'll be updating the current pos relative to the last tracked
    // target position.
    bool updateMCTL = false;

    [SerializeField]
    bool checkFloorMissing = true;

    [SerializeField]
    float gapCheckIncrement = 1.0f;
    //in meters. Tells the AI to check for missing ground.

    [SerializeField]
    float gapCheckDepth = 3.0f;

    float moveDelay = 0;

    public bool startInIdle = false;

    #endregion

    void Start(){
		//since we are overriding start method from unitController parent:
		_unit = gameObject.GetComponent<GameUnit> ();

        vis = gameObject.GetComponentInChildren<EnemyVision>();

        if (!vis)
        {
            GameObject visGO;
            visGO = Instantiate(Resources.Load<GameObject>("Vision"), transform,false);
            vis = visGO.GetComponent<EnemyVision>();
        }

        //set start state
        currentState = AIState.initialize;
        
		if (_unit)
			AI_RUNNING = true;
		else
			AI_RUNNING = false;

        if (progressCheckDelay <= 0.02f)
        {

            progressCheckDelay = 0.02f;

        }

        // we want to make sure there are patrol points that have been put in here.
        

        
        // begin the finite state machine.
        StartCoroutine (AI_FSM ());
        
	}

    #region methods
    void updateTargetLocation()
    {

        targetLocation = patrolPointList[currentPatrolPointTarget].transform.position;

    }

	//assuming enemy is facing player, which it should be, especially if the player is in the AI zone.
	public void MoveForwards(){
		
		if (nextMoveIsLeap) {
			//leap!
			StartCoroutine( JumpForwards ());
			nextMoveIsLeap = false;
		}else if (abilityControlMovement) {
            
            if (_unit.TriggerAbility("Move"))
                StartCoroutine(CheckIfMovedToTarget());

		} else {
            _unit.unitRB.MovePosition(transform.position + (Vector3.right * (_unit.directionMult * speedmps * Time.deltaTime)));
		}

	}

	IEnumerator JumpForwards(){
        debug ("Trying a Leap");
        if (abilityControlMovement) {
			if (_unit.TriggerAbility ("Leap")) {
                debug ("Lept!");
                StartCoroutine(CheckIfMovedToTarget());
                yield return new WaitForSeconds (0.3f);
				_unit.ForceTriggerAbility ("Move");
			}

		} else {

            if (_unit.TriggerAbility("Leap"))
            {
                //Debug.Log("We're still going up!!");
                yield return new WaitForSeconds(0.3f);
                _unit.unitRB.AddForce((_unit.directionMult * jumpForwardsForce), jumpHeight / 2, .0f, ForceMode.Impulse);
            }

        }

		attemptedJump = true;
	}

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


	bool IsFloorMissing(){
        //basically in here, we're going to scan the floor before we move. A few simple raycasts should do the trick.
        //if we notice the floor is missing, we don't move forwards again, instead we jump.
        //if it's to far, we turn around. 
        if (checkFloorMissing)
        {
            bool groundIsThere = true;

            Ray cast = new Ray(transform.position + Vector3.right * _unit.directionMult * gapCheckIncrement, Vector3.down);

            groundIsThere = Physics.Raycast(cast, gapCheckDepth);

            Debug.DrawLine(cast.origin, cast.origin + (Vector3.down * gapCheckDepth), Color.black, 0.5f);

            return !groundIsThere;
        }
        
        return false;
        
	}

    void useAttackAbility()
    {
        
        _unit.TriggerAbility("Attack");

    }

    bool isFacingPlayer()
    {

        float dirToPlayer = player.transform.position.x - transform.position.x;

        //Debug.Log("Player in sight!");

        if ((dirToPlayer / Mathf.Abs(dirToPlayer)) == _unit.directionMult)
        {

            return true;

        }

        return false;

    }

    public void DelayMoving(float time)
    {

        moveDelay = time;

    }

    #endregion

    #region coroutines

    IEnumerator UpdateMCTL() //update move check target location. 
    {
        /*
        basically we want to be doing this for a fixed amount of time.
        */
        float time = 0.0f;
        updateMCTL = true;
        while (updateMCTL)
        {
            time = time + Time.deltaTime;
            distanceFromMoveCheckTL = Vector3.Distance(transform.position, moveCheckTL);

            if (time > 10.0f) //this should never last longer than 10 seconds. 
            {
                break;
            }

            yield return null;

        }

    }

    
    IEnumerator CheckIfMovingToTargetContinuousMovement()
    {

        moveCheckTL = targetLocation;
        //debug ("Started routine: check if moving towards target");
        //if the old distance is less than the new distance, then we haven't made progress.
        yield return new WaitForSeconds(progressCheckDelay);
        while (true)
        {
            if (!attacking)
            {
                distanceFromMoveCheckTL = Vector3.Distance(transform.position, moveCheckTL);

                if (Mathf.Abs(distanceFromMoveCheckTL - oldDistanceFromTarget) < 0.01)
                {

                    // we've moved less than 0.01 meters since the last movement check.
                    if (!attemptedJump)
                        nextMoveIsLeap = true;
                    else
                        turnAround();

                }
                else if (distanceFromMoveCheckTL > oldDistanceFromTarget)
                {

                    // we've been moving away from our target. Turn around!
                    turnAround();

                }
                else
                {

                    attemptedJump = false;

                }

                if (IsFloorMissing())
                {

                    turnAround();

                }

                oldDistanceFromTarget = distanceFromMoveCheckTL;

                moveCheckTL = targetLocation;
            }
            yield return new WaitForSeconds(progressCheckDelay);
        }

    }

    IEnumerator CheckIfMovedToTarget(){

        moveCheckTL = targetLocation;
        StartCoroutine(UpdateMCTL());
        //debug ("Started routine: check if moving towards target");
        //if the old distance is less than the new distance, then we haven't made progress.
        debug("██████RUNNING MOVEMENT SCAN██████");
        
        oldDistanceFromTarget = distanceFromMoveCheckTL;

        debug("old distance:" + oldDistanceFromTarget.ToString());
        //wait-until: Create a delegate for that boolean, and check it every frame until its true!
        debug("waiting for movement to start");
        //wait for movement to start.
        //yield return new WaitUntil(() => _unit.unitRB.velocity.magnitude > 0.01);
        yield return new WaitForSeconds(0.1f);
        debug("waiting for movement to end");
        //Now that movement has started, we want to wait until its over.
        yield return new WaitUntil(() => _unit.unitRB.velocity.magnitude < 0.01);
        debug("checking for movement:");

        if (IsFloorMissing())
        {
            turnAround();
            numTriesForPoint++;

        }else if ((Mathf.Abs (oldDistanceFromTarget - distanceFromMoveCheckTL) < 0.1f)) { //no movement and we haven't jumped yet.
			nextMoveIsLeap = true;
            numTriesForPoint++;
            debug("numTriesForPoint: " + numTriesForPoint.ToString());
        }else if (oldDistanceFromTarget < distanceFromMoveCheckTL) {
            debug("new distance = " + distanceFromMoveCheckTL.ToString());
            debug("Turning around because I'm moving away from my target.");
			turnAround ();
            
        } else
        {
            numTriesForPoint = 0;
        }

        

        if (numTriesForPoint > maxAttemptsToTravel)
        {
            nextPoint();
            if (!IsFloorMissing())
            {
                turnAround();
            }

            targetLocation = patrolPointList[currentPatrolPointTarget].transform.position;
            numTriesForPoint = 0;
            debug("Switching Targets, since this just isn't working.");
        }

        debug("█  █  █  █Scan Over█  █  █  █");
        updateMCTL = false;

    }

    #endregion

    #region States

    IEnumerator initialize(){

        yield return new WaitForSeconds (2);

        debug ("Ran init on AI controller for " + gameObject.name);
        
        StartCoroutine(_unit.doMovementChecking());

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (patrolPointList == null)
        {

            patrolPointList = new List<Transform>();

        }
        if (patrolPointList.Count > 0)
        {
            for (int i = 0; i < patrolPointList.Count; i++)
            {

                if (patrolPointList[i] == null)
                {

                    Debug.Log("Warning! No patrol point set for: " + name + " on slot: " + (i + 1).ToString() + "!\nConsider reducing the size of the list or adding an object to the list.");
                    patrolPointList[i] = transform;

                }

            }
        }
        else
        {

            Debug.Log("Warning, patrol point list empty! Adding self as patrolPoint");
            patrolPointList.Add(transform);
            currentPatrolPointTarget = 0;

        }

        if (player != null)
        {
            if (startInIdle)
            {
                currentState = AIState.idle;
            }
            else {
                currentState = AIState.patrolling;
            }

        } else
        {

            currentState = AIState.idle;

        }

		yield return null;

	}

	IEnumerator idle(){
        //More or less the same as patrolling, except while standing still.
        //will queue the abilities as usual.
        while (currentState == AIState.idle)
        {
            if (vis.enemyInSight)
            {

                if (isFacingPlayer())
                {

                    currentState = AIState.hunting;
                    targetLocation = player.position;

                }

            }

            yield return null;

        }
        
	}

	IEnumerator patrolling(){

        debug("Entered Patrolling State");
        //WalkAround
        //if point A and point B are set, walk to those. 
        //bool goingToPoint = true;
        bool atTarget = false;

        //float oldDistanceToPoint;

        if (!abilityControlMovement)
        {

            StartCoroutine("CheckIfMovingToTargetContinuousMovement");

        }

        while (currentState == AIState.patrolling) {

            targetLocation = patrolPointList[currentPatrolPointTarget].transform.position;
            distanceFromTarget = Vector3.Distance (transform.position, targetLocation);

            
            if (distanceFromTarget > distanceToStop)
            {
                MoveForwards();
            }

			if (distanceFromTarget < minDistance && !atTarget) {
				
				nextPoint ();
                debug("target set to: " + patrolPointList [currentPatrolPointTarget].gameObject.name);
                atTarget = true;

            }
            else if (atTarget)
            {

                if (distanceFromTarget > minDistance)
                {

                    atTarget = false;

                }

            }

            if (vis.enemyInSight)
            {

                if (isFacingPlayer()) { 

                    currentState = AIState.hunting;
                    targetLocation = player.position;

                }

            }

			yield return null;

		}

		StopCoroutine ("CheckIfMovedToTarget");
        StopCoroutine("CheckIfMovingToTargetContinuousMovement");
    }

	IEnumerator hunting(){

        float attackTimer = 0;

        //float checkDist;

        float jumpDelay = 1.5f;

        float jumpTimer = 0.0f;
        
        while (currentState == AIState.hunting)
        {
            attackTimer += Time.deltaTime;
            
            targetLocation = player.position;
            distanceFromTarget = Vector3.Distance(transform.position, targetLocation);

            if (!isFacingPlayer())
            {
                //Debug.Log("not facing player!");
                turnAround();

            }

            if (moveDelay > 0)
            {
                yield return new WaitForSeconds(moveDelay);
                moveDelay = 0;
                
            }
            
            if (distanceFromTarget > distanceToStop)
            {
                MoveForwards();
            }

            if (distanceFromTarget > visionRange)
            {

                currentState = AIState.patrolling;
                updateTargetLocation();

            }

            if (distanceFromTarget < attackRange)
            {
                if (attackTimer > attackFrequency)
                {
                    if (isFacingPlayer())
                    {
                        useAttackAbility();
                        attackTimer = 0;
                        attacking = true;
                        yield return new WaitForSeconds(moveAfterAttackDelay);
                        attacking = false;
                    }
                }
            }

            yield return null;

        }
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
                // this pauses until the state is finished.
				yield return StartCoroutine(currentState.ToString ());
			}
		}

	}
    #endregion

    void debug(string text)
    {
        if (printDebug)
            Debug.Log(text);
    }

}
