using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public GameObject droplet;
    public Button btnAudio;
    public Sprite audioOn, audioOff;
	// Use this for initialization
	void Start () {
        Application.targetFrameRate = 60;
        if (PlayerPrefs.GetInt("audio", 1) == 1)
        {
            btnAudio.GetComponent<Image>().sprite = audioOn;
            AudioListener.volume = 1;
        }
        else
        {
            btnAudio.GetComponent<Image>().sprite = audioOff;
            AudioListener.volume = 0;
        }
        
        SceneTransition.Instance.Out();
        StartCoroutine(waterFall());
    }
    IEnumerator waterFall()
    {
        for (int i = 0; i < 35; i++)
        {
            GameObject obj = Instantiate(droplet,new Vector3(-0.5f,-0.5f,0), Quaternion.identity, transform);
            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1, 0.3f)*5);
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void AudioClick()
    {
        PlayerPrefs.SetInt("audio", -PlayerPrefs.GetInt("audio", 1));
        if (PlayerPrefs.GetInt("audio", 1) == 1)
        {
            btnAudio.GetComponent<Image>().sprite = audioOn;
            AudioListener.volume = 1;
        }
        else
        {
            btnAudio.GetComponent<Image>().sprite = audioOff;
            AudioListener.volume = 0;
        }
    }
    public void StartClick()
    {
        //SceneManager.LoadScene("MainGame");
        
        SceneTransition.Instance.LoadScene("MainGame",TransitionType.WaterLogo);
    }
    public void ChooseLevelClick()
    {
        SceneTransition.Instance.LoadScene("ChooseLevel", TransitionType.WaterLogo);
    }
    public void ClearAllData()
    {
        DirectoryInfo dataDir = new DirectoryInfo(Application.persistentDataPath);
        dataDir.Delete(true);
        PlayerPrefs.DeleteAll();
    }
}
