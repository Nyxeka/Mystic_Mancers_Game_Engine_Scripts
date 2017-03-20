using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCubeRotationHandler : MonoBehaviour {

	private GameUnit _unit;

    Quaternion pointLeft;
    Quaternion pointRight;
    Quaternion pointUp;
    Quaternion pointDown;

    // Use this for initialization
    void Start () {
		_unit = GetComponentInParent<GameUnit> ();

        pointRight = Quaternion.AngleAxis(0, Vector3.forward);
        pointDown = Quaternion.AngleAxis(270, Vector3.forward);
        pointLeft = Quaternion.AngleAxis(180, Vector3.forward);
        pointUp = Quaternion.AngleAxis(90, Vector3.forward); ;


    }
	
	// Update is called once per frame
	void Update () {

        if (_unit.armDir == ArmDir.up && _unit.legDir == LegDir.right)
            transform.rotation = pointUp;

        if (_unit.armDir == ArmDir.up && _unit.legDir == LegDir.left)
            transform.rotation = pointUp;

        if (_unit.armDir == ArmDir.down && _unit.legDir == LegDir.right)
            transform.rotation = pointDown;

        if (_unit.armDir == ArmDir.down && _unit.legDir == LegDir.left)
            transform.rotation = pointDown;

        if (_unit.armDir == ArmDir.forwards && _unit.legDir == LegDir.right)
            transform.rotation = pointRight;

        if (_unit.armDir == ArmDir.forwards && _unit.legDir == LegDir.left)
            transform.rotation = pointLeft;


	}
}
