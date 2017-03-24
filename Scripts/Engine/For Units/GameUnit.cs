using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum LegDir {

	left, right

}

public enum ArmDir
{

	up, down, forwards

}

[AddComponentMenu("Final Integrated Stuff/Game Unit")]
public class GameUnit : MonoBehaviour {

    /*
	 * Unit Script
	 * 
	 * This will be the component that identifies a game object as a "unit"
	 * 
	 * A unit is basically any object or thing in the game world that will move around and interact with the level and other units.
	 * 
	 * Since this is built for a 2.5D side-scroller game with cardinal aiming, there will be a limited number of commands or things that a unit will be able to do
	 * 
	 * Here, a unit will have 4 movement controls: move left/right, and either look straight ahead or  up/down.
	 * 
	 * Will have a "controller", but we wont be creating one in here specifically, since its the controllers job to take the unit and control it. Controllers 
	 * must be added as additional components. If one doesn't exist, the unit will merely sit there and play idle.
	 * 
	 * Unit will provide handles and functions for controllers to utilize.
	 * 
	 */






    public string unitName = "new unit";


    public bool noclip = false; //if false then must fly.

    //public bool canFly = false;

    public bool gravity = true;

    //public GameObject belongsToFaction;
    //[Tooltip("Use for your units animation controller setup - unit input controllers and AI controllers are added via component menu.")]
    //public Animator unitAnimController;

    [Tooltip("This is the maximum speed a unit is allowed to go at all times. If the unit tries to go faster, it will stop.")]
    public float maximumMagnitude = 20;

    [Space(20)]

    //Don't want to show up in inspector, but we do want to be able to get this with controller scripts.
    [HideInInspector]
    public Rigidbody unitRB;

    [HideInInspector]
    public bool grounded = true;

    [HideInInspector]
    public bool ladder = false;

    [HideInInspector]
    public float ladderLockXLocation;

    public float ladderClimbSpeed = 5.0f;

    [HideInInspector]
	public bool ladderLocked = false; //set to true when attached to a ladder, until jump ability is casted. 

	//This is a generated list of abilities - we're going to search the gameobject for ability components that have been added.
	public Dictionary<string,Ability> abilityList;

	[HideInInspector]
	public LegDir legDir; //can be left or right
	private LegDir oldLegDir;
	[HideInInspector]
	public ArmDir armDir; //can be up down or right;
	private ArmDir oldArmDir;

	public bool canClimb = true;

	[HideInInspector]
	public float directionMult = 1.0f;

    [Header("Ignore this if the unit doesn't use dash.")]
	public float dashSpeedMult = 0.5f;

	public UnityEvent dashStartMethods;

	public UnityEvent dashEndMethods;

	[HideInInspector]
	public bool dashing = false;

	[HideInInspector]
	public bool moving = false;

	Vector3 newVelocity;

    bool maintainSpeed = true;

	// Use this for initialization
	void Start () {
		
        //find the abilities in the unit
		abilityList = new Dictionary<string, Ability> ();
        
		unitRB = gameObject.GetComponent<Rigidbody>();
		//TO-DO: Check if rigidbody exists and make a warning if it doesn't.

		Component[] abilityComponentsList = gameObject.GetComponents<Ability>();
		foreach (Ability i in abilityComponentsList){

			abilityList.Add (i.abilityName, i);
			//Debug.Log ("Added Ability: " + i.abilityName + " To: " + gameObject.name);

		}

		legDir = LegDir.left;
		armDir = ArmDir.forwards;
		oldLegDir = legDir;
		oldArmDir = armDir;

		unitRB.useGravity = gravity; 

		//gameObject.GetComponent<Collider>().isTrigger = 

		newVelocity = new Vector3(0.0f,0.0f,0.0f);

        //Keep the player from going to fast, esp. if you dash into a rigidbody and the physics send you flying
        //and a bazillion miles an hour.
        StartCoroutine(MaintainSpeed());

	}

	float boolToNormal(bool tester){
		if (tester)
			return 1;
		else
			return -1;
	}


