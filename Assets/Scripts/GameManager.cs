using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
    public Text txtLevel,txtCountDown,txtTotalStar;
    public StarSlider starSlider;
    public GameObject FullWaterEffect, EditorButton;
    [Header("Hint")]
    public GameObject HintLine;
    public Button UseHint, BuyHint;
    public Text HintNumTxt;
    public List<GameObject> listLevel;
    public GameStatus GameStatus=GameStatus.WAITING;
    public static bool showHint = false;
    public static int totalLevel = 84;
    int numCup = 0;
    List<LineRenderer> listHintLine = new List<LineRenderer>();
	// Use this for initialization
	void Start () {
        //PlayerPrefs.SetInt("totalStar", 20);
        txtTotalStar.text = PlayerPrefs.GetInt("totalStar", 0).ToString();
        txtLevel.text = PlayerPrefs.GetInt("curLevel", 1).ToString();
        Instantiate(listLevel[PlayerPrefs.GetInt("curLevel", 1) - 1], transform);
        starSlider.setThreeStarLength(GetComponentInChildren<LevelInfo>().ThreeStarLength);
        if (showHint)
        {
            showHint = false;
            string[] allLine=new string[0];
#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/hint" + PlayerPrefs.GetInt("curLevel", 1) + ".txt");  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            if (string.IsNullOrEmpty(loadDb.error))
                allLine = Regex.Split(loadDb.text, "\r\n");
#endif
#if UNITY_EDITOR
            if(File.Exists(Application.streamingAssetsPath + "/hint" + PlayerPrefs.GetInt("curLevel", 1) + ".txt"))
                allLine = File.ReadAllLines(Application.streamingAssetsPath + "/hint"+ PlayerPrefs.GetInt("curLevel", 1) + ".txt");
#endif
            for (int i = 0; i < allLine.Length; i++)
            {
                allLine[i] = allLine[i].Trim();
                string[] arr=allLine[i].Split(' ');
                LineRenderer hintLine = Instantiate(HintLine, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
                listHintLine.Add(hintLine);
                hintLine.positionCount = 0;
                for (int j = 0; j < arr.Length/2; j++)
                {
                    hintLine.positionCount++;
                    hintLine.SetPosition(hintLine.positionCount - 1, new Vector2(float.Parse(arr[j * 2]), float.Parse(arr[j * 2 + 1])));
                }
            }
        }
        SceneTransition.Instance.Out();
        #if UNITY_EDITOR
            EditorButton.SetActive(true);
        #endif
    }
    public void giftBoxClick()
    {
        StartCoroutine(giftBoxClickIEnumerator());
    }
    IEnumerator giftBoxClickIEnumerator()
    {
        yield return new WaitForSeconds(0);
    }
    public void showHintBoard()
    {
        if (PlayerPrefs.GetInt("hintNum", 5) == 0)
            UseHint.interactable = false;
        if (PlayerPrefs.GetInt("totalStar", 0) < 10)
            BuyHint.interactable = false;
        HintNumTxt.text = PlayerPrefs.GetInt("hintNum", 5).ToString();
        Time.timeScale = 0;
        GetComponent<AudioSource>().Pause();
    }
    public void hideHintBoard()
    {
        Time.timeScale = 1;
        GetComponent<AudioSource>().UnPause();
    }
    public void UsehintClick()
    {
        PlayerPrefs.SetInt("hintNum", PlayerPrefs.GetInt("hintNum", 5) -1);
        HintNumTxt.text = PlayerPrefs.GetInt("hintNum", 5).ToString();
        UseHint.interactable = false;
        showHint = true;
        Time.timeScale = 1;
        ReplayClick();
    }
    public void BuyHintClick()
    {
        if(PlayerPrefs.GetInt("totalStar", 0) >= 10)
        {
            PlayerPrefs.SetInt("hintNum", PlayerPrefs.GetInt("hintNum", 5) + 1);
            PlayerPrefs.SetInt("totalStar", PlayerPrefs.GetInt("totalStar", 0) - 10);
            if (PlayerPrefs.GetInt("totalStar", 0) < 10)
                BuyHint.interactable = false;
            txtTotalStar.text = PlayerPrefs.GetInt("totalStar", 0).ToString();
            HintNumTxt.text = PlayerPrefs.GetInt("hintNum", 5).ToString();
            UseHint.interactable = true;
        }
        else
        {
            
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HomeClick();
        }
        if (GameStatus == GameStatus.PLAYING && listHintLine.Count > 0)
        {
            if (!DOTween.IsTweening(listHintLine[0]))
            {
                foreach (LineRenderer l in listHintLine)
                {
                    l.DOColor(new Color2(l.startColor,l.endColor), new Color2(Color.clear,Color.clear), 0.5f);
                    Destroy(l.gameObject, 0.5f);
                }
                listHintLine.Clear();
            }
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
        if (PlayerPrefs.GetInt("curLevel", 1) < totalLevel)
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
        Debug.Log("replay");
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
        {
            PlayerPrefs.SetInt("totalStar", PlayerPrefs.GetInt("totalStar", 0) + starSlider.starNum - PlayerPrefs.GetInt("star_lv" + PlayerPrefs.GetInt("curLevel", 1), 0));
            txtTotalStar.text = PlayerPrefs.GetInt("totalStar", 0).ToString();
            PlayerPrefs.SetInt("star_lv" + PlayerPrefs.GetInt("curLevel", 1), starSlider.starNum); //Lưu sao
        }
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
