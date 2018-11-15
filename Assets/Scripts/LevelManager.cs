using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelManager : MonoBehaviour {

    public GameObject btnLvPrefab,btnLvContainer;
    // Use this for initialization
    void Start() {
        if (!File.Exists(Application.persistentDataPath + "/1.png"))
        {
#if UNITY_ANDROID
            for (int i = 1; i <= 18; i++)
            {
                var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + i + ".png");  // this is the path to your StreamingAssets in android
                while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                                            // then save to Application.persistentDataPath
                File.WriteAllBytes(Application.persistentDataPath + "/" + i + ".png", loadDb.bytes);
            }
#endif
#if UNITY_EDITOR
            for (int i = 1; i <= 18; i++)
            {
                File.Copy(@"Assets/StreamingAssets/"+i+".png", Application.persistentDataPath + "/" + i + ".png");
            }
#endif
        }
        /*#if UNITY_EDITOR
                    var dbPath = Application.persistentDataPath;
        #else
                        // check if file exists in Application.persistentDataPath
                        var dbPath = Application.persistentDataPath;
                        if (!File.Exists(dbPath+"/1.png"))
                        {
                            Debug.Log("Previews not in Persistent path");
                            // if it doesn't ->
                            // open StreamingAssets directory and load the db ->
        #if UNITY_ANDROID
                               for (int i = 1; i <=18; i++)
                                {
                                    var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/"+i+".png");  // this is the path to your StreamingAssets in android
                                    while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                                    // then save to Application.persistentDataPath
                                    File.WriteAllBytes(dbPath+"/"+i+".png", loadDb.bytes);
                                }                       
        #endif
                        }
        #endif*/
        for (int i = 1; i <=18; i++)
        {
            GameObject btn=Instantiate(btnLvPrefab, btnLvContainer.transform);
            btn.GetComponentInChildren<Text>().text = i.ToString();
            btn.GetComponent<Image>().sprite = LoadSprite(Application.persistentDataPath + "/" + i + ".png");
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
    private Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f),100);
            return sprite;
        }
        return null;
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
