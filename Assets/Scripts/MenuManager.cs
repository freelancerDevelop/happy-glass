using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Application.targetFrameRate = 60;
        SceneTransition.Instance.Out();
    }
	
	public void StartClick()
    {
        //SceneManager.LoadScene("MainGame");
        SceneTransition.Instance.LoadScene("MainGame",TransitionType.WaterLogo);
    }
    public void ChooseLevelClick()
    {
        SceneTransition.Instance.LoadScene("ChooseLevel", TransitionType.FadeToBlack);
    }
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }
}