    // Update is called once per frame
    void FixedUpdate() {
        unitRB.useGravity = gravity;

        if (oldLegDir != legDir) {
            //value has changed
            //either invoke events or talk to animator?
            if (ladder) {

                //do nothing yet really. Just turning around on the ladder

            }

        }

        if (oldArmDir != armDir) {
            //value has changed
            //either invoke events or talk to animator?

        }

        //turn off being attached to the ladder

        if (grounded && armDir == ArmDir.forwards)
            ladderLocked = false;
        
        //LADDER HANDLING
        if (ladder && canClimb) {

            if (armDir != ArmDir.forwards) {
                if (ladderLocked) {

                    if (!(grounded && armDir == ArmDir.down))
                    {

                        newVelocity.y = (boolToNormal(armDir == ArmDir.up)) * ladderClimbSpeed;

                        unitRB.MovePosition(new Vector3(ladderLockXLocation, unitRB.position.y, unitRB.position.z));
                        unitRB.velocity = newVelocity;
                    }

                } else {

                    ladderLocked = true;

                }
            } else {

                if (ladderLocked) {

                    newVelocity.y = 0;
                    unitRB.MovePosition(new Vector3(ladderLockXLocation, unitRB.position.y, unitRB.position.z));
                    unitRB.velocity = newVelocity;

                }

            }

        } else {

            ladderLocked = false;

        }
    

		if (ladderLocked) {

			gravity = false;

		} else {

			gravity = true;

		}


		if (legDir == LegDir.right) {
			directionMult = 1.0f;
		} else {
			directionMult = -1.0f;
		}
		//we want to talk to the animator when a direction is changed!

		//make sure that if this unit starts no-clipping, it doesn't fall out of the world.
		if (noclip == true) {

			gravity = false;

		}

	}

    IEnumerator MaintainSpeed()
    {

        while (maintainSpeed)
        {

            if (unitRB.velocity.magnitude > maximumMagnitude)
            {

                unitRB.velocity = unitRB.velocity.normalized * maximumMagnitude;



            }

            yield return new WaitForFixedUpdate();

        }

        yield return null;

    }

	public void addVelocityY(float newForce){
		if (ladderLocked)
			ladderLocked = false;
		else
			unitRB.AddForce (Vector3.up * newForce, ForceMode.Impulse);
	}

	public void addVelocityX(float newForce){
		if (ladderLocked)
			ladderLocked = false;
		else
			unitRB.AddForce (Vector3.right * newForce * directionMult, ForceMode.Impulse);
	}

	///<summary>
	///Basically for Jump.
	///</summary>
	public void setVelocityY(float newVelocity){
		if (ladderLocked)
			ladderLocked = false;
		else
			unitRB.velocity = new Vector3 (unitRB.velocity.x, newVelocity, unitRB.velocity.z);

	}

	public void DashForDuration(float dashTime){
		StartCoroutine (Dash(dashTime));
	}

	//Need a custom co-routine for dashing, since it can get complicated otherwise.
	public IEnumerator Dash(float dashTime){
		float curTime = .0f;
		//Need to change our physical appearance here
		dashStartMethods.Invoke();
		//
		while (curTime < dashTime) {
            dashing = true;
			curTime += Time.fixedDeltaTime;

			unitRB.MovePosition (unitRB.position + (Vector3.right * directionMult * dashSpeedMult));
			//Announce this as a co-routine.
			yield return new WaitForFixedUpdate();
		}
        dashing = false;
		dashEndMethods.Invoke ();
		//change physical appearance back
	}

	//Simply call this method to cast ability with name.
	public bool TriggerAbility(string abilityName){

		//temp "Ability" -
		Ability toCast;
		//We need to check if the ability we asked for actually exists, before we go and ask for it.
		if (abilityList.TryGetValue(abilityName, out toCast)){

			return toCast.TriggerAbility();

		} else {

			//Does not exist!!
			Debug.Log("Tried to trigger ability that does not exist: " + abilityName);
			return false;
		}

	}

    public void TriggerAbilityNoBool(string abilityName)
    {

        //temp "Ability" -
        Ability toCast;
        //We need to check if the ability we asked for actually exists, before we go and ask for it.
        if (abilityList.TryGetValue(abilityName, out toCast))
        {

            toCast.TriggerAbility();

        }
        else {

            //Does not exist!!
            Debug.Log("Tried to trigger ability that does not exist: " + abilityName);
        }

    }

    public bool ForceTriggerAbility(string abilityName)
    {

        //temp "Ability" -
        Ability toCast;
        //We need to check if the ability we asked for actually exists, before we go and ask for it.
        if (abilityList.TryGetValue(abilityName, out toCast))
        {

            return toCast.TriggerAbilityNoCooldown();

        }
        else {

            //Does not exist!!
            Debug.Log("Tried to trigger ability that does not exist: " + abilityName);
            return false;
        }

    }

    //TO-DO: Add an "on death" event list.
    public void Kill(){

		Debug.Log ("Killed: " + unitName);

	}
}
