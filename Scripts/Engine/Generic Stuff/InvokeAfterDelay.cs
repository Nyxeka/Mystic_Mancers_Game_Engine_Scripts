using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeAfterDelay : MonoBehaviour {

    [SerializeField]
    UnityEvent eventToInvoke;

    [SerializeField]
    float delay = 1.0f;

    void Start()
    {

        StartCoroutine(startAfterDelay());

    }

	IEnumerator startAfterDelay()
    {

        yield return new WaitForSeconds(delay);

        eventToInvoke.Invoke();

    }

}
