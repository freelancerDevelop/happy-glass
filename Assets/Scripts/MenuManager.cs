using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Application.targetFrameRate = 60;
    }
	
	public void StartClick()
    {
        SceneManager.LoadScene("MainGame");
    }
}
