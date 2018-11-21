using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TransitionType
{
    WaterLogo=0,
    FadeToBlack=1,
    FadeToWhite=2
}
public class SceneTransition : MonoBehaviour {
    public static SceneTransition Instance;
    static TransitionType activeTransitionType=TransitionType.FadeToBlack;
    bool isTransition = true;
    public void LoadScene(string s,TransitionType transitionType)
    {
        Debug.Log("SceneLoad");
        if (!isTransition)
        {
            activeTransitionType = transitionType;
            StartCoroutine(In(s));
        }
    }
    IEnumerator In(string s)
    {
        isTransition = true;
        if (activeTransitionType == TransitionType.WaterLogo)
        {
            transform.GetChild(0).GetComponent<Animator>().SetTrigger("in");
            yield return new WaitForSeconds(1);
        }
        if (activeTransitionType == TransitionType.FadeToBlack)
        {
            transform.GetChild(1).GetComponent<Animator>().SetTrigger("in");
            yield return new WaitForSeconds(0.34f);
        }
        if (activeTransitionType == TransitionType.FadeToWhite)
        {
            transform.GetChild(2).GetComponent<Animator>().SetTrigger("in");
            yield return new WaitForSeconds(0.34f);
        }
        SceneManager.LoadScene(s);
    }
    public void Out()
    {
        if (isTransition)
        {
            //if(activeTransitionType == TransitionType.WaterLogo) AudioManager.Instance.Play("SceneIn");
            transform.GetChild((int)activeTransitionType).GetComponent<Animator>().SetTrigger("out");
            isTransition = false;
        }
            
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
}
