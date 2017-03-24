using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationHandler : MonoBehaviour {

	/*
	 * Animation Handler for Units.
	 * 
	 */

    Animation playerAnim;

    List<AnimationClip> animList;

	GameUnit _unit;

    Animator playerAnimator;
    
    RuntimeAnimatorController runAnimC;

    AnimatorOverrideController overC;


    public GameObject playerObject;

    //speedmults for animations. These have a slider in the inspector.
    public float runningAnimSpeedMult = 1;
    public float climbingAnimSpeedMult = 1;
    public float attackingAnimSpeed = 1;

    public bool playStoppingAnimation;

    public bool manuallySetFilePaths = false;

    public string assetPath;

    public string animControllerName = "PlayerAnimControllerTemplate";
    
    int jumpState = 0;
    int ladderState;
    int attackState;

    int ladderAttackDir;

    //TEMPORARY!!!
    Text txtDebug;

    void Start() {
        //set up unit.
        _unit = gameObject.GetComponent<GameUnit>();
        //animList = gameObject.GetComponeonts<Animation>();
        //animList = GetComponents<Animation>();
        refreshAnimationList();

        //TEMPORARY!!
        if (txtDebug = GameObject.Find("txt_debug").GetComponent<Text>())
        {
            StartCoroutine(debugText());

        }

        StartCoroutine(updateAnimStatePlayer());

    }

    void refreshAnimationList()
    {

        animList = new List<AnimationClip>();
        
        //Debug.Log("-------------------------------------");
        string playerName = playerObject.name;
        //string assetPath = "Assets/" + playerName + "/" + playerName + ".FBX";
        //string maskPath = "Assets/" + playerName + "/" + playerName + ".mask";
        //assetPath = "Assets/Assets/" + playerName + "/";
        //Debug.Log("Searching path:\n" + assetPath);

        

        AnimationClip[] contained;


        contained = Resources.LoadAll<AnimationClip>(playerName);

        //contained = AssetDatabase.LoadAllAssetsAtPath(assetPath);

        Debug.Log("number of objects: " + contained.Length.ToString());

        //grab the animator on the player.
        playerAnimator = playerObject.GetComponent<Animator>();

        //create a controller for use in this scope
        runAnimC = Resources.Load<RuntimeAnimatorController>(animControllerName);

        overC = new AnimatorOverrideController();

        overC.runtimeAnimatorController = runAnimC;
        
        playerAnimator.runtimeAnimatorController = overC;

        //Debug.Log("ANIMAITON LIST For " + playerObject.name + ":");

        //int clipCount = playerAnim.GetClipCount();
        foreach (Object temp in contained)
        {
            //for testing for now...
            //animClipList.Add(playerAnim.Clip);
            
            if (temp.name != "__preview__Take 001")
            {
                //add animations from files to list.
                animList.Add((AnimationClip)temp);

                overC[temp.name] = (AnimationClip)temp;
                
                    
            }
            //Debug.Log("contains animation: " + temp.name);
            
        }


    }

    IEnumerator updateAnimStatePlayer()
    {

        yield return new WaitUntil(() => (_unit.unitRB != null));
        Debug.Log("_unit loaded, starting anim state handler.");
        while (true)
        {

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
                }
                else if (_unit.armDir == ArmDir.down)
                {
                    ladderState = 2;
                    playerAnimator.SetFloat("LadderClimbSpeedMult", -1);
                }
                else if (_unit.armDir == ArmDir.forwards)
                {
                    ladderState = 0;
                    playerAnimator.SetFloat("LadderClimbSpeedMult", 0);
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

            }
            else
            {
                ladderAttackDir = 0;

            }

            //attacking direction
            if (_unit.armDir == ArmDir.forwards)
            {
                attackState = 1;
            }
            else if (_unit.armDir == ArmDir.up)
            {
                attackState = 2;
            }
            else
            {
                attackState = 3;
            }


            //rising, falling, nothing
            playerAnimator.SetInteger("JumpState", jumpState);

            //char is on ground.
            playerAnimator.SetBool("Grounded", _unit.grounded);

            playerAnimator.SetBool("IsRunning", _unit.moving);

            playerAnimator.SetBool("IsDashing", _unit.dashing);

            playerAnimator.SetBool("OnLadder", _unit.ladderLocked);

            playerAnimator.SetInteger("LadderDirection", ladderState);

            playerAnimator.SetInteger("LadderAttackDir", ladderAttackDir);

            playerAnimator.SetInteger("AttackDirection", attackState);

            //txtDebug.text = txtDebug.text + "\n" + playerAnimator.GetCurrentAnimatorClipInfoCount(0);

            yield return null;

        }


    }

    IEnumerator debugText()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {

            txtDebug.text = "" + playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            yield return null;
        }

    }

}
