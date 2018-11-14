using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameStatus
{
    PLAYING,
    VICTORY,
    GAMEOVER
}
public class GameManager : MonoBehaviour {

    public static int curLevel = 2;
    public Text txtLevel,txtCountDown;
    public StarSlider starSlider;
    public List<GameObject> listLevel;
    GameStatus GameStatus=GameStatus.PLAYING;
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
        SceneManager.LoadScene("Menu");
    }
    public void DayNuoc()
    {
        //QuaMan;

    }
    IEnumerator VictoryIEnumerator()
    {
        if (curLevel + 1 > listLevel.Count) SceneManager.LoadScene("Menu");
        else if (curLevel==PlayerPrefs.GetInt("LevelOpen", 1)) PlayerPrefs.SetInt("LevelOpen", curLevel + 1);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Victory");
        
    }
    public void CountDown()
    {
        txtCountDown.DOFade(0.7f, 0.5f).SetDelay(2f);
        StartCoroutine(CountDownIEnumerator());
    }
    IEnumerator CountDownIEnumerator()
    {
        for (int i = 5; i >=1; i--)
        {
            txtCountDown.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        txtCountDown.DOFade(0f, 0.5f);
        yield return new WaitForSeconds(1f);
        if (GameStatus != GameStatus.VICTORY)
            ReplayClick();
    }
}
