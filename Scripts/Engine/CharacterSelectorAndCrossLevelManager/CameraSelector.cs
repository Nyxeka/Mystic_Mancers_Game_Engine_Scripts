using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CameraSelector : MonoBehaviour {

    //------------------------
    // Camera Handler for Character Selector Scene
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    public float lerpDelay = 0.5f;

    Vector3 lookAtTarget;

    Vector3 lookAtTargetDestination;

    public GameObject[] characters;
    int curPlayer = 0;

    string charName = "";

    //public bool okToInit;

    SelectedCharacterHandler SCH;

    public UnityEvent onStart;

    // Use this for initialization
    /*void Awake () {
        Debug.Log("Attempting to start intialization co-routine");
        StartCoroutine("initialize");

    }*/

        // for input grabbing later
    float timer = 0;

    public float switchDelay = 0.5f;

    void Awake()
    {
        GameObject SCHGO;
        if (SCHGO = GameObject.FindGameObjectWithTag("SCH"))
        {
            SCH = SCHGO.GetComponent<SelectedCharacterHandler>();
        }

        if (SCH == null)
            Debug.Log("Warning! No SCH found");
        else
            Debug.Log("SCH found");
        lookAtTarget = new Vector3();

        lookAtTargetDestination = new Vector3();

        if (characters.Length == 0)
        {
            characters = GameObject.FindGameObjectsWithTag("Player");
        }

        //Debug.Log(characters.Length.ToString());

        charName = characters[curPlayer].name;
        updateSCH();

        lookAtTargetDestination = characters[curPlayer].transform.position;

        if (lerpDelay == 0)
        {

            lerpDelay = 0.5f;

        }

    }

    IEnumerator initialize()
    {
        Debug.Log("Started intialization co-routine");
        yield return new WaitForSeconds(0.75f);
        Debug.Log("Looking for SCH");
        GameObject SCHGO;
        if (SCHGO = GameObject.FindGameObjectWithTag("SCH"))
        {
            SCH = SCHGO.GetComponent<SelectedCharacterHandler>();
        }
        
        if (SCH == null)
            Debug.Log("Warning! No SCH found");
        else
            Debug.Log("SCH found");
        lookAtTarget = new Vector3();

        lookAtTargetDestination = new Vector3();

        if (characters.Length == 0)
        {
            characters = GameObject.FindGameObjectsWithTag("Player");
        }

        //Debug.Log(characters.Length.ToString());

        charName = characters[curPlayer].name;
        updateSCH();

        lookAtTargetDestination = characters[curPlayer].transform.position;
        

        //Debug.Log("Initialized co-routine for camera");

    }

    int Mod(int a, int b)
    {
        return (a % b + b) % b;
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log("updating change: " + lookAtTarget.ToString() + ", " + lookAtTargetDestination.ToString() + ", " + lerpDelay.ToString() + ", " + Time.deltaTime);
        lookAtTarget = Vector3.Lerp(lookAtTarget, lookAtTargetDestination, lerpDelay * Time.deltaTime);
        transform.LookAt(lookAtTarget);

        timer += Time.deltaTime;
        if (Input.GetAxis("Horizontal") > 0)
        {
            if (timer > switchDelay)
            {
                nextTarget();
                timer = 0;
            }
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (timer > switchDelay)
            {
                prevTarget();
                timer = 0;
            }
        }

        if (Input.GetButtonDown("Submit"))
        {

            onStart.Invoke();

        }

    }

    void updateSCH()
    {
        if (SCH)
        {
            SCH.setCurrentSelectedChar(charName);
            Debug.Log("Updated SCH char name to: " + charName);
        } else
        {
            Debug.Log("SCH doesn't exist");
        }

    }

    public void nextTarget()
    {

        curPlayer = Mod((curPlayer + 1), characters.Length);
        Debug.Log("PlayerSelectIndex: " + curPlayer.ToString());
        lookAtTargetDestination = characters[curPlayer].transform.position;
        charName = characters[curPlayer].name;
        Debug.Log("Selected Character Name: " + characters[curPlayer].name + lookAtTargetDestination.ToString());
        updateSCH();
    }

    public void prevTarget()
    {

        curPlayer = Mod((curPlayer - 1), characters.Length);
        Debug.Log("PlayerSelectIndex: " + curPlayer.ToString());
        lookAtTargetDestination = characters[curPlayer].transform.position;
        charName = characters[curPlayer].name;
        Debug.Log("Selected Character Name: " + characters[curPlayer].name + lookAtTargetDestination.ToString());
        updateSCH();
    }

}
