using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour {
    public GameObject Board,StarBust,Confetti;
    public Text txtLevel;
    public static int numStar=0;
    IEnumerator Start()
    {
        txtLevel.text = PlayerPrefs.GetInt("curLevel", 1).ToString();
        for (int i = 0; i < numStar; i++)
        {
            StartCoroutine(StarExplosion(i));
            yield return new WaitForSeconds(0.3f);
        }
    }
    IEnumerator StarExplosion(int i)
    {
        yield return new WaitForSeconds(2);
        Board.transform.GetChild(i).GetChild(0).DOScale(1, 0.8f).SetEase(Ease.OutBounce);
        Destroy(Instantiate(StarBust, Board.transform.GetChild(i).position, Quaternion.identity), 3);
        if (i == 2)
        {
            yield return new WaitForSeconds(0.3f);
            Destroy(Instantiate(Confetti, Board.transform.position+Vector3.up*1.8f, Quaternion.identity), 3);
        }
    }
    public void NextLevel()
    {
        if (PlayerPrefs.GetInt("curLevel", 1) < 12)
        {
            PlayerPrefs.SetInt("curLevel", PlayerPrefs.GetInt("curLevel", 1)+1);
            SceneManager.LoadScene("MainGame");
        }
        else
        {
            SceneManager.LoadScene("ChooseLevel");
        }
    }
    public void HomeClick()
    {
        SceneManager.LoadScene("Menu");
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void ChooseLevel()
    {
        SceneManager.LoadScene("ChooseLevel");
    }
}
