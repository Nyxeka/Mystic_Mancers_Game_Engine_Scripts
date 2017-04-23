using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterDelay : MonoBehaviour {

    public float delay = 1.0f;

    public string newSceneToLoad;

    public Scene newScene;
    
	void Start () {

        StartCoroutine(RunAfterDelay());

	}
	
	IEnumerator RunAfterDelay()
    {

        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(newSceneToLoad);

    }
}
