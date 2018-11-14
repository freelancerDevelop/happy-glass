using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public GameObject btnLvPrefab,btnLvContainer;
	// Use this for initialization
	void Start () {
        for (int i = 1; i <=12; i++)
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
    }
    void btnLvClick(int lv)
    {
        Debug.Log(lv);
        PlayerPrefs.SetInt("curLevel", lv);
        SceneManager.LoadScene("MainGame");
    }
    public void homeClick()
    {
        SceneManager.LoadScene("Menu");
    }
}
