using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SelectedCharacterHandler : MonoBehaviour {

    //------------------------
    // Character Spawner&Handle carrying stuff between levels
    // By: Nicholas J. Hylands
    // me@nickhylands.com
    // github.com/nyxeka
    //------------------------

    [HideInInspector]
    public GameObject chosenCharacter;
    
    string currentSelectedChar;

    string sceneName;
    string lastSceneName;

    [HideInInspector]
    public int curCheckPoint = 0;

    // used for collectables
    HashSet<string> pickups;

    GameObject[] alivePickups;

    WorldSetup newLevelController;
    string currentSceneName;
    string oldSceneName;

    void Start()
    {

        pickups = new HashSet<string>();

        StartCoroutine(updateScoreForUI());

    }

    bool printScore = false;

    int score = 0;
    int lastScore = 0;

    int printedScore = 0;

    public AudioClip scoreIncreaseSound;

    public Font myFont;

    public Vector3 pickupSoundPlayOffset;

    public float pickupIncrement = 0.1f;

    Vector3 cameraPos;

    GameObject _cam;

    Text scoreText;

    private void init(Scene previousScene, Scene newScene)
    {
        Time.timeScale = 1;
        currentSceneName = SceneManager.GetActiveScene().name;
        //Debug.Log("Initializing Selected Character Handler for current scene.");
        newLevelController = new WorldSetup();
        GameObject worldSetup;
        //myFont.
        lastScore = score;

        GameObject scoreGO;

        if (scoreGO = GameObject.FindGameObjectWithTag("ScoreText"))
        {

            scoreGO.GetComponent<Text>();
            //Debug.Log("Found the score text game object!!!!!!!!!!!!!!!!!!!");
        }
        else
        {

            //Debug.Log("Couldn't find score text game object!!");
        }

        StopCoroutine("UpdateScore");
        

        if (worldSetup = GameObject.FindGameObjectWithTag("GameController"))
        {
            // assign it and if it's not null it will let the if statement go through 
            if (newLevelController = worldSetup.GetComponent<WorldSetup>())
            {
                
                if (currentSceneName == oldSceneName)
                {

                    newLevelController.curCheckpoint = curCheckPoint;

                } else
                {

                    pickups = null;
                    pickups = new HashSet<string>();

                }
                
                newLevelController.spawnCharacter(currentSelectedChar);
                StartCoroutine(getCheckpoint());
                StartCoroutine("UpdateScore");
                printScore = true;
            }
            else
            {
                Debug.Log("No World Setup Script found!");

            }
        } else
        {
            Debug.Log("No background script server found!");
            printScore = false;

        }

        _cam = GameObject.FindGameObjectWithTag("MainCamera");
        
        // check for collectables:

        alivePickups = GameObject.FindGameObjectsWithTag("Collectable");

        foreach(GameObject g in alivePickups)
        {

            if (pickups.Contains(CrystalBits.GetCustomCrystalHash(g)))
            {

                GameObject.Destroy(g);

            }

        }

        

        //scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();

        //possibly no poiint:
        /*if (newLevelController != null)
        {
            newLevelController.character.GetComponent<PlayerController>().setSCH(this);
        }*/

        oldSceneName = currentSceneName;

    }

    public void resetScore()
    {

        score = lastScore;
        printedScore = score;
        

    }

    void Update()
    {

        if (_cam != null)
        {

            cameraPos = _cam.transform.position;

        }

    }

    IEnumerator updateScoreForUI()
    {
        
        //float defaultIncrement = pickupIncrement;

        float increment = pickupIncrement;

        if (increment == 0)
        {

            increment = 0.1f;

        }

        float targetIncrement = 0.08f;

        float lerpTime = 0.5f;

        int oldScore = 0;

        bool counting = false;

        while (true)
        {
            if (oldScore != score && counting == false)
            {

                oldScore = score;
                counting = true;
                increment = pickupIncrement;
                //score has changed, this means we want to start counting.
            }


            if (counting)
            {

                yield return new WaitForSeconds(increment);

                if (printedScore < score)
                {

                    printedScore++;
                    if (newLevelController != null && scoreIncreaseSound != null)
                    {
                        AudioSource.PlayClipAtPoint(scoreIncreaseSound, cameraPos + pickupSoundPlayOffset,1.0f);
                        
                    }
                    //increment = Mathf.Lerp(increment, targetIncrement, lerpTime);

                } else
                {

                    counting = false;

                }

            }

            
            yield return null;
        }

    }

    IEnumerator UpdateScore()
    {
        int oldScore = 0;
        
        if (scoreText == null)
        {

            scoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>();

        }
        scoreText.text = "Score: " + printedScore.ToString();
        while (true)
        {

            yield return new WaitUntil(() => oldScore != printedScore);
            oldScore = printedScore;
            //scoreText.text = "test";
            scoreText.text = "Score: " + printedScore.ToString();
                
            yield return null;
        }
    }

    /*void OnGUI()
    {
        if (printScore)
        {
            if (myFont != null)
            {

                GUI.skin.font = myFont;

            }
            //GUILayout.Label("Score: " + score.ToString());
            GUI.Label(new Rect(10, 10, 200, 200), "Score: " + printedScore.ToString());
        }

    }*/

    public void addPickup(string newPickup)
    {
        //print("ID: " + newPickup);
        if (!pickups.Contains(newPickup))
        {
            
            pickups.Add(newPickup);
            score++;
        }
    }

    public void setCurrentSelectedChar(string newChar)
    {

        currentSelectedChar = newChar;
        
    }

    IEnumerator getCheckpoint()
    {
        while (true)
        {
            yield return new WaitUntil(() => curCheckPoint != newLevelController.curCheckpoint);
            curCheckPoint = newLevelController.curCheckpoint;
        }
    }

    void Awake()
    {
        
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        Time.timeScale = 1.0f;
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.activeSceneChanged += init;
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= init; // unsubscribe
    }

}
