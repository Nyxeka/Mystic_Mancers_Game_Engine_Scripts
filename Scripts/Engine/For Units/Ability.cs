using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum targetList { 
	nearestUnit, hasCustomUnitTag, noTarget
} 

[AddComponentMenu("Final Integrated Stuff/Ability")]
public class Ability : MonoBehaviour {

	/*
	 * --------------------------------
	 * 
	 * Ability Class
	 * 
	 * This is a script component. You can add it through the inspector "add component" meny under Final Integrated Stuff.
	 * 
	 * 
	 * --------------------------------
	 */

	public string abilityName = "new ability";

	[Header("Cooldown in seconds")]
	public float cooldown = 0.5f;

	public float spawnProjectileDelay = .0f;
	private float curDelay = .0f;

	public float coolDownMult = 1.0f;

	public float cost = .0f;

	public Resource resource;

	public bool useResource = false;

	[Space(20)]

	public bool releaseFromLadderFirst = false;

	[Space(20)]

	[Header("You can leave this blank if you want")]
	public Transform projectile;
	public Transform projectileSpawnAt;
	public Vector3 projSpawnOffset;

    public bool spawnAsChild = false;

	[Space(20)]

	[Header("These events will be invoked on cast")]
	public UnityEvent customScriptTargets;


	[HideInInspector]
	/// <summary>
	/// A public string you can access to get the current cooldown time formatted as minutes:seconds
	/// </summary>
	public string timeLeftOnCooldown; //format 00:00 [minutes:seconds]

	//When this is above 0, the ability is on cooldown and un-usable.
	float curCooldown = .0f;

	bool hasProjectile = false;
	bool hasProjectileSpawnTarget = false;

	bool abilityTriggered = false;

	ArmDir abilityDirection;

	// Use this for initialization
	void Start (){
		
		if (!resource) {

			useResource = false;

		} else {
			//Debug.Log ("Using: " + resource.resourceName + ", has amount: " + resource.getResourceAmount().ToString());

		}

		if (projectile != null) 
			hasProjectile = true;
		
		if (projectileSpawnAt != null) 
			hasProjectileSpawnTarget = true;

	}

	/// <summary>
	/// Manually trigger an ability
	/// </summary>
	public bool TriggerAbility(){

		if (useResource) {

			if (cost > resource.getResourceAmount()) {
				Debug.Log ("Not Enough Mana! cost: " + cost.ToString() + ", " + resource.resourceName + ": " + resource.getResourceAmount().ToString());
				return false;


			}

		}
		//trigger events.
		//temp:
		if (curCooldown == .0f) {
			if (resource) {
				resource.Remove (cost);

			}
			abilityTriggered = true;
			//Debug.Log ("Ability Triggered: " + abilityName + " in gameObject: " + gameObject.name);
			customScriptTargets.Invoke ();
			curCooldown = cooldown*coolDownMult;
			return true;
		}

		return false;
		
	}

    public bool TriggerAbilityNoCooldown()
    {
        //trigger events.
        //temp:

        abilityTriggered = true;
        //Debug.Log ("Ability Triggered: " + abilityName + " in gameObject: " + gameObject.name);
        customScriptTargets.Invoke();
        curCooldown = cooldown * coolDownMult;
        return true;
    }
    /// <summary>
    /// Reset cooldown on ability
    /// </summary>
    public void ResetCooldown(){

		curCooldown = .0f;

	}

	/// <summary>
	/// Lets you put an ability on cooldown for newCooldown amount of time
	/// </summary>
	public void SetCurrentCooldown(float newCooldown){

		curCooldown = newCooldown;

	}

    /// <summary>
    /// get the cooldown of the ability in minutes:seconds
    /// </summary>
    /// <returns>returns a string eg, "00:01" or "25:72"</returns>
    public string getCooldownMinutes()
    {

        return (Mathf.Floor(curCooldown / 60).ToString("00")) + ":" + ((curCooldown % 60).ToString("00"));

    }

    public string getCooldownSeconds()
    {

        return (curCooldown % 60).ToString("00");

    }
	
	// Update is called once per frame
	void Update () {
        

		//need to set the rotation for projectiles depending on the arm pos...


		if (abilityTriggered) {
			curDelay += Time.deltaTime;

			if (curDelay >= spawnProjectileDelay) {

				curDelay = .0f;

				if (hasProjectile && hasProjectileSpawnTarget) {

                    if (spawnAsChild)
                        Instantiate(projectile, projectileSpawnAt.position + projSpawnOffset, projectileSpawnAt.rotation, gameObject.transform);
                    else
                        Instantiate(projectile, projectileSpawnAt.position + projSpawnOffset, projectileSpawnAt.rotation);
                    

				} else if (hasProjectile && !hasProjectileSpawnTarget) {
                    if (spawnAsChild)
                        Instantiate (projectile, gameObject.transform.position + projSpawnOffset, gameObject.transform.rotation);
                    else
                        Instantiate(projectile, gameObject.transform.position + projSpawnOffset, gameObject.transform.rotation, gameObject.transform);

                }

				abilityTriggered = false;



			}

		}

		//We can do cooldowns here, since it's pretty much only a visual thing.
		if (curCooldown != .0f) {

			if (curCooldown < .0f) {
				curCooldown = .0f;

			} else {

				curCooldown = curCooldown - Time.deltaTime;

			}


		}



	}
}
