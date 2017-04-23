using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeOnCollide : MonoBehaviour {

    public UnityEvent eventOnStart;

    public UnityEvent eventOnCollide;

    void Start()
    {

        eventOnStart.Invoke();

    }

    void OnCollisionEnter()
    {

        eventOnCollide.Invoke();

    }
	
}
