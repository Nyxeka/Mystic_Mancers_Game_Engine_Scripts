using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantEnemyKill : MonoBehaviour {

	void OnCollisionEnter (Collision _col) 
	{	
		if (_col.gameObject.tag == "Projectile") 
		{
			Debug.Log ("Killing Enemy");
			Destroy (this.gameObject);
		}
	}
}