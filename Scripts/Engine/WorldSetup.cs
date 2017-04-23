using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WorldSetup : MonoBehaviour {

    //------------------------
    // Background World Setup Script
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------
    

    float grav = -9.8f;

    public float gravMult = 3.0f;

	bool paused;

	float defaultTimeScale;

	public UnityEvent pauseStart;
	public UnityEvent pauseEnd;

    bool spawnPlayer;

    GameObject[] spawnGOList; // list of checkpoints

    CheckPoint currentSpawn;

    public int curCheckpoint; // current check-point.
    
    GameObject _character;
    string charName;

    List<CheckPoint> spawnList;

    bool ready;

    bool first = true;

    public HashSet<Transform> pickups;

    SelectedCharacterHandler SCH;

    // Use this for initialization
    void Start () {
        // set up level gravity
        defaultTimeScale = 1;
        paused = false;
        spawnPlayer = false;
        Vector3 newGrav = new Vector3(.0f, grav*gravMult, .0f);
        Physics.gravity = newGrav;
        ready = false;
        
        //get our list of checkpoints.
        spawnGOList = GameObject.FindGameObjectsWithTag("Checkpoint");

        spawnList = getCheckpointList();

        //then we want to sort the list.

        GameObject schGO;

        if (schGO = GameObject.FindGameObjectWithTag("SCH"))
        {

            SCH = schGO.GetComponent<SelectedCharacterHandler>();

        }
        
        if (_character = GameObject.FindGameObjectWithTag("Player"))
        {
           
           
        } else
        {

            //wait for the CharacterSelectorHandler to spawn one.
            spawnPlayer = true;

        }

        ready = true;

	}

    public void PauseGame()
    {

        pauseStart.Invoke();

    }

    public void UnPauseGame()
    {

        pauseEnd.Invoke();
        Time.timeScale = defaultTimeScale;
        paused = false;

    }

    public void resetLevel()
    {

        if (SCH != null)
        {

            SCH.resetScore();
            SCH.curCheckPoint = 0;
            curCheckpoint = 0;

        }
        UnPauseGame();

    }

    public GameObject character
    {

        get
        {

            return _character;

        }

    }

    List<CheckPoint> getCheckpointList()
    {
        //This is where we're inserting the new sorted checkpoint into the list
        int insertIndex = 0;
        List<CheckPoint> newList = new List<CheckPoint>();
        
        if (spawnGOList.Length > 1)
        {
            //spawn list is relatively small, so we're just gonna use a simple selection sort algorithm

            for (int i = 0;i < spawnGOList.Length; i++)
            {
                insertIndex = 0;
                foreach(CheckPoint j in newList)
                {

                    if (spawnGOList[i].GetComponent<CheckPoint>().IDCount > j.IDCount)
                    {
                        //put it into the list
                        insertIndex++;

                    }

                }
                newList.Insert(insertIndex, spawnGOList[i].GetComponent<CheckPoint>());
            }

        }else if (spawnGOList.Length == 1)
        {
            newList.Add(spawnGOList[0].GetComponent<CheckPoint>());
        }

        // add a point to this function. We want to increase the checkpoint when the player hits one.
        foreach(CheckPoint b in newList)
        {

            b.addCheck += increaseCheckpoint;

        }

        return newList;

    }

    public void increaseCheckpoint()
    {

        if (!first)
        {
            Debug.Log("Hit checkpoint. Increasing checkpointCount");
            curCheckpoint = curCheckpoint + 1;
            if (curCheckpoint > spawnList.Count - 1)
            {

                curCheckpoint = spawnList.Count - 1;

            }
            // and, just in case there's only one checkpoint...
            curCheckpoint = curCheckpoint % spawnList.Count;
            Debug.Log("Checkpoint set to: " + spawnList[curCheckpoint].IDCount.ToString());
        } else
        {
            first = false;
        }

    }

    public void SetCurrentSpawnPoint(int ID)
    {

        curCheckpoint = ID;

    }

    public void spawnCharacter(string newChar)
    {
        charName = newChar;
        Debug.Log("Tried to load player: " + newChar);
        StartCoroutine(spawnChar());
    }

    IEnumerator spawnChar()
    {
        // wait until checkpoints have been found and sorted
        Debug.Log("Waiting until checkpoints are sorted and loaded.");
        yield return new WaitUntil(() => (ready == true));


        if (_character != null)
        {
            GameObject.Destroy(_character);
            spawnPlayer = true;
        }

        if (spawnPlayer)
        {
            Debug.Log("Spawned Character.");
            // spawn a new player. Hopefully all the serialized variables hold.
            _character = Instantiate((GameObject)Resources.Load("Characters/" + charName), spawnList[curCheckpoint].transform.position, spawnList[curCheckpoint].transform.rotation);
        }

    }

	void Update(){

		if (Input.GetButtonDown ("Escape") && !paused) {
			
			Time.timeScale = 0;
			//StartCoroutine (waitForUnpause ());
			paused = true;
			pauseStart.Invoke ();
			Debug.Log ("Pausing Game");
			
		} else if (Input.GetButtonDown ("Escape") && paused) {

			Time.timeScale = defaultTimeScale;
			paused = false;
			pauseEnd.Invoke ();
			Debug.Log ("Unpausing Game");
		}
	}

    void OnDisable()
    {
        if (spawnList != null)
        {
            if (spawnList.Count > 0)
            {
                Debug.Log("Removing Delegates from spawnlist.");
                foreach (CheckPoint i in spawnList)
                {

                    i.addCheck -= increaseCheckpoint;

                }
            }
        }

    }


}
