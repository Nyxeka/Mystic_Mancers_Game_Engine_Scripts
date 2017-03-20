using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine.UI;

public class AnimationHandler : MonoBehaviour {

	/*
	 * Animation Handler for Units.
	 * 
	 * In here we're going to provide Unity Events that we'll use to invoke triggers and booleans in the animation controller.
	 * 
	 * Also during ladder climbing and stuff, we need to tell the anim controller to set the ladder climb animation.
	 * 
	 * Idle animation needs to be set manually.
	 * Make a list of animations for the idles.
	 * Make another list for the "roll" or "number of rolls" or "chance" for each one.
	 * 
	 * we need events for the changing of each tye of value, from jumping, double-jumping, falling, running, idle
	 * 
	 * We need to track velocity.
	 * 
	 * Talk to the playercontroller, and check if we're moving forwards or not?
	 * Or maybe the player controller talks to animation handler.
	 * Or maybe we just track the velocity.
	 * 
	 * Tracking velocity and grounded would be a good idea.
	 * We also want to get the general acceleration of the player. 
	 * Maybe something like, if we're slowing down...
	 * 
	 * We need a public bool somewhere that says "IsHoldingRight". We can't get input here. We need to talk to the gameunit for this because this should work for 
	 * enemies as well.
	 * 
	 * have a checkmark for "stopping animation"
	 * 
	 * So:
	 * 
	 * enter idle
	 * 
	 * running
	 * IF (grounded) and (moving)
	 * 
	 * We need to also get the animations from the animations in a character game object.

        OK. So, we search for the animations first.
        I guess this will be done during runtime.
	 * 
	 */

    Animation playerAnim;

    List<AnimationClip> animList;
    List<int> speed;

	GameUnit _unit;

    Animator playerAnimator;

    Avatar plAv;

    AvatarMask plAvMask;

    public GameObject playerObject;

    //speedmults for animations. These have a slider in the inspector.
    public float runningAnimSpeedMult = 1;
    public float climbingAnimSpeedMult = 1;
    public float attackingAnimSpeed = 1;


    public bool playStoppingAnimation;

    public bool manuallySetFilePaths = false;

    public string assetPath;
    public AnimatorController manualAnimC;
    public string animControllerPath;

    AnimatorController animC;

    bool setJumpState = false;
    int jumpState = 0;
    int ladderState;
    int attackState;

    int ladderAttackDir;

    //TEMPORARY!!!
    Text txtDebug;

    void Start () {
        //set up unit.
		_unit = gameObject.GetComponent<GameUnit> ();
        //animList = gameObject.GetComponeonts<Animation>();
        //animList = GetComponents<Animation>();
        refreshAnimationList();

        for (int i = 0; i < animC.parameters.Length; i++)
        {
            if (animC.parameters[i].name == "JumpState")
            {
                setJumpState = true;
            }

        }

        //TEMPORARY!!
        txtDebug = GameObject.Find("txt_debug").GetComponent<Text>();

    }

    void refreshAnimationList()
    {
        
        animList = new List<AnimationClip>();
        
        //Debug.Log("-------------------------------------");
        string playerName = playerObject.name;
        string assetPath = "Assets/Assets/" + playerName + "/" + playerName + ".FBX";
        string maskPath = "Assets/Assets/" + playerName + "/" + playerName + ".mask";
        //assetPath = "Assets/Assets/" + playerName + "/";
        //Debug.Log("Searching path:\n" + assetPath);
        Object[] contained;
        contained = AssetDatabase.LoadAllAssetsAtPath(assetPath);
        //Debug.Log("number of objects: " + contained.Length.ToString());

        //grab the animator on the player.
        playerAnimator = playerObject.GetComponent<Animator>();

        plAvMask = AssetDatabase.LoadAssetAtPath<AvatarMask>(maskPath);

        /*
        //plAvMask.SetHumanoidBodyPartActive(AvatarMaskBodyPart.)

        System.Array values = System.Enum.GetValues(typeof(AvatarMaskBodyPart));

        foreach(AvatarMaskBodyPart bp in values)
        {

            plAvMask.SetHumanoidBodyPartActive(bp, true);

        }

        */
        //find the controller asset in the directory
        Object animCObject = AssetDatabase.LoadAssetAtPath("Assets/Assets/AnimControllerTemplate.controller", typeof(AnimatorController));

        //create a controller for use in this scope
        animC = new AnimatorController();

        //check if it exists
        if (animCObject is AnimatorController)
        {
            animC = (AnimatorController)animCObject;
            playerAnimator.runtimeAnimatorController = animC;
            
        }


        //Debug.Log("ANIMAITON LIST For " + playerObject.name + ":");

        //int clipCount = playerAnim.GetClipCount();
        foreach (Object temp in contained)
        {
            //for testing for now...
            //animClipList.Add(playerAnim.Clip);
            if (temp is AnimationClip)
            {
                if (temp.name != "__preview__Take 001")
                {
                    //add animations from files to list.
                    animList.Add((AnimationClip)temp);

                    foreach (AnimatorControllerLayer tempLayer in animC.layers)
                    {
                        foreach (ChildAnimatorState curState in tempLayer.stateMachine.states)
                        {
                            if (curState.state.name == temp.name)
                            {

                                curState.state.motion = (AnimationClip)temp;

                                if (curState.state.name.ToLower().Contains("run"))
                                {
                                    curState.state.speed = runningAnimSpeedMult;

                                } else if (curState.state.name.ToLower().Contains("climb"))
                                {
                                    curState.state.speed = climbingAnimSpeedMult;
                                
                                } else if (curState.state.name.ToLower().Contains("attack"))
                                {

                                    curState.state.speed = attackingAnimSpeed;

                                }
                            
                            }

                        }
                        tempLayer.avatarMask = plAvMask;
                    }
                    
                }
                //Debug.Log("contains animation: " + temp.name);
            } else if (temp is Avatar)
            {

                plAv = (Avatar)temp;

            }

        }
        


    }
	
