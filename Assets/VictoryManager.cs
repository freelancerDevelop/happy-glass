using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour {

    public void NextLevel()
    {
        GameManager.curLevel++;
        SceneManager.LoadScene("MainGame");
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("MainGame");
    }
}
