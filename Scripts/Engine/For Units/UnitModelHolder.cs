using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitModelHolder : MonoBehaviour {

    GameUnit _unit;

    float yRotation;

    Quaternion pointLeft;
    Quaternion pointRight;
    Quaternion pointBackwards;

	// Use this for initialization
	void Start () {
        _unit = GetComponentInParent<GameUnit>();

        //set up rotations before hand, we don't need to do the math over and over and over again.
        pointLeft = Quaternion.AngleAxis(180, Vector3.up);
        pointRight = Quaternion.AngleAxis(0, Vector3.up);
        pointBackwards = Quaternion.AngleAxis(270, Vector3.up);
    }

	// Update is called once per frame
	void Update () {

        if (_unit.legDir == LegDir.left && !_unit.ladderLocked)
        {

            transform.rotation = pointLeft;
            
        }
        if (_unit.legDir == LegDir.right && !_unit.ladderLocked)
        {

            transform.rotation = pointRight;

        }
        if (_unit.ladderLocked && !(_unit.grounded && _unit.armDir == ArmDir.down))
        {

            transform.rotation = pointBackwards;

        }

    }
}
