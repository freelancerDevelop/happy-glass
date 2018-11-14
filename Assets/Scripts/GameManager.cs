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
    public Text txtLevel,txtCountDown;
    public StarSlider starSlider;
    public List<GameObject> listLevel;
    GameStatus GameStatus=GameStatus.PLAYING;
    int numCup = 0;
	// Use this for initialization
	void Start () {
        txtLevel.text = PlayerPrefs.GetInt("curLevel",1).ToString();
        Instantiate(listLevel[PlayerPrefs.GetInt("curLevel", 1) - 1],transform);
        starSlider.setThreeStarLength(GetComponentInChildren<LevelInfo>().ThreeStarLength);
	}
    public void ClearAllData()
    {
        PlayerPrefs.DeleteAll();
    }
    public void ReplayClick()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void HomeClick()
    {
        SceneManager.LoadScene("Menu");
    }
    public void DayNuoc()
    {
        numCup++;
        if(numCup==gameObject.GetComponentsInChildren<Cup>().Length)
            StartCoroutine(VictoryIEnumerator());
    }
    IEnumerator VictoryIEnumerator()
    {
        if (PlayerPrefs.GetInt("curLevel", 1) == PlayerPrefs.GetInt("LevelOpen", 1)) PlayerPrefs.SetInt("LevelOpen", PlayerPrefs.GetInt("curLevel", 1) + 1); //mở lv
        if (starSlider.starNum > PlayerPrefs.GetInt("star_lv" + PlayerPrefs.GetInt("curLevel", 1), 0))
            PlayerPrefs.SetInt("star_lv" + PlayerPrefs.GetInt("curLevel", 1), starSlider.starNum); //Lưu sao
        GameStatus = GameStatus.VICTORY;
        txtCountDown.DOKill();
        txtCountDown.DOFade(0f, 0.3f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Victory");
    }
    public void CountDown()
    {
        StartCoroutine(CountDownIEnumerator());
    }
    IEnumerator CountDownIEnumerator()
    {
        for (int i = 10; i >=1; i--)
        {
            txtCountDown.text = i.ToString();
            if (i == 3 && GameStatus != GameStatus.VICTORY) txtCountDown.DOFade(0.7f, 0.1f);
            yield return new WaitForSeconds(1f);
        }
        txtCountDown.DOFade(0f, 0.3f);
        yield return new WaitForSeconds(1f);
        if (GameStatus != GameStatus.VICTORY)
        {
            ReplayClick();
        }
            
    }
}
