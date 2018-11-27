using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameStatus
{
    WAITING,
    PLAYING,
    PAUSE,
    VICTORY,
    GAMEOVER
}
public class GameManager : MonoBehaviour {
    public Text txtLevel,txtCountDown;
    public StarSlider starSlider;
    public GameObject FullWaterEffect,EditorButton;
    public List<GameObject> listLevel;
    public GameStatus GameStatus=GameStatus.WAITING;
    public static bool showHint = true;
    int numCup = 0;
	// Use this for initialization
	void Start () {
        txtLevel.text = PlayerPrefs.GetInt("curLevel", 1).ToString();
        Instantiate(listLevel[PlayerPrefs.GetInt("curLevel", 1) - 1], transform);
        starSlider.setThreeStarLength(GetComponentInChildren<LevelInfo>().ThreeStarLength);
        if (showHint)
        {

        }
        SceneTransition.Instance.Out();
        #if UNITY_EDITOR
            EditorButton.SetActive(true);
        #endif
    }
    public void showHintBoard()
    {
        Time.timeScale = 0;
        GetComponent<AudioSource>().Pause();
    }
    public void hideHintBoard()
    {
        Time.timeScale = 1;
        GetComponent<AudioSource>().UnPause();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HomeClick();
        }
    }
    public void waterFall()
    {
        GetComponents<AudioSource>()[0].Play();
    }
    public void ChooseLevel()
    {
        SceneTransition.Instance.LoadScene("ChooseLevel", TransitionType.FadeToBlack);
    }
    public void NextLevel()
    {
        if (PlayerPrefs.GetInt("curLevel", 1) < 34)
        {
            PlayerPrefs.SetInt("curLevel", PlayerPrefs.GetInt("curLevel", 1) + 1);
            ReplayClick();
        }
        else
        {
            ChooseLevel();
        }
    }
    public void SavePeviewClick()
    {
        StartCoroutine(TakeScreenShot(Application.streamingAssetsPath));
    }

    public void ReplayClick()
    {
        if (GameStatus == GameStatus.PLAYING||GameStatus == GameStatus.WAITING)
            SceneTransition.Instance.LoadScene("MainGame", TransitionType.FadeToBlack);
    }
    public void HomeClick()
    {
        if(GameStatus == GameStatus.PLAYING || GameStatus == GameStatus.WAITING)
            SceneTransition.Instance.LoadScene("Menu", TransitionType.FadeToBlack);
    }
    public void DayNuoc(Vector2 cupPosition)
    {
        GetComponents<AudioSource>()[1].Play();
        Destroy(Instantiate(FullWaterEffect, new Vector2(cupPosition.x, cupPosition.y+0.5f), Quaternion.identity),3);
        numCup++;
        if (numCup == gameObject.GetComponentsInChildren<Cup>().Length)
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
        yield return new WaitForSeconds(0.12f);
        StartCoroutine(TakeScreenShot(Application.persistentDataPath));
        yield return new WaitForSeconds(2f);
        //
        SceneTransition.Instance.LoadScene("Victory", TransitionType.WaterLogo);
    }
    private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        float incX = (1.0f / (float)targetWidth);
        float incY = (1.0f / (float)targetHeight);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / (float)result.width, (float)i / (float)result.height);
                result.SetPixel(j, i, newColor);
            }
        }
        result.Apply();
        return result;
    }
    IEnumerator TakeScreenShot(string path)
    {
        yield return new WaitForEndOfFrame();
        Texture2D tex = new Texture2D(Screen.width, Screen.width);
        tex.ReadPixels(new Rect(0, (Screen.height - Screen.width) / 2, Screen.width, Screen.width), 0, 0);
        tex = ScaleTexture(tex, 300, 300);
        tex.Apply();
        File.WriteAllBytes(path + "/" + PlayerPrefs.GetInt("curLevel", 1) + ".jpg", tex.EncodeToJPG(90));
        Debug.Log("Save to:" + path);

    }
    public void CountDown()
    {
        StartCoroutine(CountDownIEnumerator());
    }
    IEnumerator CountDownIEnumerator()
    {
        for (int i = 7; i >=1; i--)
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
