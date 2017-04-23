using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeBackAndForthDelay : MonoBehaviour {

    //invoke back and forth delay. 
    //will wait for delay, then imvoke event, then wait for delay, and invoke other event.
    // can use this for toggling other things on and off, etc...

    public float delay = 1.0f;

    public UnityEvent eventA;

    public UnityEvent eventB;



	// Use this for initialization
	void Start () {
		
	}

    IEnumerator doToggling()
    {

        while (true)
        {

            eventA.Invoke();

            // guaruntee infinite application-breaking loop doesn't happen!
            yield return null;
            yield return new WaitForSeconds(delay - Time.deltaTime);

            eventB.Invoke();

            yield return null;
            yield return new WaitForSeconds(delay - Time.deltaTime);

        }

    }


}
