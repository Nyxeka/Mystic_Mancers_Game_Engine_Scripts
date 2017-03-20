using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObjectScript : MonoBehaviour {

	void OnTriggerEnter (Collider _col) 
	{	
		if (_col.tag == "Player") 
		{
			PickupHandler.currentPickUps += 1;
			Destroy (this.gameObject);
		}
	}

}
