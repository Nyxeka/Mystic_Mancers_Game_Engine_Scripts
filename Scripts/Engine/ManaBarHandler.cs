using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarHandler : MonoBehaviour {

    Slider manaFill;

    Resource resourceToTrack;

	// Use this for initialization
	void Start () {
        manaFill = gameObject.GetComponent<Slider>();
        resourceToTrack = GameObject.FindWithTag("Player").GetComponent<Resource>();
    }
	
	// Update is called once per frame
	void Update () {
		if (resourceToTrack)
        {

            manaFill.value = resourceToTrack.getPercentage();

        }
	}
}
