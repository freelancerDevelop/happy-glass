using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public GameObject FullWaterEffect;
    public List<GameObject> listLevel;
    
    GameStatus GameStatus=GameStatus.PLAYING;
    int numCup = 0;
	// Use this for initialization
	void Start () {
        txtLevel.text = PlayerPrefs.GetInt("curLevel",1).ToString();
        Instantiate(listLevel[PlayerPrefs.GetInt("curLevel", 1) - 1],transform);
        starSlider.setThreeStarLength(GetComponentInChildren<LevelInfo>().ThreeStarLength);
        SceneTransition.Instance.Out();
	}
  
    public void ReplayClick()
    {
        SceneTransition.Instance.LoadScene("MainGame", TransitionType.FadeToBlack);
    }
    public void HomeClick()
    {
        SceneTransition.Instance.LoadScene("Menu", TransitionType.FadeToBlack);
    }
    public void DayNuoc(Vector2 cupPosition)
    {
        Destroy(Instantiate(FullWaterEffect, new Vector2(cupPosition.x, cupPosition.y+0.5f), Quaternion.identity),3);
        numCup++;
        if(numCup==gameObject.GetComponentsInChildren<Cup>().Length)
            StartCoroutine(VictoryIEnumerator());
    }
    IEnumerator VictoryIEnumerator()
    {
        if (PlayerPrefs.GetInt("curLevel", 1) == PlayerPrefs.GetInt("LevelOpen", 1))
            PlayerPrefs.SetInt("LevelOpen", PlayerPrefs.GetInt("LevelOpen", 1) + 1); //mở lv
        if (starSlider.starNum > PlayerPrefs.GetInt("star_lv" + PlayerPrefs.GetInt("curLevel", 1), 0))
            PlayerPrefs.SetInt("star_lv" + PlayerPrefs.GetInt("curLevel", 1), starSlider.starNum); //Lưu sao
        VictoryManager.numStar = starSlider.starNum;
        GameStatus = GameStatus.VICTORY;
        txtCountDown.DOKill();
        txtCountDown.DOFade(0f, 0.3f);
        //Save preview
        yield return new WaitForSeconds(0.15f);
        yield return new WaitForEndOfFrame();
        Texture2D tex = new Texture2D(Screen.width, Screen.width);
        tex.ReadPixels(new Rect(0, (Screen.height-Screen.width)/2, Screen.width, Screen.width),0,0);
        tex.Apply();
        File.WriteAllBytes(Application.persistentDataPath + "/" + PlayerPrefs.GetInt("curLevel", 1) + ".png", tex.EncodeToPNG());
        yield return new WaitForSeconds(2f);
        SceneTransition.Instance.LoadScene("Victory", TransitionType.WaterLogo);
    }
    public void CountDown()
    {
        StartCoroutine(CountDownIEnumerator());
    }
    IEnumerator CountDownIEnumerator()
    {
        for (int i = 8; i >=1; i--)
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
