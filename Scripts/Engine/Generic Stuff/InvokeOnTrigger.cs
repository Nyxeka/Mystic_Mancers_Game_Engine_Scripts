using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeOnTrigger : MonoBehaviour {

    public UnityEvent eventOnStart;

    public UnityEvent eventOnTrigger;
    
    void Start()
    {

        eventOnStart.Invoke();

    }

    void OnTriggerEnter()
    {

        eventOnTrigger.Invoke();

    }

}
