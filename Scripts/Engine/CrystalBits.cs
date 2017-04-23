using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBits : MonoBehaviour {

	// Crystal Bit Script
    //re-wrote by Nick
    
    private string _customHash;

    void Start()
    {

        _customHash = name + ((1000 * transform.position.x) + transform.position.y + (.001 * transform.position.z)).ToString();

    }

    public string customHash
    {
        get
        {
            return _customHash;
        }

    }

    public static string GetCustomCrystalHash(GameObject g)
    {

        return g.name + ((1000 * g.transform.position.x) + g.transform.position.y + (.001 * g.transform.position.z)).ToString();

    }

	void OnTriggerEnter (Collider _col) 
	{	
		if (_col.tag == "Player") 
		{
			//Vector3 position = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			//AudioSource.PlayClipAtPoint (clip, position);

            PlayerController p;

            if (p = _col.GetComponent<PlayerController>())
            {

                p.addPickup(_customHash);

            }

			Destroy (this.gameObject);
		}
	}

}
