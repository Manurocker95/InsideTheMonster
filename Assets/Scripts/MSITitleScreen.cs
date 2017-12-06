using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MSITitleScreen : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
    void GoToGame()
    {
        SceneManager.LoadScene(1);
    }

	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Return))
        {
            GoToGame();
        }
	}
}
