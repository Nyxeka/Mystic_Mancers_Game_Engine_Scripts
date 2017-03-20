using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Glacie_Projectile : MonoBehaviour 
{
	public float lifetime = 4.0f;
	public float minCheckTime = 0.5f; //after this amount of time, we check if we're close enough to the player
	float currentLifetime = 0.0f; //anything not specifically set as public or protected or private is set to private. This should be private

	public float throwDistance = 10;

	public float speed = 2;

    public float minDistanceToCatch = 1;

    public int damage = 1;

    //un-needed for now
    //Vector3 curPlayerPos;
    //Vector3 origPlayerPos;

    //Don't need this, have distance:
    //Vector3 maxPos;

    Rigidbody rb;

    //you'd want to set these up here for ease of use.
    float moveSpeedReturn = 0.1f;

    float curDistance = 0;

    bool didDamage = false;

    GameUnit _player;

	void Start()
	{
        _player = GameObject.FindWithTag("Player").GetComponent<GameUnit>();
		rb = gameObject.GetComponent<Rigidbody> ();
        if (rb)
            rb.AddRelativeForce(Vector3.right * speed, ForceMode.Impulse);

    }

	void OnCollisionEnter(Collision collisionList)
	{
		//Debug.Log ("DESTROYED GLACIE_PROJECTILE collide with: " + collisionList.gameObject.name);
        //I'll be fixing the way we access the camera shake soon lol. The way I did it in the projectile script was a bit of a hack.
        //it works though, but it's not modular. It's like, instead of getting on the bus to go to school, you use a cannon to shoot yourself in the air
        //towards the school, and hope they kept the net over there to catch you.
		GameObject.Find ("CameraGuide").GetComponent<cameraController> ().CameraShake (0.3f);
        GameUnit collided;
        if (didDamage == false)
        {
            if (collided = collisionList.collider.gameObject.GetComponent<GameUnit>())
            {

                //We hit a game unit, so do ur damage or whatever here
                collided.SendMessage("removeHealth", damage);
                didDamage = true;

            }
        }
    }

	void FixedUpdate ()
    {
        curDistance = transform.localPosition.magnitude;
        //this is good.
		currentLifetime += Time.fixedDeltaTime;
        if (currentLifetime >= lifetime)
            GameObject.Destroy(gameObject);

        if (currentLifetime >= minCheckTime)
        {

            if (curDistance <= minDistanceToCatch)
            {
                Ability temp;
                if (_player.abilityList.TryGetValue("Ability1", out temp))
                {

                    temp.ResetCooldown();
                    GameObject.Destroy(gameObject);
                }

            }

        }

        rb.AddForce(((transform.localPosition * -1) * moveSpeedReturn), ForceMode.Impulse);

	}
}
