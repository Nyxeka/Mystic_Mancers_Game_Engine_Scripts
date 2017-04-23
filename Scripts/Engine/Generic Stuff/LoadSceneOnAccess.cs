using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnAccess : MonoBehaviour {

    public string newSceneName;

	public void loadNewScene()
    {

        SceneManager.LoadSceneAsync(newSceneName);

    }

    public void loadCustomScene(Scene newScene)
    {

        SceneManager.LoadSceneAsync(newScene.name);

    }

    public void loadCustomSceneBuildNumber(int buildNum)
    {

        SceneManager.LoadSceneAsync(buildNum);

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
