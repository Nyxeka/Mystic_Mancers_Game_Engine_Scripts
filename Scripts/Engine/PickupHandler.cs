using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupHandler : MonoBehaviour {

	[SerializeField]
	public static int totalPickUps;
	public static int currentPickUps = 0;

	[SerializeField]
	public Text totalText;
	[SerializeField]
	public Text currentText;

	void Start ()
	{
		totalText.text = totalPickUps.ToString ();
	}

	void Update ()
	{
		currentText.text = currentPickUps.ToString ();
	}
}
