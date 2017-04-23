using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyFollow : MonoBehaviour {

    //------------------------
    // Lerp Follow Script
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    public Transform toFollow;

    public Vector3 offset;

    public float delay = 0.5f;

	// Use this for initialization
	void Start () {
        StartCoroutine(FollowTarget());
	}
	
	// Update is called once per frame
	IEnumerator FollowTarget()
    {

        while (true)
        {

            this.transform.position = Vector3.Lerp(transform.position, toFollow.position + offset, delay * Time.deltaTime);

            yield return null;

        }

    }
}
