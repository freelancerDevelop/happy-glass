using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static int curLevel = 2;
    public Text txtLevel;
    public StarSlider starSlider;
    public List<GameObject> listLevel;
	// Use this for initialization
	void Start () {
       
        txtLevel.text = curLevel.ToString();
        Instantiate(listLevel[curLevel - 1],transform);
        starSlider.setThreeStarLength(GetComponentInChildren<LevelInfo>().ThreeStarLength);
	}
    
    public void ReplayClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void HomeClick()
    {
        SceneManager.LoadScene("Home");
    }

}
