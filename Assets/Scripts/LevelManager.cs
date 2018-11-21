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
        for (int i = 1; i <=34; i++)
        {
            GameObject btn=Instantiate(btnLvPrefab, btnLvContainer.transform);
            btn.GetComponentInChildren<Text>().text = i.ToString();
            btn.GetComponent<Image>().sprite = LoadPreview(i);
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
                #if !UNITY_EDITOR
                    btn.GetComponent<Button>().interactable = false;
                #endif
                btn.transform.GetChild(6).gameObject.SetActive(false);
            }
            for (int j = 0; j < PlayerPrefs.GetInt("star_lv"+i, 0); j++)
            {
                btn.transform.GetChild(j).GetChild(0).gameObject.SetActive(true);
            }
        }
        SceneTransition.Instance.Out();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            homeClick();
        }
    }
    Sprite LoadPreview(int lv)
    {
        if (!File.Exists(Application.persistentDataPath + "/"+lv+".jpg"))
        {
#if UNITY_ANDROID
                var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + lv + ".jpg");  // this is the path to your StreamingAssets in android
                while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                if(string.IsNullOrEmpty(loadDb.error))
                    File.WriteAllBytes(Application.persistentDataPath + "/" + lv + ".jpg", loadDb.bytes);
#endif
#if UNITY_EDITOR
            if(File.Exists(@"Assets/StreamingAssets/" + lv + ".jpg"))
                File.Copy(@"Assets/StreamingAssets/" + lv + ".jpg", Application.persistentDataPath + "/" + lv + ".jpg", true);
#endif
        }
        if (File.Exists(Application.persistentDataPath + "/" + lv + ".jpg"))
        {
            byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + "/" + lv + ".jpg");
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100);
            return sprite;
        }
        return null;
    }
    void btnLvClick(int lv)
    {
        GetComponent<AudioSource>().Play();
        PlayerPrefs.SetInt("curLevel", lv);
        SceneTransition.Instance.LoadScene("MainGame", TransitionType.WaterLogo);
    }
    public void homeClick()
    {
        SceneTransition.Instance.LoadScene("Menu", TransitionType.FadeToBlack);
    }
}
