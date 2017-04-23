using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationHandler : MonoBehaviour {

    //------------------------
    // Automated Animation Handler
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    Animation playerAnim;

    List<AnimationClip> animList;

    AnimationClip idle;
    AnimationClip idleVariant;

	GameUnit _unit;

    Animator playerAnimator;
    
    RuntimeAnimatorController runAnimC;

    AnimatorOverrideController overC;
    
    public GameObject playerObject;

    //speedmults for animations. These have a slider in the inspector.
    public float runningAnimSpeedMult = 1;
    public float climbingAnimSpeedMult = 1;
    public float attackingAnimSpeed = 1;
    bool updateRunAnim;
    bool updateClimbAnim;
    bool updateAttackAnim;

    public bool playStoppingAnimation;

    public bool manuallySetFilePaths = false;

    public string assetPath;

    public string animControllerName = "PlayerAnimControllerTemplate";
    
    int jumpState = 0;
    int ladderState;
    int attackState;

    int ladderAttackDir;

    public bool manualAnimController;

    //TEMPORARY!!!
    //Text txtDebug;

    bool hasUnit;

    int numIdles;

    void Start() {
        // find the unit component
        if (_unit = gameObject.GetComponent<GameUnit>())
        {
            hasUnit = true;
        }
        
        
        refreshAnimationList();

        //TEMPORARY!!
        /*if (txtDebug = GameObject.Find("txt_debug").GetComponent<Text>())
        {
            StartCoroutine(debugText());

        }*/
        if (hasUnit)
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

        //Debug.Log("number of objects: " + contained.Length.ToString());

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

                if (temp.name.ToLower().Contains("climb"))
                {
                    updateClimbAnim = true;
                    
                }

                if (temp.name.ToLower().Contains("attack"))
                {
                    updateAttackAnim = true;
                }

                if (temp.name.ToLower().Contains("run"))
                {
                    updateRunAnim = true;
                }

                if (temp.name.ToLower().Contains("idle") && !temp.name.ToLower().Contains("ladder"))
                {

                    numIdles += 1;

                    if (idle == null)
                    {

                        idle = (AnimationClip)temp;

                    } else
                    {

                        idleVariant = (AnimationClip)temp;

                    }

                }
                
            }
            //Debug.Log("contains animation: " + temp.name);
        }
        if (hasUnit)
            StartCoroutine(updatePlayerAnimSpeeds());

        if (numIdles > 1)
        {

            StartCoroutine(SwitchIdle());

        }


    }

    IEnumerator updatePlayerAnimSpeeds()
    {

        // bool updated;

        //Debug.Log("Upating player animation speeds" + updateRunAnim.ToString() + updateClimbAnim.ToString() + updateAttackAnim.ToString());

        while (!playerAnimator.isInitialized)
        {
            yield return null;

        }
        playerAnimator.SetFloat("AttackingSpeedMult", GetComponent<AnimSpeedPrefs>().attackingAnimSpeed);
        playerAnimator.SetFloat("LadderClimbSpeedMult", GetComponent<AnimSpeedPrefs>().climbingAnimSpeedMult);
        playerAnimator.SetFloat("RunningSpeedMult", GetComponent<AnimSpeedPrefs>().runningAnimSpeedMult);
        /*
#if UNITY_EDITOR
        if (updateRunAnim)
            playerAnimator.SetFloat("AttackingSpeedMult", attackingAnimSpeed);

        if (updateClimbAnim)
            playerAnimator.SetFloat("LadderClimbSpeedMult", climbingAnimSpeedMult);
        
        if (updateAttackAnim)
            playerAnimator.SetFloat("RunningSpeedMult", runningAnimSpeedMult);

        INIWorker.IniWriteValue("Animation Speeds", playerObject.name + "AttackingSpeedMult", attackingAnimSpeed.ToString());
        INIWorker.IniWriteValue("Animation Speeds", playerObject.name + "LadderClimbSpeedMult", climbingAnimSpeedMult.ToString());
        INIWorker.IniWriteValue("Animation Speeds", playerObject.name + "RunningSpeedMult", runningAnimSpeedMult.ToString());

#endif

        float newAttackAnimSpeed;
        float newClimbAnimSpeed;
        float newRunAnimSpeed;
        

        if (float.TryParse(INIWorker.IniReadValue("Animation Speeds",playerObject.name + "AttackingSpeedMult"),out newAttackAnimSpeed))
        {

            playerAnimator.SetFloat("AttackingSpeedMult", attackingAnimSpeed);

        }
        if (float.TryParse(INIWorker.IniReadValue("Animation Speeds", playerObject.name + "LadderClimbSpeedMult"), out newClimbAnimSpeed))
        {

            playerAnimator.SetFloat("LadderClimbSpeedMult", newClimbAnimSpeed);

        }
        if (float.TryParse(INIWorker.IniReadValue("Animation Speeds", playerObject.name + "RunningSpeedMult"), out newRunAnimSpeed))
        {

            playerAnimator.SetFloat("AttackingSpeedMult", newRunAnimSpeed);

        }*/

    }

    IEnumerator SwitchIdle()
    {
        bool variant = false;
        while (true)
        {

            if (Mathf.Floor(Random.Range(0,3)) == 0)
            {

                overC["Idle"] = idleVariant;
                variant = true;

            } else if (overC["Idle"] != idle)
            {
                variant = false;
                overC["Idle"] = idle;

            }
            if (variant)
            {
                yield return new WaitForSeconds(idleVariant.length);

            } else
            {
                yield return new WaitForSeconds(idle.length);

            }
            yield return null; //wait another frame just in case. We don't want an infinite loop.

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
                    playerAnimator.SetFloat("LadderClimbSpeedMult", climbingAnimSpeedMult);
                }
                else if (_unit.armDir == ArmDir.down)
                {
                    ladderState = 2;
                    playerAnimator.SetFloat("LadderClimbSpeedMult", -climbingAnimSpeedMult);
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

}
