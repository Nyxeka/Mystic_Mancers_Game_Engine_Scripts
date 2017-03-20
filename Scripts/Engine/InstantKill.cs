using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantKill : MonoBehaviour {

	void OnTriggerEnter (Collider _col) 
	{	
		if (_col.tag == "Player") 
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}
}
