using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour {

    public void NextLevel()
    {
        if (PlayerPrefs.GetInt("curLevel", 1) < 12)
        {
            PlayerPrefs.SetInt("curLevel", PlayerPrefs.GetInt("curLevel", 1)+1);
            SceneManager.LoadScene("MainGame");
        }
        else
        {
            SceneManager.LoadScene("ChooseLevel");
        }

        
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void ChooseLevel()
    {
        SceneManager.LoadScene("ChooseLevel");
    }
}
