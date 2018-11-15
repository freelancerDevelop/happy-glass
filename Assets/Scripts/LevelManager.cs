using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public GameObject btnLvPrefab,btnLvContainer;
	// Use this for initialization
	void Start () {

/*#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", "tung");
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);
        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

        #if UNITY_ANDROID
                    var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
                    while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                    // then save to Application.persistentDataPath
                    File.WriteAllBytes(filepath, loadDb.bytes);
        #endif
#endif*/
        for (int i = 1; i <=18; i++)
        {
            GameObject btn=Instantiate(btnLvPrefab, btnLvContainer.transform);
            btn.GetComponentInChildren<Text>().text = i.ToString();
            int lvIdx = i;
            btn.GetComponent<Button>().onClick.AddListener(delegate { btnLvClick(lvIdx); });
            if(i<PlayerPrefs.GetInt("LevelOpen", 1))
            {
                btn.transform.GetChild(3).gameObject.SetActive(false);
            }
            else if (i == PlayerPrefs.GetInt("LevelOpen", 1))
            {
                btn.transform.GetChild(6).gameObject.SetActive(false);
                btn.transform.GetChild(3).gameObject.SetActive(false);
            }
            else if (i > PlayerPrefs.GetInt("LevelOpen", 1))
            {
                btn.GetComponent<Button>().interactable = false;
                btn.transform.GetChild(6).gameObject.SetActive(false);
            }
            for (int j = 0; j < PlayerPrefs.GetInt("star_lv"+i, 0); j++)
            {
                btn.transform.GetChild(j).GetChild(0).gameObject.SetActive(true);
            }
        }
        SceneTransition.Instance.Out();
    }
    void btnLvClick(int lv)
    {
        Debug.Log(lv);
        PlayerPrefs.SetInt("curLevel", lv);
        SceneTransition.Instance.LoadScene("MainGame", TransitionType.WaterLogo);
    }
    public void homeClick()
    {
        SceneTransition.Instance.LoadScene("Menu", TransitionType.FadeToBlack);
    }
}
