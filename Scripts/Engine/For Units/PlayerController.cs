﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Final Integrated Stuff/Player Controller")]
public class PlayerController : UnitController {

    //------------------------
    // Player Input Handler
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    public enum InputMethod
	{
		mouseKeyboard, steamcontroller, gamePad
	}

	public InputMethod inputMethod;

	public float speedMult = 1.0f;

	public float maxSpeed = 3.0f;

	public string ability1Name = "Ability1";
	public string ability2Name = "Ability2";
	public string ability3Name = "Dash";
	public string ability4Name = "Jump";
    
	private float horizontalAxis = .0f;
	private float verticalAxis = .0f;

	private Vector3 newVelocity;

	private bool UP = false;
	private bool DOWN = false;
	private bool RIGHT = false;
	private bool LEFT = false;

	private bool jumped = false;
	private bool doubleJumped = false;

	private bool groundedOld = false;
    [HideInInspector]
    public bool grounded = true;

    SelectedCharacterHandler SCH;

	//public float groundDistCheck = 0.5f;

	void Start(){

		//Find the unit script in our game object, since we're over-riding the start method.
		_unit = gameObject.GetComponent<GameUnit> ();

		newVelocity = new Vector3 ();

        StartCoroutine(initialize());

	}

    IEnumerator initialize()
    {

        yield return new WaitForSeconds(1);
        
        GameObject SCHGO = GameObject.FindGameObjectWithTag("SCH");
        if (SCHGO != null)
        {
            //Debug.Log("Found SCH. attaching.");
            SCH = SCHGO.GetComponent<SelectedCharacterHandler>();
        }
            

    }

    public void showDeathScreen()
    {

        GameObject hudGO;

        if (hudGO = GameObject.FindGameObjectWithTag("HUD"))
        {

            hudGO.GetComponent<HUDmanager>().activateDeathScreen();

        } else
        {

            Debug.Log("No DeathScreen found!");

        }

    }

    public void setSCH(SelectedCharacterHandler newSCH)
    {

        SCH = newSCH;

    }

    public void addPickup(string newPickup)
    {

        if (SCH != null)
        {
            SCH.addPickup(newPickup);
        }

    }

	void Update(){
		_unit.grounded = grounded;

		if (groundedOld && !grounded) {
			
			//jumped = true;
			doubleJumped = false;
			groundedOld = grounded;

		} else if (!groundedOld && grounded) {

			jumped = false;
			groundedOld = grounded;

		}

		if (doubleJumped && grounded)
			jumped = false;

		if (_unit.ladder) {

			jumped = false;
			doubleJumped = false;
		}

		//have to handle button press events in here because of the way unity does inputs.
		//which is actually a pretty good way of doing things.
		//also gotta check to make sure that we aren't paused.
		if (Time.timeScale != 0) {
			if (Input.GetButtonDown ("Fire1")) {
				//B
				_unit.TriggerAbility (ability1Name);

			}
			if (Input.GetButtonDown ("Fire2")) {
				//Y
				_unit.TriggerAbility (ability2Name);

			}
			if (Input.GetButtonDown ("Fire3")) {
				//X
				_unit.TriggerAbility (ability3Name);

			}
			if (Input.GetButtonDown ("Jump") && (!doubleJumped || !jumped)) {
				//A
				_unit.TriggerAbility (ability4Name);
				doubleJumped = jumped;
				jumped = true;
			}
		}

	}




	//run every physics update
	void FixedUpdate(){

		//handling horizontal movement for the player in a fairly simple way for now. Probably wont change in the future as this works pretty well and is pretty smooth.
		horizontalAxis = Input.GetAxis ("Horizontal");
		verticalAxis = Input.GetAxisRaw ("Vertical");
		if ((Mathf.Abs (horizontalAxis) > .1f) || Mathf.Abs (verticalAxis) > .1f) {
			LEFT = (horizontalAxis < 0);
			RIGHT = (horizontalAxis > 0);
			UP = (verticalAxis > (Mathf.Abs (horizontalAxis)/2));
			DOWN = (verticalAxis < (.0f - (Mathf.Abs (horizontalAxis)/2)));
		}

		if (Mathf.Abs (verticalAxis) < 0.1) {

			UP = false;
			DOWN = false;

		}

		//handle movement

		if (LEFT)
			_unit.legDir = LegDir.left;
		if (RIGHT)
			_unit.legDir = LegDir.right;
		
		if (UP) {
			_unit.armDir = ArmDir.up;
		} else if (DOWN) {
			_unit.armDir = ArmDir.down;
		} else {
			_unit.armDir = ArmDir.forwards;
		}

		_unit.moving = (horizontalAxis != 0);

        if (_unit.moving == false)
        {

            _unit.unitRB.velocity = SlowDownHorizontalVector(_unit.unitRB.velocity,1.3f);

        }

		newVelocity = (Vector3.right * horizontalAxis * speedMult);
		//_unit.unitRB.MovePosition (_unit.transform.position + newVelocity);
		_unit.unitRB.AddForce(newVelocity * (maxSpeed - (Mathf.Abs( _unit.unitRB.velocity.x))),ForceMode.Impulse);
		//---

	}
    //gonna use this to slow down the player.
    Vector3 SlowDownHorizontalVector(Vector3 vec, float amount = 2)
    {
        if (amount < 1)
        {
            return new Vector3(vec.x / 2, vec.y, vec.z);
        } else
        {
            return new Vector3(vec.x / amount, vec.y, vec.z);
        }
        

    }

}
