﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour {
    public GameObject Board,StarBust,Confetti;
    public Text txtTotalStar;
    public Text txtLevel;
    public static int numStar=0;
    bool clicked = false;
    IEnumerator Start()
    {
        txtTotalStar.text = PlayerPrefs.GetInt("totalStar", 0).ToString();
        SceneTransition.Instance.Out();
        if (Random.Range(0, 100) < 90)
        {
            AdManager.Ins.ShowInterstitial();
        }
        txtLevel.text = PlayerPrefs.GetInt("curLevel", 1).ToString();
        for (int i = 0; i < numStar; i++)
        {
            StartCoroutine(StarExplosion(i));
            yield return new WaitForSeconds(0.3f);
        }
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HomeClick();
        }
    }
  
    IEnumerator StarExplosion(int i)
    {
        yield return new WaitForSeconds(2);
        //AudioManager.Instance.Play("star_explosion");
        GetComponents<AudioSource>()[0].Play();
        Board.transform.GetChild(i).GetChild(0).DOScale(1, 0.8f).SetEase(Ease.OutBounce);
        Destroy(Instantiate(StarBust, Board.transform.GetChild(i).position, Quaternion.identity), 3);
        if (i == 2)
        {
            yield return new WaitForSeconds(0.3f);
            //AudioManager.Instance.Play("congratulation");
            GetComponents<AudioSource>()[1].Play();
            Destroy(Instantiate(Confetti, Board.transform.position+Vector3.up*1.8f, Quaternion.identity), 3);
        }
    }
    public void NextLevel()
    {
        if (PlayerPrefs.GetInt("curLevel", 1) < GameManager.totalLevel && !clicked)
        {
            PlayerPrefs.SetInt("curLevel", PlayerPrefs.GetInt("curLevel", 1) + 1);
            PlayAgain();
            clicked = true;
        } 
        else if (!clicked)
        {
            ChooseLevel();
        }
    }
    public void HomeClick()
    {
        SceneTransition.Instance.LoadScene("Menu", TransitionType.FadeToBlack);
    }
    public void PlayAgain()
    {
        SceneTransition.Instance.LoadScene("MainGame", TransitionType.WaterLogo);
    }
    public void ChooseLevel()
    {
        SceneTransition.Instance.LoadScene("ChooseLevel", TransitionType.WaterLogo);
    }
}