	// Update is called once per frame
	void Update () {
        if (_unit.unitRB.velocity.y > 0)
        {
            jumpState = 1;
        }
        else if (_unit.unitRB.velocity.y < 0)
        {
            jumpState = 2;
        }
        else {
            jumpState = 0;
        }

        if (_unit.ladderLocked)
        {

            if (_unit.armDir == ArmDir.up)
            {
                ladderState = 1;
                playerAnimator.SetFloat("LadderClimbSpeedMult", 1);
            } else if (_unit.armDir == ArmDir.down)
            {
                ladderState = 2;
                playerAnimator.SetFloat("LadderClimbSpeedMult", -1);
            } else if (_unit.armDir == ArmDir.forwards)
            {
                ladderState = 0;
            }

        }

        if (ladderState == 0)
        {

            if (_unit.legDir == LegDir.right)
            {
                ladderAttackDir = 1;
            }
            else
            {
                ladderAttackDir = 2;
            }

        } else
        {
            ladderAttackDir = 0;

        }

        //attacking direction
        if (_unit.armDir == ArmDir.forwards)
        {
            attackState = 1;
        } else if (_unit.armDir == ArmDir.up)
        {
            attackState = 2;
        } else
        {
            attackState = 3;
        }
        

        //rising, falling, nothing
        if (setJumpState)
        {

            playerAnimator.SetInteger("JumpState", jumpState);

        }

        //char is on ground.
        playerAnimator.SetBool("Grounded", _unit.grounded);

        playerAnimator.SetBool("IsRunning", _unit.moving);

        playerAnimator.SetBool("IsDashing", _unit.dashing);

        playerAnimator.SetBool("OnLadder", _unit.ladderLocked);

        playerAnimator.SetInteger("LadderDirection", ladderState);

        playerAnimator.SetInteger("LadderAttackDir", ladderAttackDir);

        playerAnimator.SetInteger("AttackDirection", attackState);


        StartCoroutine(debugText());
        
        //txtDebug.text = txtDebug.text + "\n" + playerAnimator.GetCurrentAnimatorClipInfoCount(0);



    }
    IEnumerator debugText()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {

            //txtDebug.text = "" + playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            yield return null;
        }

    }

}




[CustomEditor (typeof(AnimationHandler))]
[CanEditMultipleObjects]
public class AnimationHandlerEditor : Editor {

    SerializedProperty playerObject;

    SerializedProperty manualAnimC;

    //SerializedProperty manuallySetFilePaths;
    SerializedProperty assetPath;
    SerializedProperty animControllerPath;

    SerializedProperty runningAnimSpeedMult;
    SerializedProperty climbingAnimSpeedMult;

    SerializedProperty attackingAnimSpeed;

    void OnEnable()
    {

        playerObject = serializedObject.FindProperty("playerObject");
        //manuallySetFilePaths = serializedObject.FindProperty("manuallySetFilePaths");
        assetPath = serializedObject.FindProperty("assetPath");
        manualAnimC = serializedObject.FindProperty("manualAnimC");
        runningAnimSpeedMult = serializedObject.FindProperty("runningAnimSpeedMult");
        climbingAnimSpeedMult = serializedObject.FindProperty("climbingAnimSpeedMult");
        attackingAnimSpeed = serializedObject.FindProperty("attackingAnimSpeed");

    }



    public override void OnInspectorGUI()
	{
        serializedObject.Update();

        //GUILayout.Label ("-------------\nThis is a Label in a Custom Editor\n-------------");
        GUILayout.Box("-------------\nThis is a custom editor for setting up the animations in your Game Unit.\n-------------",GUILayout.ExpandWidth(true));

        //GUILayout.Label("");
        EditorGUILayout.PropertyField(playerObject,new GUIContent("Player Model","You'll find the player model under the player holder ojbect in the player unit."));

        GUILayout.Label("this isn't working yet:");
        //manuallySetFilePaths.boolValue = EditorGUILayout.Toggle("Manually set AC?", manuallySetFilePaths.boolValue);

        /*if (manuallySetFilePaths.boolValue)
        {
            EditorGUILayout.PropertyField(manualAnimC, new GUIContent("Animation Controller", "Drag the custom animation controller here if you need one."));
        }*/

        GUILayout.Box ("Change animation speeds here", GUILayout.ExpandWidth(true));

        climbingAnimSpeedMult.floatValue = EditorGUILayout.Slider("Climb anim speed:",climbingAnimSpeedMult.floatValue, .0f, 10.0f);
        runningAnimSpeedMult.floatValue = EditorGUILayout.Slider("Run anim speed:",runningAnimSpeedMult.floatValue, .0f, 10.0f);
        attackingAnimSpeed.floatValue = EditorGUILayout.Slider("Attack anim speed:", attackingAnimSpeed.floatValue, .0f, 10.0f);

        GUILayout.Box("-------------------------", GUILayout.ExpandWidth(true));
        GUILayout.Space(60);

        serializedObject.ApplyModifiedProperties();
    }
}
